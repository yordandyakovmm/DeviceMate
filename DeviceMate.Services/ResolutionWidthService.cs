using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class ResolutionWidthService : IResolutionWidthService
    {
        #region Fields
        private readonly IResolutionService _resolutionService;
        private readonly ResolutionWidthRepo _resolutionWidthRepo;
        private readonly ResolutionRepo _resolutionRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public ResolutionWidthService(IResolutionService resolutionService)
        {
            _context = new DeviceContext();
            _resolutionWidthRepo = new ResolutionWidthRepo(_context);
            _resolutionRepo = new ResolutionRepo(_context);
            _resolutionService = resolutionService;
        }
        #endregion

        #region IResolutionWidthService methods

        public bool Add(ResolutionDimention widthDimention)
        {
            if (widthDimention.Dimention == 0)
            {
                throw new ArgumentException("Screen width cannot be 0.", "widthDimention.Dimention");
            }

            ResolutionWidthOption width = new ResolutionWidthOption()
            {
                Width = widthDimention.Dimention
            };

            _resolutionWidthRepo.Create(width);
            return true;
        }

        public bool Edit(ResolutionDimention widthDimention)
        {
            if (widthDimention.Id == 0)
            {
                throw new ArgumentException("Screen width ID cannot be 0.", "widthDimention.Id");
            }

            if (widthDimention.Dimention == 0)
            {
                throw new ArgumentException("Screen width cannot be 0.", "widthDimention.Dimention");
            }

            ResolutionWidthOption width = _resolutionWidthRepo.Get(w => w.Id == widthDimention.Id).FirstOrDefault();

            if (width == null)
            {
                throw new InvalidOperationException(string.Format("Screen width with ID {0} does not exists.", widthDimention.Id));
            }

            width.Width = widthDimention.Dimention;
            _resolutionWidthRepo.Update(width);
            _resolutionWidthRepo.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Screen width ID cannot be 0.", "id");
            }

            ResolutionWidthOption width = _resolutionWidthRepo.Get(w => w.Id == id)
                                                            .Include("Resolutions.Devices")
                                                            .FirstOrDefault();

            if (width == null)
            {
                throw new InvalidOperationException(string.Format("Screen width with ID {0} does not exists.", id));
            }

            if (width.Resolutions != null && width.Resolutions.Any())
            {
                _resolutionService.RemoveFromDevicesWithoutSave(width.Resolutions);
                _resolutionRepo.DeleteWithoutSave(width.Resolutions);
            }
            
            _resolutionWidthRepo.DeleteWithoutSave(width);
            _resolutionWidthRepo.SaveChanges();

            return true;
        }

        #endregion
    }
}
