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
    public class TownModel : IModel
    {

        [Dependency]
        public TownRepo TownRepo { get; set; }

        public void Init()
        {
        }

        public IEnumerable<Town> GetAll()
        {
            return this.TownRepo.GetAll();
        }

       
        

    }
}



