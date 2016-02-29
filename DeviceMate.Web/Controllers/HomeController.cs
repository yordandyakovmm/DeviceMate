using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DeviceMate.Web.Controllers.Abstract;
using DeviceMate.Web.Models;
using System.Collections;

namespace DeviceMate.Web.Controllers
{
    public class HomeController : BaseController
    {
        //[OpenIdAuthorize]
        [Authorize]
        public ActionResult Index()
        {
            var model = new HomeModel();
            InitUserModel<HomeModel>(model);

            List<string> keys = new List<string>(TempData.Keys);
            foreach (string key in keys)
            {
                TempData.Remove(key);
            }

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
