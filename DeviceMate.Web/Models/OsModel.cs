using DeviceMate.Objects.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DeviceMate.Objects;
using DeviceMate.Objects.Proxies;
using DeviceMate.Models.Entities;
using Microsoft.Practices.Unity;

namespace DeviceMate.Web.Models
{
    public class OsModel : IModel
    {
        [Dependency]
        public OSRepo OsRepo { get; set; }

        public void Init()
        {
        }

        public IEnumerable<OSs> GetAll()
        {
            return this.OsRepo.GetAll();
        }

        public bool SaveOs(string name)
        {
            var oss = this.OsRepo.GetAll();
            if (oss.FirstOrDefault(itm => itm.Name.Trim().ToLower() == name.Trim().ToLower()) != null)
            {
                return false;
            }

            bool result = false;
            OSs newOs = new OSs()
            {
                Name = name,
            };

            int recordsSaved = this.OsRepo.Create(newOs);
            if (recordsSaved == 1)
            {
                result = true;
            }
            return result;
        }

        public bool DeleteOs(int id)
        {
            bool result = false;
            
            IQueryable<Manufacturer> inUse = this.OsRepo.Context.Manufacturers.Where(m => m.OsId == id);
            if (inUse.ToList().Count== 0)
            {
                OSs os = this.OsRepo.Context.OSses.Where(o => o.Id == id).FirstOrDefault();
                int count = this.OsRepo.Delete(os);
                if (count > 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}