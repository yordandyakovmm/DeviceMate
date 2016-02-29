using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class OsService : IOsService
    {
        #region Fields
        private readonly OSRepo _osRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public OsService()
        {
            _context = new DeviceContext();
            _osRepo = new OSRepo(_context);
        }
        #endregion

        #region IOsService methods
        public int GetIdByName(string name)
        {
            OSs os = _osRepo.GetNoTracking(o => o.Name.ToLower() == name).FirstOrDefault();

            if (os != null)
            {
                return os.Id;
            }
            else
            {
                return 0;
            }
        }

        public bool Add(Platform platform)
        {
            if (string.IsNullOrWhiteSpace(platform.Name))
            {
                throw new ArgumentException("OS name cannot be empty.", "platform.Name");
            }

            OSs os = new OSs()
            {
                Name = platform.Name.Trim()
            };

            _osRepo.Create(os);
            return true;
        }

        public bool Edit(Platform platform)
        {
            if (platform.Id == 0)
            {
                throw new ArgumentException("OS ID cannot be 0.", "platform.Id");
            }

            if (string.IsNullOrWhiteSpace(platform.Name))
            {
                throw new ArgumentException("OS name cannot be empty.", "platform.Name");
            }

            OSs os = _osRepo.Get(o => o.Id == platform.Id).FirstOrDefault();

            if (os == null)
            {
                throw new InvalidOperationException(string.Format("OS with ID {0} does not exists.", platform.Id));
            }

            os.Name = platform.Name.Trim();
            _osRepo.Update(os);
            _osRepo.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("OS ID cannot be 0.", "id");
            }

            OSs os = _osRepo.Get(o => o.Id == id)
                            .Include("Manufacturers")
                            .Include("Accessories")
                            .FirstOrDefault();

            if (os == null)
            {
                throw new InvalidOperationException(string.Format("OS with ID {0} does not exists.", id));
            }

            if ((os.Manufacturers != null && os.Manufacturers.Any()) ||
                (os.Accessories != null && os.Accessories.Any()))
            {
                throw new InvalidOperationException(string.Format("OS with ID {0} cannot be deleted because it has related properties.", id));
            }

            _osRepo.Delete(os);
            return true;
        }
        #endregion
    }
}
