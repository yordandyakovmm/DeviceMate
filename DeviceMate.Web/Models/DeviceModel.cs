using AutoMapper;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace DeviceMate.Web.Models
{
 
    public class DeviceModel : BaseModelWithSortingAndPaginationAndColumnSelection<DeviceRepo, Device, int?>, IUserModel
    {
        #region Properties

        [Dependency]
        public ColorRepo ColorRepo { get; set; }
        [Dependency]
        public TeamRepo TeamRepo { get; set; }
        [Dependency]
        public OSRepo OSRepo { get; set; }
        [Dependency]
        public ModelRepo ModelRepo { get; set; }
        [Dependency]
        public ManufacturerRepo ManufacturerRepo { get; set; }
        [Dependency]
        public DeviceTypeRepo DeviceTypeRepo { get; set; }
        [Dependency]
        public HoldRepo HoldRepo { get; set; }
        [Dependency]
        public TownRepo TownRepo { get; set; }
        [Dependency]
        public ResolutionRepo ResolutionRepo { get; set; }
        [Dependency]
        public ResolutionWidthRepo ResolutionWidthRepo { get; set; }
        [Dependency]
        public ResolutionHeightRepo ResolutionHeightRepo { get; set; }
        [Dependency]
        public ScreenSizeRepo ScreenSizeRepo { get; set; }

        [Dependency]
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
        
        public IEnumerable<Device> Devices { get; set; }

        public SearchFilterModel SearchFilter { get; set; }

        // IUserModel
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }

        #endregion

        #region Methods

        public override void Init(int? id)
        {
            if (id.HasValue)
            {
                Device d = Repo.GetById(id.Value);
                this.Device = new DeviceProxy()
                {
                    Id = d.Id,
                    Number = d.Number,
                    ColorId = d.ColorId,
                    OSVersion = d.OsVersion,
                    DeviceTypeId = d.DeviceTypeId,
                    SerialNumber = d.SerialNumber,
                    Name = d.Name,
                    OsId = d.Model.Manufacturer.OsId,
                    ManufacturerId = d.Model.ManufacturerId,
                    ModelId = d.ModelId,
                    HoldId = d.HoldId,
                    ScreenSizeId = d.ScreenSizeId,
                    ResolutionWidthId = d.Resolution != null ? (int?)d.Resolution.ResolutionWidthId : null,
                    ResolutionHeightId = d.Resolution != null ? (int?)d.Resolution.ResolutionHeightId : null,
                    TownId = d.Hold.TownID
                };

                this.PopulateData(this.Device.OsId, this.Device.ManufacturerId);
                this.OS.First(os => os.Value == this.Device.OsId.ToString()).Selected = true;
                this.Manufacturers.First(manufacturer => manufacturer.Value == this.Device.ManufacturerId.ToString()).Selected = true;
                this.Models.First(model => model.Value == this.Device.ModelId.ToString()).Selected = true;
                this.DeviceTypes.First(type => type.Value == this.Device.DeviceTypeId.ToString()).Selected = true;
 
                //this.Availability.Add(new SelectListItem{ Text = "Available", Value = "1" });
                

                if (this.Device.ColorId != null)
                {
                    this.Colors.First(color => color.Value == this.Device.ColorId.ToString()).Selected = true;
                }

                if (this.Device.TownId.HasValue && this.Device.TownId.Value > 0)
                {
                    this.Towns.First(town => town.Value == this.Device.TownId.ToString()).Selected = true;
                }

                if (this.Device.ResolutionWidthId.HasValue)
                {
                    this.ResolutionWidths.First(width => width.Value == this.Device.ResolutionWidthId.ToString()).Selected = true;
                }

                if (this.Device.ResolutionHeightId.HasValue)
                {
                    this.ResolutionHeights.First(height => height.Value == this.Device.ResolutionHeightId.ToString()).Selected = true;
                }

                if (this.Device.ScreenSizeId.HasValue)
                {
                    this.ScreenSizes.First(sz => sz.Value == this.Device.ScreenSizeId.ToString()).Selected = true;
                }
            }
            else
            {
                Device = new DeviceProxy();
                this.PopulateData();
            }
        }

        public void PopulateData(int? osId = null, int? manufacturerId = null)
        {
            List<SelectListItem> deviceTypes = new List<SelectListItem>();
            deviceTypes.Add(new SelectListItem { Text = "Not selected", Value = string.Empty, Selected = true });
            deviceTypes.AddRange(DeviceTypeRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }));
            this.DeviceTypes = deviceTypes;

            this.OS = OSRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            this.Colors = ColorRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });

            this.Teams = TeamRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });

            this.Towns = TownRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TownId.ToString() });

            this.ScreenSizes =
                ScreenSizeRepo.GetAll()
                    .Select(sz => new SelectListItem {Text = sz.Size.ToString("0.##"), Value = sz.Id.ToString() });

            this.ResolutionWidths =
                ResolutionWidthRepo.GetAll()
                    .Select(x => new SelectListItem { Text = x.Width.ToString(), Value = x.Id.ToString() });


            this.ResolutionHeights =
                ResolutionHeightRepo.GetAll()
                    .Select(x => new SelectListItem { Text = x.Height.ToString(), Value = x.Id.ToString() });

            if (osId.HasValue)
            {
                this.Manufacturers = ManufacturerRepo.GetAll().Where(m => m.OsId == osId)
                   .Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            }
            else
            {
                this.Manufacturers = new List<SelectListItem>();
            }

            if (manufacturerId.HasValue)
            {
                this.Models = ModelRepo.GetAll().Where(m => m.ManufacturerId == manufacturerId)
                    .Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            }
            else
            {
                this.Models = new List<SelectListItem>();
            }
            List<SelectListItem> l = new List<SelectListItem>();
            l.Add(new SelectListItem { Text = "All", Value = "" });
            l.Add(new SelectListItem { Text = "Available", Value = "1" });
            l.Add(new SelectListItem { Text = "Not Available", Value = "0" });
            this.Availability = l.AsEnumerable();

            TotalNumberOfDevices = Repo.GetAllCount();
        }

        public void SaveDevice()
        {
            this.Device.Number = this.Device.Number.Trim();

            if (Device.Id.HasValue)
            {
                var data = Mapper.Map<Device>(Device);
                SetDeviceResoulution(data);
                if (data.HoldId != 0)
                {
                    data.Hold = HoldRepo.GetById(data.HoldId);
                    data.Hold.TownID = this.Device.TownId.HasValue ? this.Device.TownId.Value : 1;
                }

                Repo.Save(data);
            }
            else
            {
                try
                {
                    Device deviceEntity = new Device()
                    {
                        Number = this.Device.Number,
                        ColorId = this.Device.ColorId,
                        OsVersion = this.Device.OSVersion,
                        ModelId = this.Device.ModelId.Value,
                        DeviceTypeId = this.Device.DeviceTypeId,
                        SerialNumber = this.Device.SerialNumber,
                        Name = this.Device.Name,
                        ScreenSizeId = this.Device.ScreenSizeId,
                    };


                    SetDeviceResoulution(deviceEntity);

                    Hold dh = new Hold();
                    dh.TownID = this.Device.TownId.HasValue ? this.Device.TownId.Value : 1;
                    dh.HoldDate = DateTime.Now;
                    dh.Email = UserName;
                    dh.TeamId = this.UserRepo.Get(u => u.Email == this.UserName).Select(u => u.TeamId.Value).FirstOrDefault();
                    deviceEntity.Hold = dh;
                    Repo.Create(deviceEntity);
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        private void SetDeviceResoulution(Device device)
        {
            if (!this.Device.ResolutionWidthId.HasValue || !this.Device.ResolutionHeightId.HasValue)
            {
                return;
            }

            var resolution = ResolutionRepo.GetByWidthIdHeightId(this.Device.ResolutionWidthId.Value, this.Device.ResolutionHeightId.Value);
            if (resolution != null)
            {
                device.ResolutionId = resolution.Id;
                return;
            }

            if (1 != ResolutionRepo.Create(
                new Resolution
                {
                    ResolutionWidthId = this.Device.ResolutionWidthId.Value,
                    ResolutionHeightId = this.Device.ResolutionHeightId.Value
                }
            )) return;

            device.ResolutionId = ResolutionRepo.GetByWidthIdHeightId(this.Device.ResolutionWidthId.Value, this.Device.ResolutionHeightId.Value).Id;

        }

        public bool SubmitDevice()
        {
            bool res = false;
            Device dev = null;
            if (this.Device.Id.HasValue)
            {
                dev = Repo.GetById(this.Device.Id.Value);
            }
            else
            {
                if (string.IsNullOrEmpty(Device.Number) == false)
                {
                    dev = Repo.GetByNumber(Device.Number);
                }
            }
            if (dev != null)
            {
                Hold dh = new Hold();
                dh.HoldDate = DateTime.Now;
                dh.Email = UserName;
                dh.TeamId = this.UserRepo.Get(u => u.Email == this.UserName).Select(u => u.TeamId.Value).FirstOrDefault();
                dh.TownID = this.Device.TownId.HasValue ? this.Device.TownId.Value : 1;
                dev.Hold = dh;
                Repo.SaveChanges();
                res = true;
            }
            return res;
        }

        public void Delete(int id)
        {
            this.DeviceHistoryRepo.DeleteHistoryByDeviceId(id);
            int holdId = this.Repo.GetHoldIdById(id);
            this.Repo.Delete(id);
            this.HoldRepo.Delete(holdId);
        }

        public void GetAllDevices()
        {
            IEnumerable<Device> list = Repo.GetAll(Pager.Page, Pager.PageSize);
            Devices = list;
        }

        public void GetAllDevicesJson()
        {
            IEnumerable<Device> list = Repo.GetAllJson();
            Devices = list;
        }

        public int GetSearchDevicesCount()
        {
            return SearchFilter == null
                ? Repo.GetAllCount()
                : Repo.SearchCount(this.SearchFilter.Number, this.SearchFilter.TypeId, this.SearchFilter.ModelId, this.SearchFilter.ManufacturerId,
                    this.SearchFilter.OsId, this.SearchFilter.Name, this.SearchFilter.ColorId, this.SearchFilter.TeamId,
                    this.SearchFilter.SerialNumber, this.SearchFilter.Email, this.SearchFilter.OSVersion, this.SearchFilter.AvailableID, this.SearchFilter.TownID,
                    this.SearchFilter.ScreenSizeId, this.SearchFilter.ResolutionWidthId, this.SearchFilter.ResolutionHeightId);
        }

        public void GetSearchedDevices()
        {
            if (SearchFilter == null)
            {
                GetAllDevices();
            }
            else
            {
                Devices = Repo.Search(this.SearchFilter.Number, this.SearchFilter.TypeId, this.SearchFilter.ModelId, this.SearchFilter.ManufacturerId,
                    this.SearchFilter.OsId, this.SearchFilter.Name, this.SearchFilter.ColorId, this.SearchFilter.TeamId,
                    this.SearchFilter.SerialNumber, this.SearchFilter.Email, this.SearchFilter.OSVersion, this.SearchFilter.AvailableID, this.SearchFilter.TownID,
                    this.SearchFilter.ScreenSizeId, this.SearchFilter.ResolutionWidthId, this.SearchFilter.ResolutionHeightId,
                    Sorter.Expression, Sorter.Column, (int)Sorter.Direction, Pager.Page, Pager.PageSize);
            }
        }

        public void GetSearchedDevicesJson()
        {
            if (SearchFilter == null)
            {
                GetAllDevicesJson();
            }
            else
            {
                Devices = Repo.Search(this.SearchFilter.Number, this.SearchFilter.TypeId, this.SearchFilter.ModelId, this.SearchFilter.ManufacturerId,
                    this.SearchFilter.OsId, this.SearchFilter.Name, this.SearchFilter.ColorId, this.SearchFilter.TeamId,
                    this.SearchFilter.SerialNumber, this.SearchFilter.Email, this.SearchFilter.OSVersion, this.SearchFilter.AvailableID, this.SearchFilter.TownID,
                    this.SearchFilter.ScreenSizeId, this.SearchFilter.ResolutionWidthId, this.SearchFilter.ResolutionHeightId,
                    null, null, 0, 0, 0);
            }
        }

        public bool IsAccessoryNameTaken()
        {
            HashSet<string> deviceNumbers = this.Repo.GetNumbers();
            if (this.Device.Id.HasValue)
            {
                string oldNumer = this.Repo.GetNumberById(this.Device.Id.Value);
                deviceNumbers.Remove(oldNumer.ToLower());
            }

            bool isTaken = deviceNumbers.Contains(this.Device.Number.ToLower().Trim());
            return isTaken;
        }

        internal void IsAvailable(int? id)
        {
            HoldRepo.ChangeAvailability(id.Value);
        }

        #endregion


    }
}