using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Web.Models
{
    public class ColorModel : IModel
    {
        [Dependency]
        public ColorRepo ColorRepo { get; set; }

        public void Init()
        {
        }
        
        public IEnumerable<Color> GetAll()
        {
            return this.ColorRepo.GetAll();
        }

        public bool SaveColor(ColorProxy color)
        {
            var colors = this.ColorRepo.GetAll();
            if (colors.FirstOrDefault(itm => itm.Name.Trim().ToLower() == color.Name.Trim().ToLower()) != null)
            {
                return false;
            }

            bool result = false;
            Color newColor = new Color()
            {
                Name = color.Name,
            };

            int recordsSaved = this.ColorRepo.Create(newColor);
            if (recordsSaved == 1)
            {
                result = true;
            }
            return result;
        }

        public bool DeleteColor(int id)
        {
            bool result = false;
            IQueryable<Device> inUse = this.ColorRepo.Context.Devices.Where(device => device.ColorId == id);
            
            if (inUse.ToList().Count== 0)
            {
                Color color = this.ColorRepo.Context.Colors.Where(c => c.Id == id).FirstOrDefault();
                int count = this.ColorRepo.Delete(color);
                if (count > 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}