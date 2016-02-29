using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DeviceMate.Web.Models
{
    public class DeviceModelJson 
    {
        #region Properties

        public ColorRepo ColorRepo { get; set; }
        public TeamRepo TeamRepo { get; set; }
        public OSRepo OSRepo { get; set; }
        public ModelRepo ModelRepo { get; set; }
        public ManufacturerRepo ManufacturerRepo { get; set; }
        public DeviceTypeRepo DeviceTypeRepo { get; set; }
        public HoldRepo HoldRepo { get; set; }
        public TownRepo TownRepo { get; set; }
        public ResolutionRepo ResolutionRepo { get; set; }
        public ResolutionWidthRepo ResolutionWidthRepo { get; set; }
        public ResolutionHeightRepo ResolutionHeightRepo { get; set; }
        public ScreenSizeRepo ScreenSizeRepo { get; set; }
        public DeviceHoldsHistoryRepo DeviceHistoryRepo { get; set; }
        public DeviceProxy Device { get; set; }
        public IEnumerable<SelectListItem> OS { get; set; }
        public IEnumerable<SelectListItem> Colors { get; set; }
        public IEnumerable<SelectListItem> DeviceTypes { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }
        public IEnumerable<SelectListItem> Models { get; set; }
        public IEnumerable<SelectListItem> Manufacturers { get; set; }
        public IEnumerable<SelectListItem> Availability { get; set; }
        public IEnumerable<SelectListItem> Towns { get; set; }
        public IEnumerable<SelectListItem> ScreenSizes { get; set; }
        public IEnumerable<SelectListItem> ResolutionWidths { get; set; }
        public IEnumerable<SelectListItem> ResolutionHeights { get; set; }
        
        public List<DeviceJson> Devices { get; set; }

        #endregion

        #region Methods

        public DeviceModelJson(DeviceModel deviceModel)
        {
            this.ColorRepo =  deviceModel.ColorRepo;
            this.TeamRepo = deviceModel.TeamRepo;
            this.OSRepo = deviceModel.OSRepo;
            this.ModelRepo = deviceModel.ModelRepo;
            this.ManufacturerRepo = deviceModel.ManufacturerRepo;
            this.DeviceTypeRepo = deviceModel.DeviceTypeRepo;
            this.HoldRepo = deviceModel.HoldRepo;
            this.TownRepo = deviceModel.TownRepo;
            this.ResolutionRepo = deviceModel.ResolutionRepo;
            this.ResolutionWidthRepo = deviceModel.ResolutionWidthRepo;
            this.ResolutionHeightRepo = deviceModel.ResolutionHeightRepo;
            this.ScreenSizeRepo = deviceModel.ScreenSizeRepo;
            this.DeviceHistoryRepo = deviceModel.DeviceHistoryRepo; 
            this.Device = deviceModel.Device;
            this.OS = deviceModel.OS;
            this.Colors = deviceModel.Colors;
            this.DeviceTypes = deviceModel.DeviceTypes; 
            this.Teams =  deviceModel.Teams;
            this.Models = deviceModel.Models;
            this.Manufacturers = deviceModel.Manufacturers;
            this.Availability = deviceModel.Availability;
            this.Towns = deviceModel.Towns;
            this.ScreenSizes = deviceModel.ScreenSizes;
            this.ResolutionWidths = deviceModel.ResolutionWidths;
            this.ResolutionHeights = deviceModel.ResolutionHeights;

            var query = from list in deviceModel.Devices
                       select new DeviceJson(list);

            this.Devices  = query.ToList();
                   
        }

        #endregion

    }

    public class DeviceJson : BaseJson
    {
        public DeviceJson(Device device)
        {
            this.Id = device.Id;
            this.Number = device.Number;
            this.ColorId = device.ColorId;
            this.OsVersion = device.OsVersion;
            this.HoldId = device.HoldId;
            this.ModelId = device.ModelId;
            this.DeviceTypeId = device.DeviceTypeId;
            this.SerialNumber = device.SerialNumber;
            this.Name = device.Name;
            this.ResolutionId = device.ResolutionId;
            this.ScreenSizeId = device.ScreenSizeId;
            
            this.Color = device.Color != null ? device.Color.Name : ""; 
            this.Hold_Team_Name = device.Hold.Team.Name;
            this.Hold_Email = device.Hold.Email;
            this.Hold_IsBusy = device.Hold.IsBusy.ToString();
            this.Hold_Town_Name = device.Hold.Town.Name;
            this.DeviceType_Name = device.DeviceType.Name;

            this.DeviceType_Name = device.DeviceType.Name;
            this.Model_Name = device.Model.Name;
            this.Resolution_Height = device.Resolution != null ? device.Resolution.ResolutionHeightOption.Height.ToString() : "";
            this.Resolution_Width = device.Resolution != null ? device.Resolution.ResolutionWidthOption.Width.ToString() : "";
            this.ScreenSize = device.ScreenSize != null ? device.ScreenSize.Size.ToString() : "";
            this.emailEmployee = device.Hold.Email;
            this.employee = employee;


        }

        public int Id { get; set; }
        public string Number { get; set; }
        public Nullable<int> ColorId { get; set; }
        public string OsVersion { get; set; }
        public int HoldId { get; set; }
        public int ModelId { get; set; }
        public int DeviceTypeId { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string DeviceType_Name { get; set; }
        public string Hold_Town_Name { get; set; }

        
        public Nullable<int> ResolutionId { get; set; }
        public Nullable<int> ScreenSizeId { get; set; }
        
        public string Color { get; set; }
        //public ICollection<DeviceHoldsHistory> DeviceHoldsHistories { get; set; }
        public string Hold_Team_Name { get; set; }
        public string Hold_Email { get; set; }
        public string Hold_IsBusy { get; set; }
        public string DeviceType { get; set; }
        public string Model { get; set; }
        public string Model_Name { get; set; }
        public string Resolution_Height { get; set; }
        public string Resolution_Width { get; set; }
        public string ScreenSize { get; set; }
        
    }

}