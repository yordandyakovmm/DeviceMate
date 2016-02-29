using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class ColorService : IColorService
    {
        #region Fields
        private readonly ColorRepo _colorRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public ColorService()
        {
            _context = new DeviceContext();
            _colorRepo = new ColorRepo(_context);
        }
        #endregion

        #region IColorService methods

        public bool Add(ColorProxy colorProxy)
        {
            if (string.IsNullOrWhiteSpace(colorProxy.Name))
            {
                throw new ArgumentException("Color name cannot be empty.", "colorProxy.Name");
            }

            Color color = new Color()
            {
                Name = colorProxy.Name.Trim()
            };

            _colorRepo.Create(color);
            return true;
        }

        public bool Edit(ColorProxy colorProxy)
        {
            if (colorProxy.Id == 0)
            {
                throw new ArgumentException("Color ID cannot be 0.", "colorProxy.Id");
            }

            if (string.IsNullOrWhiteSpace(colorProxy.Name))
            {
                throw new ArgumentException("Color name cannot be empty.", "platform.Name");
            }

            Color color = _colorRepo.Get(c => c.Id == colorProxy.Id).FirstOrDefault();

            if (color == null)
            {
                throw new InvalidOperationException(string.Format("Color with ID {0} does not exists.", colorProxy.Id));
            }

            color.Name = colorProxy.Name.Trim();
            _colorRepo.Update(color);
            _colorRepo.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Color ID cannot be 0.", "id");
            }

            Color color = _colorRepo.Get(c => c.Id == id)
                                    .Include("Accessories")
                                    .Include("Devices")
                                    .FirstOrDefault();

            if (color == null)
            {
                throw new InvalidOperationException(string.Format("Color with ID {0} does not exists.", id));
            }

            foreach (Accessory accessory in color.Accessories)
            {
                accessory.ColorId = null;
            }
            foreach (Device device in color.Devices)
            {
                device.ColorId = null;
            }

            _colorRepo.DeleteWithoutSave(color);
            _colorRepo.SaveChanges();
            return true;
        }

        #endregion
    }
}
