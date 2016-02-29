using DeviceMate.Models.Entities;
using DeviceMate.Objects.Helpers;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DeviceMate.Web.Models
{
    public class HoldModel<T> : BaseModel<HoldRepo, int?>, IUserModel where T : IHoldable
    {
        #region Properties

        [Dependency]
        public TeamRepo TeamRepo { get; set; }
        [Dependency]
        public T HoldedItemRepo { get; set; }
        [Dependency]
        public TownRepo TownRepo { get; set; }
        [Dependency]
        public DeviceHoldsHistoryRepo HistoryRepo { get; set; }
        

        public HoldProxy Hold { get; set; }
        public TownProxy Town { get; set; }

        public IEnumerable<SelectListItem> Teams { get; set; }

        public IEnumerable<SelectListItem> Towns { get; set; }

        public bool IsAdmin { get; set; }
        public string UserName { get; set; }

        #endregion

        #region Methods

        public override void Init(int? holdId)
        {
            this.PopulateData();
            this.Hold = new HoldProxy();
            this.Town = new TownProxy();

            if (holdId.HasValue)
            {
                Hold hold = this.Repo.GetById(holdId.Value);
                this.Hold.Id = hold.Id;
                this.Hold.HoldedItemName = this.HoldedItemRepo.GetNumberByHoldId(hold.Id);
               
                if (hold.Town!=null)
                {
                    this.Town.TownId = hold.Town.TownId;
                    this.Town.Name = hold.Town.Name;

                    foreach (SelectListItem itm in Towns)
                    {
                        if (itm.Value == this.Town.TownId.ToString())
                        {
                            itm.Selected = true;
                        }
                        else
                        {
                            itm.Selected = false;
                        }
                    }
                }
            }

            
        }

        public void PopulateData()
        {
            this.Teams = TeamRepo.GetAll()
                .Select(type => new SelectListItem { Text = type.Name, Value = type.Id.ToString() });


            this.Towns = new List<SelectListItem>();
            //(this.Towns as List<SelectListItem>).Add(new SelectListItem { Text = "Not Specified", Value = "" });
            (this.Towns as List<SelectListItem>).AddRange(TownRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TownId.ToString() }));
            
        }

        public bool DoesHoldItemNumberExist()
        {
            bool doesExist = HoldedItemRepo.GetNumbers().Contains(this.Hold.HoldedItemName.ToLower());
            return doesExist;
        }

        public void Submit()
        {
            Hold currentHold = this.HoldedItemRepo.GetHoldByNumber(this.Hold.HoldedItemName);

            currentHold.HoldDate = DateTime.Now;
            currentHold.Email = this.UserName;
            currentHold.TeamId = this.Hold.TeamId;
            currentHold.TownID = this.Hold.TownId;
            currentHold.IsBusy = true;

            Repo.MakeOtherUserDevicesWithSameOsAvailable(currentHold);
            Repo.EditHold(currentHold, false);
            Repo.SaveChanges();

            HistoryRepo.AddHold(currentHold);
        }

        #endregion        
    }
}