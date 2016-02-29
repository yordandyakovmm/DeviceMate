using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Web.Models
{
    public class ScreenSizeModel : IModel
    {
        [Dependency]
        public ScreenSizeRepo ScreenSizeRepo { get; set; }

        public void Init()
        {
        }

        public IEnumerable<ScreenSize> GetAll()
        {
            return this.ScreenSizeRepo.GetAll();
        }

        public bool SaveScreenSize(ScreenSizeProxy sz)
        {
            var screenSizes = this.ScreenSizeRepo.GetAll();
            if (screenSizes.FirstOrDefault(itm => itm.Size == sz.Size) != null)
            {
                return false;
            }

            var result = false;
            var newScreenSize = new ScreenSize()
            {
                Size = sz.Size,
            };

            int recordsSaved = this.ScreenSizeRepo.Create(newScreenSize);
            if (recordsSaved == 1)
            {
                result = true;
            }
            return result;
        }

        public bool DeleteScreenSize(int id)
        {
            bool result = false;
            IQueryable<Device> inUse = this.ScreenSizeRepo.Context.Devices.Where(device => device.ScreenSizeId == id);

            if (inUse.ToList().Count == 0)
            {
                var sz = this.ScreenSizeRepo.Context.ScreenSizes.FirstOrDefault(c => c.Id == id);
                int count = this.ScreenSizeRepo.Delete(sz);
                if (count > 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}