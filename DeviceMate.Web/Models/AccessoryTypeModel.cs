using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceMate.Web.Models
{
    public class AccessoryTypeModel : IModel
    {
        #region Properties

        [Dependency]
        public AccessoryTypeRepo AccessoryTypeRepo { get; set; }

        public void Init()
        {
        }

        #endregion

        #region Methods

        public IEnumerable<AccessoryType> GetAll()
        {
            return this.AccessoryTypeRepo.GetAll();
        }

        public void AddAccessoryType(AccessoryTypeProxy accessoryTypeProxy)
        {
            if (this.AccessoryTypeRepo.Context.AccessoryTypes.Any(a => a.Name == accessoryTypeProxy.Name))
            {
                throw new ArgumentException("There is an accessory type with the same name.");
            }

            AccessoryType accessoryType = new AccessoryType
            {
                Name = accessoryTypeProxy.Name
            };

            this.AccessoryTypeRepo.Create(accessoryType);
        }

        public void DeleteAccessoryType(int id)
        {
            AccessoryType type = this.AccessoryTypeRepo.Context.AccessoryTypes.FirstOrDefault(t => t.Id == id);
            if (type != null)
            {
                bool isAnyAccessoryInUse = this.AccessoryTypeRepo.Context.Accessories.Any(a => a.TypeId == type.Id);
                if (!isAnyAccessoryInUse)
                {
                    this.AccessoryTypeRepo.Delete(type);
                }
                else
                {
                    throw new ArgumentException("The accessory type cannot be deleted bcause it is in use.");
                }
            }
            else
            {
                throw new ArgumentNullException("The accessory type has not been found.");
            }
        }

        #endregion
    }
}