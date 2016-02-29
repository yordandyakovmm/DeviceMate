using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class ModelService : IModelService
    {
        #region Fields
        private readonly ModelRepo _modelRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public ModelService()
        {
            _context = new DeviceContext();
            _modelRepo = new ModelRepo(_context);
        }
        #endregion

        #region IModelService methods

        public bool Add(ModelProxy modelProxy)
        {
            if (string.IsNullOrWhiteSpace(modelProxy.Name))
            {
                throw new ArgumentException("Model name cannot be empty.", "modelProxy.Name");
            }

            Model model = new Model()
            {
                Name = modelProxy.Name.Trim(),
                ManufacturerId = modelProxy.ManufacturerId
            };

            _modelRepo.Create(model);
            return true;
        }

        public bool Edit(ModelProxy modelProxy)
        {
            if (modelProxy.Id == 0)
            {
                throw new ArgumentException("Model ID cannot be 0.", "colorProxy.Id");
            }

            if (string.IsNullOrWhiteSpace(modelProxy.Name))
            {
                throw new ArgumentException("Model name cannot be empty.", "platform.Name");
            }

            Model model = _modelRepo.Get(m => m.Id == modelProxy.Id).FirstOrDefault();

            if (model == null)
            {
                throw new InvalidOperationException(string.Format("Model with ID {0} does not exists.", modelProxy.Id));
            }

            model.Name = modelProxy.Name.Trim();
            model.ManufacturerId = modelProxy.ManufacturerId;
            _modelRepo.Update(model);
            _modelRepo.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Model ID cannot be 0.", "id");
            }

            Model model = _modelRepo.Get(m => m.Id == id)
                                    .Include("Devices")
                                    .FirstOrDefault();

            if (model == null)
            {
                throw new InvalidOperationException(string.Format("Model with ID {0} does not exists.", id));
            }

            if (model.Devices != null && model.Devices.Any())
            {
                throw new InvalidOperationException(string.Format("Model with ID {0} cannot be deleted because it has related properties.", id));
            }

            _modelRepo.Delete(model);
            return true;
        }

        #endregion
    }
}
