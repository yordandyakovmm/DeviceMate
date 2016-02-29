using DeviceMate.Web.Controllers.Abstract;
using DeviceMate.Web.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Controllers
{
    [Authorize]
    public class AdministratorController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            AdminUserModel adminUserModel = LoadModelUser<AdminUserModel, int?>(null);
            adminUserModel.LoadAllUsers();

            ViewBag.IsAdmin = Roles.IsUserInRole(DMC.Common.AdminUserRole);

            if (TempData["showSuccessNotification"] != null && (bool)TempData["showSuccessNotification"] == true)
            {
                ViewBag.showSuccessNotification = true;
                ViewBag.successNotification = TempData["successNotification"];
            }

            return View(adminUserModel);
        }

        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ActionResult AddOrEdit(int? id)
        {
            AdminUserModel adminUserModel = LoadModelUser<AdminUserModel, int?>(id);

            if (adminUserModel.AdminUser.Email == User.Identity.Name)
            {
                return RedirectToAction("Index", "Administrator");
            }

            return View(adminUserModel);
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public async Task<ActionResult> AddOrEdit(AdminUserModel adminUserModel)
        {
            BuildUp<AdminUserModel>(adminUserModel);

            if (adminUserModel.AdminUser.Email == User.Identity.Name)
            {
                return RedirectToAction("Index", "Administrator");
            }

            if (ModelState.IsValid)
            {
                if (adminUserModel.AdminUser.Id.HasValue)
                {
                    adminUserModel.Edt();
                }
                else
                {
                    adminUserModel.Add();
                }                

                TempData["showSuccessNotification"] = true;
                if (adminUserModel.AdminUser.Id.HasValue)
                {
                    TempData["successNotification"] = string.Format("You have successfully edited {0}.", adminUserModel.AdminUser.IsAdmin ? "an administrator" : "a user");
                }
                else
                {
                    TempData["successNotification"] = string.Format("You have successfully added {0}.", adminUserModel.AdminUser.IsAdmin ? "an administrator" : "a user");
                }
                await updateUserInfoFormGoogleApi(adminUserModel.AdminUser.Email);
                return RedirectToAction("Index");
            }
            else
            {
                adminUserModel.PopulateData();
                return View(adminUserModel);
            }            
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ActionResult Delete(int id)
        {
            try
            {
                AdminUserModel adminUserModel = LoadModelUser<AdminUserModel, int?>(id);

                if (adminUserModel.AdminUser.Email == User.Identity.Name)
                {
                    return RedirectToAction("Index", "Administrator");
                }

                adminUserModel.Delete(id);

                return Json(new { isSuccess = true, Msg = "You have successfully deleted an administrator!" });
            }
            catch (Exception exception)
            {
                return Json(new { isSuccess = false, Msg = exception.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public async Task<string> updateUserInfo()
        {
            return await updateUserInfoFormGoogleApi();
        }

    }
}
