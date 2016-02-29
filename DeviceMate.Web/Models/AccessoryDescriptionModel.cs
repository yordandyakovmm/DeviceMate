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
    public class AccessoryDescriptionModel : IModel
    {
        #region Properties

        [Dependency]
        public AccessoryDescriptionRepo AccessoryDescriptionRepo { get; set; }

        public void Init()
        {
        }

        #endregion

        #region Methods

        public IEnumerable<AccessoryDescription> GetAll()
        {
            return this.AccessoryDescriptionRepo.GetAll();
        }

        public void AddAccessoryDescription(AccessoryDescriptionProxy accessoryDescriptionProxy)
        {
            if (this.AccessoryDescriptionRepo.Context.AccessoryDescriptions.Any(a => a.Description == accessoryDescriptionProxy.Description))
            {
                throw new ArgumentException("There is an accessory description with the same name.");
            }

            AccessoryDescription accessoryDescription = new AccessoryDescription
            {
                Description = accessoryDescriptionProxy.Description
            };

            this.AccessoryDescriptionRepo.Create(accessoryDescription);
        }

        public void DeleteAccessoryDescription(int id)
        {
            AccessoryDescription accessoryDescription = this.AccessoryDescriptionRepo.Context.AccessoryDescriptions.FirstOrDefault(t => t.Id == id);
            if (accessoryDescription != null)
            {
                bool isAnyAccessoryInUse = this.AccessoryDescriptionRepo.Context.Accessories.Any(a => a.AccessoryDescriptionId == accessoryDescription.Id);
                if (!isAnyAccessoryInUse)
                {
                    this.AccessoryDescriptionRepo.Delete(accessoryDescription);
                }
                else
                {
                    throw new ArgumentException("The accessory descrption cannot be deleted because it is in use.");
                }
            }
            else
            {
                throw new ArgumentNullException("The accessory description has not been found.");
            }
        }

        #endregion
    }
}