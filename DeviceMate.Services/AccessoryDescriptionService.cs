using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class AccessoryDescriptionService : IAccessoryDescriptionService
    {
        #region Fields
        private readonly AccessoryDescriptionRepo _accessoryDescriptionRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public AccessoryDescriptionService()
        {
            _context = new DeviceContext();
            _accessoryDescriptionRepo = new AccessoryDescriptionRepo(_context);
        }
        #endregion

        #region IAccessoryDescriptionService methods
        public bool Add(AccessoryDescriptionProxy accessoryDescriptionProxy)
        {
            if (string.IsNullOrWhiteSpace(accessoryDescriptionProxy.Name))
            {
                throw new ArgumentException("Accessory description name cannot be empty.", "accessoryTypeProxy.Name");
            }

            AccessoryDescription accessoryDescription = new AccessoryDescription()
            {
                Description = accessoryDescriptionProxy.Name.Trim()
            };

            _accessoryDescriptionRepo.Create(accessoryDescription);
            return true;
        }

        public bool Edit(AccessoryDescriptionProxy accessoryDescriptionProxy)
        {
            if (accessoryDescriptionProxy.Id == 0)
            {
                throw new ArgumentException("Accessory description ID cannot be 0.", "accessoryTypeProxy.Id");
            }

            if (string.IsNullOrWhiteSpace(accessoryDescriptionProxy.Name))
            {
                throw new ArgumentException("Accessory description name cannot be empty.", "accessoryTypeProxy.Name");
            }

            AccessoryDescription accessoryDescription = _accessoryDescriptionRepo.Get(ad => ad.Id == accessoryDescriptionProxy.Id).FirstOrDefault();

            if (accessoryDescription == null)
            {
                throw new InvalidOperationException(string.Format("Accessory description with ID {0} does not exists.", accessoryDescriptionProxy.Id));
            }

            accessoryDescription.Description = accessoryDescriptionProxy.Name.Trim();
            _accessoryDescriptionRepo.Update(accessoryDescription);
            _accessoryDescriptionRepo.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Accessory description ID cannot be 0.", "id");
            }

            AccessoryDescription accessoryDescription = _accessoryDescriptionRepo.Get(ad => ad.Id == id)
                                                            .Include("Accessories")
                                                            .FirstOrDefault();

            if (accessoryDescription == null)
            {
                throw new InvalidOperationException(string.Format("Accessory description with ID {0} does not exists.", id));
            }

            if ((accessoryDescription.Accessories != null && accessoryDescription.Accessories.Any()))
            {
                throw new InvalidOperationException(string.Format("Accessory description with ID {0} cannot be deleted because it has related properties.", id));
            }

            _accessoryDescriptionRepo.Delete(accessoryDescription);
            return true;
        }
        #endregion
    }
}
