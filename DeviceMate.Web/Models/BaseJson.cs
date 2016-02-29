using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System.Configuration;
using DeviceMate.Objects.EmployeesInformation;



namespace DeviceMate.Web.Models
{
    public class BaseJson
    {
        public Employee employee { get; set; }
        public string emailEmployee { get; set; }
    }

}