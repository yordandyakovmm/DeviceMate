using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DeviceMate.Web.Models
{
    public class ModelModel : IModel
    {
        [Dependency]
        public ModelRepo ModelRepo { get; set; }

        public IEnumerable<SelectListItem> Manufacturers { get; set; }

        public void Init()
        {
            this.Manufacturers = new List<SelectListItem>();
        }

        public IEnumerable<Model> GetModels(int id)
        {
            return this.ModelRepo.GetByManufacturerId(id);
        }

        public bool SaveModel(ModelProxy model)
        {
            bool result = false;
            Model newModel = new Model()
            {
                Name = model.Name,
                ManufacturerId = model.ManufacturerId
            };

            IEnumerable<Model> lst = GetModels(model.ManufacturerId);
            if(lst.Where(itm=>itm.Name==model.Name).FirstOrDefault()!=null)
            {
                return false;
            }

            int recordsSaved = this.ModelRepo.Create(newModel);
            if (recordsSaved == 1)
            {
                result = true;
            }

            return result;
        }

        public bool DeleteModel(int id)
        {
            bool result = false;

            IQueryable<Device> inUse = this.ModelRepo.Context.Devices.Where(d => d.ModelId == id);
            if (inUse.ToList().Count == 0)
            {
                Model model = this.ModelRepo.Context.Models.Where(m => m.Id == id).FirstOrDefault();
                int count = this.ModelRepo.Delete(model);
                if (count > 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}