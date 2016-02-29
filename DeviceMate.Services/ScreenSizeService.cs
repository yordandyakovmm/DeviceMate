using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class ScreenSizeService : IScreenSizeService
    {
        #region Fields
        private readonly ScreenSizeRepo _screenSizeRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public ScreenSizeService()
        {
            _context = new DeviceContext();
            _screenSizeRepo = new ScreenSizeRepo(_context);
        }
        #endregion

        #region IScreenSizeService methods

        public bool Add(ScreenSizeProxy screenSizeProxy)
        {
            if (screenSizeProxy.Size == 0)
            {
                throw new ArgumentException("Screen size cannot be 0.", "screenSizeProxy.Size");
            }

            ScreenSize screenSize = new ScreenSize()
            {
                Size = screenSizeProxy.Size
            };

            _screenSizeRepo.Create(screenSize);
            return true;
        }

        public bool Edit(ScreenSizeProxy screenSizeProxy)
        {
            if (screenSizeProxy.Id == 0)
            {
                throw new ArgumentException("Screen size ID cannot be 0.", "screenSizeProxy.Id");
            }

            if (screenSizeProxy.Size == 0)
            {
                throw new ArgumentException("Screen size cannot be 0.", "screenSizeProxy.Size");
            }

            ScreenSize screenSize = _screenSizeRepo.Get(sz => sz.Id == screenSizeProxy.Id).FirstOrDefault();

            if (screenSize == null)
            {
                throw new InvalidOperationException(string.Format("Screen size with ID {0} does not exists.", screenSizeProxy.Id));
            }

            screenSize.Size = screenSizeProxy.Size;
            _screenSizeRepo.Update(screenSize);
            _screenSizeRepo.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Screen size ID cannot be 0.", "id");
            }

            ScreenSize screenSize = _screenSizeRepo.Get(sz => sz.Id == id)
                                    .Include("Devices")
                                    .FirstOrDefault();

            if (screenSize == null)
            {
                throw new InvalidOperationException(string.Format("Screen size with ID {0} does not exists.", id));
            }

            foreach (Device device in screenSize.Devices)
            {
                device.ScreenSizeId = null;
            }

            _screenSizeRepo.DeleteWithoutSave(screenSize);
            _screenSizeRepo.SaveChanges();
            return true;
        }

        #endregion
    }
}
