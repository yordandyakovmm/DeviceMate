using DeviceMate.Models.Entities;
using DeviceMate.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Objects.Repositories
{
   
    public class DeviceHoldsHistoryRepo : BaseRepo<DeviceHoldsHistory>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public DeviceHoldsHistoryRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        #region Public methods

        public void DeleteHistoryByDeviceId(int id)
        {
            IEnumerable<DeviceHoldsHistory> lst = GetAll().Where(itm => itm.DeviceId == id).ToList();
            this.Delete(lst);
        }


        public void AddHold(Hold hold)
        {
            DeviceHoldsHistory history = new DeviceHoldsHistory();
            history.HoldDate = hold.HoldDate;
            history.IsBusy = hold.IsBusy;
            history.TownID = hold.TownID;
            history.TeamId = hold.TeamId;            
            history.Email = hold.Email;

            Device device = hold.Devices.FirstOrDefault();
            if(device!=null)
            {
                history.DeviceId = device.Id;

                Add(history);
                this.Context.SaveChanges();
            }
            
        }

        public int SearchCount(string Number, string Name, int? TeamId, string Email, int? TownID, int? DeviceTypeID, string OSVersion)
        {
            return SearchQuery(Number, Name, TeamId, Email, TownID, DeviceTypeID, OSVersion).Count();
        }

        public IEnumerable<DeviceHoldsHistory> Search(string Number, string Name, int? TeamId, string Email, int? TownID, int? DeviceTypeID, string OSVersion, string Expression = null, string Column = null, int Direction = 0, int Page = 1, int PageSize = int.MaxValue)
        {
           var q = SearchQuery(Number, Name, TeamId, Email, TownID, DeviceTypeID, OSVersion);

            var query = q;
            q = q.Order(Expression, Column, Direction, () => query.OrderByDescending(a => a.HoldDate));
            q = q.Paginate(Page, PageSize);

            return q.ToList();
        }

        private IQueryable<DeviceHoldsHistory> SearchQuery(string Number, string Name, int? TeamId, string Email, int? TownID, int? DeviceTypeID, string OSVersion)
        {
            IQueryable<DeviceHoldsHistory> lst = this.Context.DeviceHoldsHistories
               .Include("Town")
               .Include("Team")
               .Include("Device")
               .OrderByDescending(a => a.HoldDate);
            if (!string.IsNullOrWhiteSpace(Number))
            {
                lst = lst.Where(d => d.Device.Number.ToLower() == Number.ToLower().Trim());
            }
            if (!string.IsNullOrWhiteSpace(Name))
            {
                lst = lst.Where(d => d.Device.Name.ToLower().Contains(Name.ToLower().Trim()));
            }
            if (TeamId.HasValue)
            {
                lst = lst.Where(d => d.TeamId == TeamId.Value);
            }
            if (TownID.HasValue && TownID > 0)
            {
                lst = lst.Where(d => d.TownID == TownID.Value);
            }
            if (DeviceTypeID.HasValue && DeviceTypeID > 0)
            {
                lst = lst.Where(d => d.Device.DeviceTypeId == DeviceTypeID);
            }
            if (!string.IsNullOrWhiteSpace(OSVersion))
            {
                lst = lst.Where(d => d.Device.OsVersion.ToLower() == OSVersion.ToLower().Trim());
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                string email = Email.ToLower().Trim();
                string[] tokens = email.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 1)
                {
                    email = string.Join(".", tokens);
                }
                lst = lst.Where(d => d.Email.ToLower().Contains(email));
            }

            return lst;
        }

        public override int GetAllCount()
        {
            return GetAllQuery().Count();
        }

        public IEnumerable<DeviceHoldsHistory> GetAll(int page = 1, int pageSize = int.MaxValue)
        {
            return GetAllQuery()
                .OrderByDescending(a => a.HoldDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
        }

        private IQueryable<DeviceHoldsHistory> GetAllQuery()
        {
            return this.Context.DeviceHoldsHistories
                .Include("Town")
                .Include("Team")
                .Include("Device");
        }
        #endregion        
    }
}
