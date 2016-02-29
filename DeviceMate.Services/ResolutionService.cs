using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Services
{
    public class ResolutionService : IResolutionService
    {
         #region Fields
        private readonly ResolutionRepo _resolutionRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public ResolutionService()
        {
            _context = new DeviceContext();
            _resolutionRepo = new ResolutionRepo(_context);
        }
        #endregion

        #region IResolutionHeightService methods

        public bool Add(ResolutionProxy resolutionProxy)
        {
            if (resolutionProxy.Height == null || resolutionProxy.Height.Id == 0 ||
                resolutionProxy.Width == null || resolutionProxy.Width.Id == 0
                )
            {
                throw new ArgumentException("Screen resolution must have width and height dimentions.", "resolutionProxy");
            }

            Resolution resolution = new Resolution()
            {
                ResolutionHeightId = resolutionProxy.Height.Id,
                ResolutionWidthId = resolutionProxy.Width.Id
            };

            _resolutionRepo.Create(resolution);
            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Screen resolution ID cannot be 0.", "id");
            }

            Resolution resolution = _resolutionRepo.Get(r => r.Id == id)
                                                            .FirstOrDefault();

            if (resolution == null)
            {
                throw new InvalidOperationException(string.Format("Screen resolution with ID {0} does not exists.", id));
            }

            if (resolution.Devices != null)
            {
                foreach (Device device in resolution.Devices)
                {
                    device.ResolutionId = null;
                }
            }

            _resolutionRepo.DeleteWithoutSave(resolution);
            _resolutionRepo.SaveChanges();
            return true;
        }

        public void RemoveFromDevicesWithoutSave(IEnumerable<Resolution> resolutions)
        {
            if (resolutions != null)
            {
                foreach (Resolution resolution in resolutions)
                {
                    if (resolution.Devices != null && resolution.Devices.Any())
                    {
                        foreach (Device device in resolution.Devices)
                        {
                            device.ResolutionId = null;
                        }
                    }
                }
            }
        }

        public int GetIdByWidthAndHeight(int widthId, int heightId)
        {
            Resolution resolution = _resolutionRepo.GetNoTracking(r => r.ResolutionHeightId == heightId &&
                                                                        r.ResolutionWidthId == widthId)
                                                    .FirstOrDefault();

            if (resolution == null)
            {
                Resolution newResolution = new Resolution()
                {
                    ResolutionHeightId = heightId,
                    ResolutionWidthId = widthId
                };

                _resolutionRepo.Add(newResolution);
                _resolutionRepo.SaveChanges();

                resolution = _resolutionRepo.GetNoTracking(r => r.ResolutionHeightId == heightId &&
                                                                r.ResolutionWidthId == widthId)
                                            .FirstOrDefault();
            }

            return resolution.Id;
        }
        #endregion
    }
}
