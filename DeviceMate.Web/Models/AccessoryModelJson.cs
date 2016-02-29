using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Web.Models
{
    public class AccessoryModelJson
    {
        #region Properties
               
        public AccessoryTypeRepo AccessoryTypeRepo { get; set; }
        public AccessoryDescriptionRepo AccessoryDescriptionRepo { get; set; }
        public List<AccessoryJson> Accessories { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public int TownId { get; set; }
        
        #endregion

        #region Methods

        public AccessoryModelJson(AccessoryModel accessoryModel)
        {
            this.AccessoryTypeRepo = accessoryModel.AccessoryTypeRepo;
            this.AccessoryDescriptionRepo = accessoryModel.AccessoryDescriptionRepo;
            this.IsAdmin = accessoryModel.IsAdmin;
            this.UserName = accessoryModel.UserName;
            this.TownId = accessoryModel.TownId;

            var query = from list in accessoryModel.Accessories
                        select new AccessoryJson(list);

            this.Accessories = query.ToList();

        }
        #endregion      
    }

    public partial class AccessoryJson : BaseJson
    {
        
        public int Id { get; set; }
        public string Number { get; set; }
        public int TypeId { get; set; }
        public string SerialNumber { get; set; }
        public int AccessoryDescriptionId { get; set; }
        public int AccessoryHoldId { get; set; }
        public Nullable<int> OsId { get; set; }
        public Nullable<int> ColorId { get; set; }
        public string AccessoryDescription { get; set; }
        public string AccessoryType { get; set; }
        public string Hold_Email { get; set; }
        public DateTime Hold_Date { get; set; }
        public bool Hold_IsBusy { get; set; }
        public string Hold_Team_Name { get; set; }
        public string Color { get; set; }
        public string OSs { get; set; }

        public AccessoryJson(Accessory accessory)
        {
            this.Id = accessory.Id;
            this.Number = accessory.Number;
            this.TypeId = accessory.TypeId;
            this.SerialNumber = accessory.SerialNumber;
            this.AccessoryDescriptionId = accessory.AccessoryDescriptionId;
            this.AccessoryHoldId = accessory.AccessoryHoldId;
            this.OsId = accessory.OsId;
            this.ColorId = accessory.ColorId;
            this.AccessoryDescription = accessory.AccessoryDescription.Description;
            this.AccessoryType = accessory.AccessoryType.Name;
            this.Hold_Email = accessory.Hold.Email;
            this.Hold_Date = accessory.Hold.HoldDate;
            this.Hold_IsBusy = accessory.Hold.IsBusy;
            this.Hold_Team_Name = accessory.Hold.Team.Name;
            this.Color = accessory.Color != null ? accessory.Color.Name : "";
            this.OSs = accessory.OSs != null ? accessory.OSs.Name : "";
            this.emailEmployee = accessory.Hold.Email;
            this.employee = employee;
        }
    }
}