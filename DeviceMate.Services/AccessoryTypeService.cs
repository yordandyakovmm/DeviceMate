using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class AccessoryTypeService : IAccessoryTypeService
    {
        #region Fields
        private readonly AccessoryTypeRepo _accessoryTypeRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public AccessoryTypeService()
        {
            _context = new DeviceContext();
            _accessoryTypeRepo = new AccessoryTypeRepo(_context);
        }
        #endregion

        #region IAccessoryTypeService methods
        public bool Add(AccessoryTypeProxy accessoryTypeProxy)
        {
            if (string.IsNullOrWhiteSpace(accessoryTypeProxy.Name))
            {
                throw new ArgumentException("Accessory type name cannot be empty.", "accessoryTypeProxy.Name");
            }

            AccessoryType accessoryType = new AccessoryType()
            {
                Name = accessoryTypeProxy.Name.Trim()
            };

            _accessoryTypeRepo.Create(accessoryType);
            return true;
        }

        public bool Edit(AccessoryTypeProxy accessoryTypeProxy)
        {
            if (accessoryTypeProxy.Id == 0)
            {
                throw new ArgumentException("Accessory type ID cannot be 0.", "accessoryTypeProxy.Id");
            }

            if (string.IsNullOrWhiteSpace(accessoryTypeProxy.Name))
            {
                throw new ArgumentException("Accessory type name cannot be empty.", "accessoryTypeProxy.Name");
            }

            AccessoryType accessoryType = _accessoryTypeRepo.Get(at => at.Id == accessoryTypeProxy.Id).FirstOrDefault();

            if (accessoryType == null)
            {
                throw new InvalidOperationException(string.Format("Accessory type with ID {0} does not exists.", accessoryTypeProxy.Id));
            }

            accessoryType.Name = accessoryTypeProxy.Name.Trim();
            _accessoryTypeRepo.Update(accessoryType);
            _accessoryTypeRepo.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Accessory type ID cannot be 0.", "id");
            }

            AccessoryType accessoryType = _accessoryTypeRepo.Get(at => at.Id == id)
                                                            .Include("Accessories")
                                                            .FirstOrDefault();

            if (accessoryType == null)
            {
                throw new InvalidOperationException(string.Format("Accessory type with ID {0} does not exists.", id));
            }

            if ((accessoryType.Accessories != null && accessoryType.Accessories.Any()))
            {
                throw new InvalidOperationException(string.Format("Accessory type with ID {0} cannot be deleted because it has related properties.", id));
            }

            _accessoryTypeRepo.Delete(accessoryType);
            return true;
        }
        #endregion
    }
}
