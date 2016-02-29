using AutoMapper;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeviceMate.Web.Models
{
    public class AccessoryModel : BaseModelWithSortingAndPaginationAndColumnSelection<AccessoryRepo, Accessory, int?>, IUserModel
    {
        #region Properties

        [Dependency]
        public AccessoryTypeRepo AccessoryTypeRepo { get; set; }
        [Dependency]
        public AccessoryDescriptionRepo AccessoryDescriptionRepo { get; set; }
        [Dependency]
        public TeamRepo TeamRepo { get; set; }
        [Dependency]
        public HoldRepo HoldRepo { get; set; }
        [Dependency]
        public TownRepo TownRepo { get; set; }
        [Dependency]
        public OSRepo OsRepo { get; set; }
        [Dependency]
        public ColorRepo ColorRepo { get; set; }

        public AccessoryProxy Accessory { get; set; }

        public AccessorySearchModel SearchModel { get; set; }

        public IEnumerable<SelectListItem> AccessoryTypes { get; set; }
        public IEnumerable<SelectListItem> AccessoryDescriptions { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }
        public IEnumerable<SelectListItem> Towns { get; set; }
        public IEnumerable<SelectListItem> OSs { get; set; }
        public IEnumerable<SelectListItem> Colors { get; set; }

        public IEnumerable<Accessory> Accessories { get; set; }

        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public int TownId { get; set; }


        #endregion

        #region Methods

        public override void Init(int? accessoryId)
        {
            this.PopulateData();

            if (accessoryId.HasValue)
            {
                Accessory accessory = Repo.GetById(accessoryId.Value);
                this.Accessory = Mapper.Map<AccessoryProxy>(accessory);

                this.AccessoryTypes.First(type => type.Value == accessory.TypeId.ToString()).Selected = true; ;
                this.AccessoryDescriptions.First(type => type.Value == accessory.AccessoryDescriptionId.ToString()).Selected = true;

                if (accessory.Hold != null && accessory.Hold.TownID > 0)
                {
                    this.TownId = accessory.Hold.TownID;
                }
                if (accessory.OsId.HasValue)
                {
                    this.OSs.First(os => os.Value == accessory.OsId.ToString()).Selected = true;
                }
                if (accessory.ColorId.HasValue)
                {
                    this.Colors.First(color => color.Value == accessory.ColorId.ToString()).Selected = true;
                }
                
            }
        }



        public void PopulateData()
        {
            this.Accessory = new AccessoryProxy();
            this.AccessoryTypes = AccessoryTypeRepo.GetAll()
                .Select(type => new SelectListItem { Text = type.Name, Value = type.Id.ToString() });
            this.AccessoryDescriptions = AccessoryDescriptionRepo.GetAll()
                .Select(desc => new SelectListItem { Text = desc.Description, Value = desc.Id.ToString() });
            this.Teams = this.TeamRepo.GetAll()
                .Select(team => new SelectListItem { Text = team.Name, Value = team.Id.ToString() });
            this.Towns = this.TownRepo.GetAll()
                .Select(town => new SelectListItem { Text = town.Name, Value = town.TownId.ToString() });
            this.OSs = this.OsRepo.GetAll()
                .Select(os => new SelectListItem { Text = os.Name, Value = os.Id.ToString() });
            this.Colors = this.ColorRepo.GetAll()
                .Select(color => new SelectListItem { Text = color.Name, Value = color.Id.ToString() });

            TotalNumberOfDevices = Repo.GetAllCount();
        }

        public void AddAccessory()
        {
            Accessory accessory = new Accessory
            {
                Number = this.Accessory.Number.Trim(),
                TypeId = this.Accessory.AccessoryTypeId,
                SerialNumber = this.Accessory.SerialNumber,
                AccessoryDescriptionId = this.Accessory.AccessoryDescriptionId,
                OsId = this.Accessory.OsId,
                ColorId = this.Accessory.ColorId
            };

            Hold hold = new Hold
            {
                HoldDate = DateTime.Now,
                Email = UserName,
                TeamId = this.UserRepo.Get(u => u.Email == this.UserName).Select(u => u.TeamId.Value).FirstOrDefault(),
                TownID = TownId
            };

            accessory.Hold = hold;
            Repo.Create(accessory);
        }

        public void EditAccessory()
        {
            this.Accessory.Number = this.Accessory.Number.Trim();

            Accessory accessory = Mapper.Map<Accessory>(Accessory);
            Repo.Edit(accessory);
        }

        public void Delete(int id)
        {
            int holdId = this.Repo.GetHoldIdById(id);
            this.Repo.Delete(id);
            this.HoldRepo.Delete(holdId);
        }

        public bool IsAccessoryNameTaken()
        {
            HashSet<string> accessoryNumbers = this.Repo.GetNumbers();
            if (this.Accessory.Id.HasValue)
            {
                string oldNumer = this.Repo.GetNumberById(this.Accessory.Id.Value);
                accessoryNumbers.Remove(oldNumer.ToLower());
            }

            bool isTaken = accessoryNumbers.Contains(this.Accessory.Number.ToLower().Trim());
            return isTaken;
        }

        public void GetAllAccessories()
        {
            this.Accessories = Repo.GetAll(Pager.Page, Pager.PageSize);
        }

        public void GetAllAccessoriesJson()
        {
            this.Accessories = Repo.GetAllJson();
        }

        public int GetSearchAccessoriesCount()
        {
            return SearchModel == null
                ? Repo.GetAllCount()
                : Repo.SearchCount(this.SearchModel.Number, this.SearchModel.SerialNumber,
                    this.SearchModel.TypeId, this.SearchModel.DescriptionId, this.SearchModel.Email,
                    this.SearchModel.TeamId, this.SearchModel.TownId, this.SearchModel.OsId, this.SearchModel.ColorId);
        }

        public void GetSearchAccessories()
        {
            if (SearchModel == null)
            {
                GetAllAccessories();
            }
            else
            {
                this.Accessories = Repo.Search(this.SearchModel.Number, this.SearchModel.SerialNumber,
                    this.SearchModel.TypeId, this.SearchModel.DescriptionId, this.SearchModel.Email, this.SearchModel.TeamId,
                    this.SearchModel.TownId, this.SearchModel.OsId, this.SearchModel.ColorId,
                    Sorter.Expression, Sorter.Column, (int)Sorter.Direction, Pager.Page, Pager.PageSize);

            }
        }

        public void GetSearchAccessoriesJson()
        {
            if (SearchModel == null)
            {
                GetAllAccessoriesJson();
            }
            else
            {
                this.Accessories = Repo.Search(this.SearchModel.Number, this.SearchModel.SerialNumber,
                    this.SearchModel.TypeId, this.SearchModel.DescriptionId, this.SearchModel.Email, this.SearchModel.TeamId,
                    this.SearchModel.TownId, this.SearchModel.OsId, this.SearchModel.ColorId,
                    Sorter.Expression, Sorter.Column, (int)Sorter.Direction, 0, 0);

            }
        }

        #endregion
    }
}