using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class ManufacturerService : IManufacturerService
    {
        #region Fields
        private readonly ManufacturerRepo _manufacturerRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public ManufacturerService()
        {
            _context = new DeviceContext();
            _manufacturerRepo = new ManufacturerRepo(_context);
        }
        #endregion

        #region IColorService methods

        public bool Add(ManufacturerProxy manufacturerProxy)
        {
            if (string.IsNullOrWhiteSpace(manufacturerProxy.Name))
            {
                throw new ArgumentException("Manufacturer name cannot be empty.", "manufacturerProxy.Name");
            }

            Manufacturer manufacturer = new Manufacturer()
            {
                Name = manufacturerProxy.Name.Trim(),
                OsId = manufacturerProxy.OsId
            };

            _manufacturerRepo.Create(manufacturer);
            return true;
        }

        public bool Edit(ManufacturerProxy manufacturerProxy)
        {
            if (manufacturerProxy.Id == 0)
            {
                throw new ArgumentException("Manufacturer ID cannot be 0.", "manufacturerProxy.Id");
            }

            if (string.IsNullOrWhiteSpace(manufacturerProxy.Name))
            {
                throw new ArgumentException("Manufacturer name cannot be empty.", "platform.Name");
            }

            Manufacturer manufacturer = _manufacturerRepo.Get(m => m.Id == manufacturerProxy.Id).FirstOrDefault();

            if (manufacturer == null)
            {
                throw new InvalidOperationException(string.Format("Manufacturer with ID {0} does not exists.", manufacturerProxy.Id));
            }

            manufacturer.Name = manufacturerProxy.Name.Trim();
            manufacturer.OsId = manufacturerProxy.OsId;

            _manufacturerRepo.Update(manufacturer);
            _manufacturerRepo.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Manufacturer ID cannot be 0.", "id");
            }

            Manufacturer manufacturer = _manufacturerRepo.Get(m => m.Id == id)
                                                        .Include("Models")
                                                        .FirstOrDefault();

            if (manufacturer == null)
            {
                throw new InvalidOperationException(string.Format("Manufacturer with ID {0} does not exists.", id));
            }

            if (manufacturer.Models != null && manufacturer.Models.Any())
            {
                throw new InvalidOperationException(string.Format("Manufacturer with ID {0} cannot be deleted because it has related properties.", id));
            }

            _manufacturerRepo.Delete(manufacturer);
            return true;
        }

        #endregion
    }
}
