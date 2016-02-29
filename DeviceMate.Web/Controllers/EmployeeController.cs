using DeviceMate.Objects.EmployeesInformation;
using DeviceMate.Web.Controllers.Abstract;
using DeviceMate.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DeviceMate.Web.Controllers
{
    public class EmployeeController : BaseController
    {
        [HttpPost]
        [OutputCache(Duration = 900, VaryByParam = "*")]
        public async Task<JsonResult> Index(List<string> emails)
        {
            try
            {
                EmployeesInfoExtractor extractor = new EmployeesInfoExtractor(emails);
                Dictionary<string, Employee> holders = await extractor.Extract(false);

                return Json(new 
                { 
                    IsSuccess = true,
                    Result = holders 
                });
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = exception.Message
                });
            }
        }

        [HttpGet]
        public ActionResult Location(string email)
        {
            var model = new EmployeeLocationModel(email);
            InitUserModel(model);

            if (!model.Validate()) return View(model);
            if (!model.FindLocation(Server.MapPath(ConfigurationManager.AppSettings["MapsDirectoryPath"]))) return View(model);

            return View(model);
        }
    }
}