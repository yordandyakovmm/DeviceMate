using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Helpers;
using DeviceMate.Objects.Proxies;
using System.Data.Entity.Infrastructure;

namespace DeviceMate.Objects.Repositories
{
    public class HoldRepo : BaseRepo<Hold>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public HoldRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        #region Public methods

        public void EditHold(Hold hold, bool saveChanges = true)
        {
            if (hold.TownID == 0)
            {
                hold.TownID = 1;
            }
            this.Context.Holds.Attach(hold);

            DbEntityEntry<Hold> entry = Context.Entry(hold);
            entry.Property(e => e.HoldDate).IsModified = true;
            entry.Property(e => e.TeamId).IsModified = true;
            entry.Property(e => e.Email).IsModified = true;
            entry.Property(e => e.TownID).IsModified = true;
            entry.Property(e => e.IsBusy).IsModified = true;

            if (saveChanges)
                this.Context.SaveChanges();
        }

        public int Delete(int id)
        {
            if (id <= 0) return 0;
            Hold hold = this.Context.Holds.First(h => h.Id == id);
            this.Context.Holds.Remove(hold);
            int rowsAffected = this.Context.SaveChanges();
            return rowsAffected;
        }

        public int ChangeAvailability(int id)
        {
            if (id <= 0) return 0;
            Hold hold = this.Context.Holds.First(h => h.Id == id);
            hold.IsBusy = !hold.IsBusy;

            if (hold.IsBusy)
                MakeOtherUserDevicesWithSameOsAvailable(hold);

            int rowsAffected = this.Context.SaveChanges();
            return rowsAffected;
        }

        public Hold GetById(int id)
        {
            if (id <= 0) return null;
            Hold hold = this.Context.Holds.First(h => h.Id == id);
            return hold;
        }

        private IEnumerable<Hold> GetByUserNameAndOs(string userName, int osId, int currentHoldId)
        {
            var holds = Context.Devices
                .Include("Hold")
                .Include("Model")
                .Include("Model.Manufacturer")
                .Include("Model.Manufacturer.OSs")
                .Where(d => d.Hold.Email == userName && d.Model.Manufacturer.OsId == osId && d.Hold.IsBusy && d.HoldId != currentHoldId)
                .Select(d => d.Hold)
                .ToList();

            return holds;
        }

        public void MakeOtherUserDevicesWithSameOsAvailable(Hold currentHold)
        {
            var device = currentHold.Devices.FirstOrDefault();
            if (device == null)
            {
                return;
            }

            var otherHoldsForSameUserSameOs = GetByUserNameAndOs(currentHold.Email, device.Model.Manufacturer.OsId, currentHold.Id);
            foreach (var hold in otherHoldsForSameUserSameOs)
            {
                if (hold.TeamId == Constants.Teams.AmcomTeamId)
                    continue;

                hold.IsBusy = false;
                EditHold(hold, false);
            }
        }

        #endregion        
    }
}
