using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Web.Models
{
    public class ManufacturerModel : IModel
    {
        [Dependency]
        public ManufacturerRepo ManufacturerRepo { get; set; }

        public void Init()
        {
        }

        public IEnumerable<Manufacturer> GetManufacturer(int id)
        {
            return this.ManufacturerRepo.GetByOsId(id);
        }

        public bool SaveManufacturer(ManufacturerProxy model)
        {
            bool result = false;
            Manufacturer newModel = new Manufacturer()
            {
                Name = model.Name,
                OsId = model.OsId
            };

            IEnumerable<Manufacturer> lst = GetManufacturer(model.OsId);
            if (lst.Where(itm => itm.Name == model.Name).FirstOrDefault() != null)
            {
                return false;
            }

            int recordsSaved = this.ManufacturerRepo.Create(newModel);
            if (recordsSaved == 1)
            {
                result = true;
            }

            return result;
        }

        public bool DeleteManufacturer(int id)
        {
            bool result = false;

            IQueryable<Model> inUse = this.ManufacturerRepo.Context.Models.Where(m => m.ManufacturerId == id);
            if (inUse.ToList().Count == 0)
            {
                Manufacturer manufacturer = this.ManufacturerRepo.Context.Manufacturers.Where(m => m.Id == id).FirstOrDefault();
                int count = this.ManufacturerRepo.Delete(manufacturer);
                if (count > 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}