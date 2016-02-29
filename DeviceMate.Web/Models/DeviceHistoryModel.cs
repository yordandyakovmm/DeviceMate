using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;

namespace DeviceMate.Web.Models
{
    public class DeviceHistoryModel : BaseModelWithSortingAndPaginationAndColumnSelection<DeviceHoldsHistoryRepo, DeviceHoldsHistory, int?>, IUserModel
    {
        #region Properties
        [Dependency]
        public DeviceRepo DeviceRepo { get; set; }
        [Dependency]
        public DeviceTypeRepo DeviceTypeRepo { get; set; }
        [Dependency]
        public TownRepo TownRepo { get; set; }
        [Dependency]
        public TeamRepo TeamRepo { get; set; }

        public IEnumerable<DeviceHoldsHistory> DeviceHistories { get; set; }

        public IEnumerable<SelectListItem> Towns { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }

        public IEnumerable<SelectListItem> DeviceTypes { get; set; }

        public SearchFilterHistoryModel SearchFilter { get; set; }

        // IUserModel
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        

        #endregion

        #region Methods

        public override void Init(int? id)
        {
            PopulateData();
        }

        public void PopulateData()
        {
            this.Teams = TeamRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            this.Towns = new List<SelectListItem>();
            (this.Towns as List<SelectListItem>).Add(new SelectListItem { Text = "All", Value = "0" });
            (this.Towns as List<SelectListItem>).AddRange(TownRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TownId.ToString() }));

            var deviceTypes = new List<SelectListItem>
            {
                new SelectListItem {Text = "Not selected", Value = string.Empty, Selected = true}
            };
            deviceTypes.AddRange(DeviceTypeRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }));
            this.DeviceTypes = deviceTypes;
            TotalNumberOfDevices = Repo.GetAll().GroupBy(item => item.DeviceId).Count();
        }

        public void GetAllDeviceHistories()
        {
            IEnumerable<DeviceHoldsHistory> list = Repo.GetAll(Pager.Page, Pager.PageSize);
            DeviceHistories = list;
        }

        public int GetSearchedDeviceHistoriesCount()
        {
            return SearchFilter == null 
                ? Repo.GetAllCount() 
                : Repo.SearchCount(SearchFilter.Number, SearchFilter.Name, SearchFilter.TeamId, SearchFilter.Email, SearchFilter.TownID, SearchFilter.DeviceTypeID, SearchFilter.OsVersion);
        }

        public void GetSearchedDeviceHistories()
        {
            if (SearchFilter != null)
            {
                DeviceHistories = Repo.Search(SearchFilter.Number, SearchFilter.Name, SearchFilter.TeamId, SearchFilter.Email, SearchFilter.TownID, SearchFilter.DeviceTypeID, SearchFilter.OsVersion, Sorter.Expression, Sorter.Column, (int)Sorter.Direction, Pager.Page, Pager.PageSize);
            }
            else
            {
                GetAllDeviceHistories();
            }
        }
        #endregion
    }
}