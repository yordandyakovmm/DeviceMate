using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using DeviceMate.Web.Controllers.Abstract;
using DeviceMate.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Controllers
{
    [Authorize]
    public class AccessoryController : BaseController
    {
        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ActionResult AddEdit(int? id)
        {
            if (TempData["showSuccessNotification"] != null && (bool)TempData["showSuccessNotification"] == true)
            {
                ViewBag.showSuccessNotification = true;
                ViewBag.successNotification = TempData["successNotification"];
            }

            AccessoryModel accessoryModel = LoadModelUser<AccessoryModel, int?>(id);
            return View(accessoryModel);
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ActionResult AddEdit(AccessoryModel accessoryModel)
        {
            BuildUp<AccessoryModel>(accessoryModel);
            if (accessoryModel.IsAccessoryNameTaken())
            {
                ModelState.AddModelError("NameUniqueness", "This number has already been taken.");
            }

            if (ModelState.IsValid)
            {
                if (accessoryModel.Accessory.Id.HasValue)
                {
                    accessoryModel.EditAccessory();
                    TempData["successNotification"] = "You have successfully edited an accessory!";
                }
                else
                {
                    accessoryModel.AddAccessory();
                    TempData["successNotification"] = "You have successfully added an accessory!";
                }

                TempData["showSuccessNotification"] = true;
                
                return RedirectToAction("AddEdit", "Accessory");
            }
            else
            {
                accessoryModel.PopulateData();
                return View(accessoryModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult Delete(int id)
        {
            try
            {
                AccessoryModel accessoryModel = LoadModelUser<AccessoryModel, int?>(id);
                accessoryModel.Delete(id);

                return Json(new { isSuccess = true, Msg = "You have successfully deleted an accessory!" });
            }
            catch (Exception exception)
            {
                return Json(new { isSuccess = false, Msg = exception.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddAcccessoryType(AccessoryTypeProxy accessoryTypeProxy)
        {
            if (ModelState.IsValid)
            {
                AccessoryTypeModel model = LoadModel<AccessoryTypeModel>();
                try
                {
                    model.AddAccessoryType(accessoryTypeProxy);
                    return Json(new { isSuccess = true });
                }
                catch(Exception exception)
                {
                    return Json(new { isSuccess = false, errorMsg = exception.Message });
                }
            }
            else
            {
                string[] errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
                string errorsStr = string.Join(" ", errors);
                return Json(new { isSuccess = false, errorMsg = errorsStr });
            }
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteAccessoryType(int id)
        {
            AccessoryTypeModel model = LoadModel<AccessoryTypeModel>();
            try
            {
                model.DeleteAccessoryType(id);
                return Json(new { isSuccess = true });
            }
            catch (Exception exception)
            {
                return Json(new { isSuccess = false, errorMsg = exception.Message });
            }
        }

        [HttpPost]
        public JsonResult GetAccessoryTypes()
        {
            AccessoryTypeModel model = LoadModel<AccessoryTypeModel>();
            IEnumerable<AccessoryType> accessoryTypes = model.GetAll();

            var data = accessoryTypes.Select(type => new
            {
                Value = type.Id,
                Text = type.Name
            });

            return Json(data);
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddAcccessoryDescription(AccessoryDescriptionProxy accessoryDescriptionProxy)
        {
            if (ModelState.IsValid)
            {
                AccessoryDescriptionModel model = LoadModel<AccessoryDescriptionModel>();
                try
                {
                    model.AddAccessoryDescription(accessoryDescriptionProxy);
                    return Json(new { isSuccess = true });
                }
                catch (Exception exception)
                {
                    return Json(new { isSuccess = false, errorMsg = exception.Message });
                }
            }
            else
            {
                string[] errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
                string errorsStr = string.Join(" ", errors);
                return Json(new { isSuccess = false, errorMsg = errorsStr });
            }
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteAccessoryDescription(int id)
        {
            AccessoryDescriptionModel model = LoadModel<AccessoryDescriptionModel>();
            try
            {
                model.DeleteAccessoryDescription(id);
                return Json(new { isSuccess = true });
            }
            catch (Exception exception)
            {
                return Json(new { isSuccess = false, errorMsg = exception.Message });
            }
        }

        [HttpPost]
        public JsonResult GetAccessoryDescriptions()
        {
            AccessoryDescriptionModel model = LoadModel<AccessoryDescriptionModel>();
            IEnumerable<AccessoryDescription> accessoryDescriptions = model.GetAll();

            var data = accessoryDescriptions.Select(type => new
            {
                Value = type.Id,
                Text = type.Description
            });

            return Json(data);
        }

        [HttpGet]
        public ActionResult Submit(int? id)
        {
            HoldModel<AccessoryRepo> holdModel = LoadModelUser<HoldModel<AccessoryRepo>, int?>(id);
            return View(holdModel);
        }

        [HttpPost]
        public ActionResult Submit(HoldModel<AccessoryRepo> holdModel)
        {
            BuildUp<HoldModel<AccessoryRepo>>(holdModel);
            if (!holdModel.DoesHoldItemNumberExist())
            {
                ModelState.AddModelError("NameUniqueness", "An item with this number does not exist.");
            }

            if (ModelState.IsValid)
            {
                holdModel.Submit();

                TempData["showSuccessNotification"] = true;
                TempData["successNotification"] = "You have successfully submitted an accessory!";

                return RedirectToAction("Search", "Accessory");        
            }
            else
            {
                holdModel.PopulateData();
                return View(holdModel); 
            }
        }

        [HttpGet]
        public async Task<ActionResult> Search()
        {
            var model = LoadModelUser<AccessoryModel, int?>(null);
            model.InitPagination(null);
            await model.InitColumnSelection(model.UserName);
            await model.InitColumnSelection(model.UserName);
            model.GetAllAccessories();

            if (TempData["showSuccessNotification"] != null && (bool)TempData["showSuccessNotification"] == true)
            {
                ViewBag.showSuccessNotification = true;
                ViewBag.successNotification = TempData["successNotification"];
            }

           
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchAjax(AccessoryModel accessoryModel)
        {
            BuildAccessoryModel(accessoryModel);
            
            return PartialView("_AccessoryGrid", accessoryModel);
        }

        [HttpPost]
        public ActionResult SearchAjaxJson(AccessoryModel accessoryModel)
        {
            BuildAccessoryModelJson(accessoryModel);
            AccessoryModelJson model = new AccessoryModelJson(accessoryModel);
            UpdateEmployeeToList(model.Accessories);
            return Json(model.Accessories, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult SearchAjaxJson()
        {
            AccessoryModel accessoryModel = new AccessoryModel();
            BuildAccessoryModelJson(accessoryModel);
            AccessoryModelJson model = new AccessoryModelJson(accessoryModel);
            UpdateEmployeeToList(model.Accessories);
            return Json(model.Accessories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Search(AccessoryModel accessoryModel)
        {
            BuildAccessoryModel(accessoryModel);
            
            return View(accessoryModel);
        }

        private void BuildAccessoryModel(AccessoryModel accessoryModel)
        {
            BuildUp<AccessoryModel>(accessoryModel);
            accessoryModel.InitPagination(accessoryModel.GetSearchAccessoriesCount());
            accessoryModel.SaveColumnSelection(accessoryModel.UserName);
            accessoryModel.GetSearchAccessories();
            accessoryModel.PopulateData();
        }

        private void BuildAccessoryModelJson(AccessoryModel accessoryModel)
        {
            BuildUp<AccessoryModel>(accessoryModel);
            //accessoryModel.InitPagination(accessoryModel.GetSearchAccessoriesCount());
            //accessoryModel.SaveColumnSelection(accessoryModel.UserName);
            accessoryModel.GetSearchAccessoriesJson();
            accessoryModel.PopulateData();
        }
    }
}