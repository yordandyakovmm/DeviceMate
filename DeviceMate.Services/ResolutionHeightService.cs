using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class ResolutionHeightService : IResolutionHeightService
    {
        #region Fields
        private readonly IResolutionService _resolutionService;
        private readonly ResolutionHeightRepo _resolutionHeightRepo;
        private readonly ResolutionRepo _resolutionRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public ResolutionHeightService(IResolutionService resolutionService)
        {
            _context = new DeviceContext();
            _resolutionHeightRepo = new ResolutionHeightRepo(_context);
            _resolutionRepo = new ResolutionRepo(_context);
            _resolutionService = resolutionService;
        }
        #endregion

        #region IResolutionHeightService methods

        public bool Add(ResolutionDimention heightDimention)
        {
            if (heightDimention.Dimention == 0)
            {
                throw new ArgumentException("Screen height cannot be 0.", "heightDimention.Dimention");
            }

            ResolutionHeightOption height = new ResolutionHeightOption()
            {
                Height = heightDimention.Dimention
            };

            _resolutionHeightRepo.Create(height);
            return true;
        }

        public bool Edit(ResolutionDimention heightDimention)
        {
            if (heightDimention.Id == 0)
            {
                throw new ArgumentException("Screen height ID cannot be 0.", "heightDimention.Id");
            }

            if (heightDimention.Dimention == 0)
            {
                throw new ArgumentException("Screen height cannot be 0.", "heightDimention.Dimention");
            }

            ResolutionHeightOption height = _resolutionHeightRepo.Get(h => h.Id == heightDimention.Id).FirstOrDefault();

            if (height == null)
            {
                throw new InvalidOperationException(string.Format("Screen height with ID {0} does not exists.", heightDimention.Id));
            }

            height.Height = heightDimention.Dimention;
            _resolutionHeightRepo.Update(height);
            _resolutionHeightRepo.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Screen height ID cannot be 0.", "id");
            }

            ResolutionHeightOption height = _resolutionHeightRepo.Get(h => h.Id == id)
                                                            .Include("Resolutions.Devices")
                                                            .FirstOrDefault();

            if (height == null)
            {
                throw new InvalidOperationException(string.Format("Screen height with ID {0} does not exists.", id));
            }

            if (height.Resolutions != null && height.Resolutions.Any())
            {
                _resolutionService.RemoveFromDevicesWithoutSave(height.Resolutions);
                _resolutionRepo.DeleteWithoutSave(height.Resolutions);
            }
            
            _resolutionHeightRepo.DeleteWithoutSave(height);
            _resolutionHeightRepo.SaveChanges();


            return true;
        }

        #endregion
    }
}
