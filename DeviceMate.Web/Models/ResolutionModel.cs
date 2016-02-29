using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Web.Models
{
    public class ResolutionModel : IModel
    {
        [Dependency]
        public ResolutionWidthRepo ResolutionWidthRepo { get; set; }
        [Dependency]
        public ResolutionHeightRepo ResolutionHeightRepo { get; set; }

        public void Init()
        {
        }

        public IEnumerable<ResolutionWidthOption> GetAllWidths()
        {
            return this.ResolutionWidthRepo.GetAll();
        }

        public IEnumerable<ResolutionHeightOption> GetAllHeights()
        {
            return this.ResolutionHeightRepo.GetAll();
        }

        public bool SaveWidth(WidthProxy w)
        {
            var widths = this.ResolutionWidthRepo.GetAll();
            if (widths.FirstOrDefault(x => x.Width == w.Width) != null)
            {
                return false;
            }

            var result = false;
            var newWidth = new ResolutionWidthOption()
            {
                Width = w.Width
            };

            int recordsSaved = this.ResolutionWidthRepo.Create(newWidth);
            if (recordsSaved == 1)
            {
                result = true;
            }
            return result;
        }

        public bool SaveHeight(HeightProxy h)
        {
            var heights = this.ResolutionHeightRepo.GetAll();
            if (heights.FirstOrDefault(x => x.Height == h.Height) != null)
            {
                return false;
            }

            var result = false;
            var newHeight = new ResolutionHeightOption()
            {
                Height = h.Height
            };

            int recordsSaved = this.ResolutionHeightRepo.Create(newHeight);
            if (recordsSaved == 1)
            {
                result = true;
            }
            return result;
        }

        public bool DeleteResolutionWidth(int id)
        {
            bool result = false;
            IQueryable<Device> inUse = this.ResolutionWidthRepo.Context.Devices.Where(device => device.Resolution.ResolutionWidthId == id);

            if (inUse.ToList().Count == 0)
            {
                int count = this.ResolutionWidthRepo.Delete(id);
                if (count > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public bool DeleteResolutionHeight(int id)
        {
            bool result = false;
            IQueryable<Device> inUse = this.ResolutionHeightRepo.Context.Devices.Where(device => device.Resolution.ResolutionHeightId == id);

            if (inUse.ToList().Count == 0)
            {
                int count = this.ResolutionHeightRepo.Delete(id);
                if (count > 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}