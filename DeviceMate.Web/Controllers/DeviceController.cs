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
    public class DeviceController : BaseController
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

            var model = LoadModelUser<DeviceModel, int?>(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddEdit(DeviceModel deviceModel)
        {
            BuildUp<DeviceModel>(deviceModel);
            if (deviceModel.IsAccessoryNameTaken())
            {
                ModelState.AddModelError("NameUniqueness", "This number has already been taken.");
            }

            if (ModelState.IsValid)
            {
                deviceModel.SaveDevice();

                TempData["showSuccessNotification"] = true;
                if (deviceModel.Device.Id.HasValue)
                {
                    TempData["successNotification"] = "You have successfully edited a device!";
                }
                else
                {
                    TempData["successNotification"] = "You have successfully added a device!";
                }

                return RedirectToAction("AddEdit", "Device");
            }
            else
            {
                deviceModel.PopulateData(deviceModel.Device.OsId, deviceModel.Device.ManufacturerId);
                return View(deviceModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ActionResult Delete(int id)
        {
            try
            {
                DeviceModel deviceModel = LoadModelUser<DeviceModel, int?>(id);
                deviceModel.Delete(id);
                return Json(new { isSuccess = true, Msg = "You have successfully deleted a device!" });
            }
            catch (Exception exception)
            {
                return Json(new { isSuccess = false, Msg = exception.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public ActionResult Admin()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Search()
        {
            var model = LoadModelUser<DeviceModel, int?>(null);
            model.InitPagination(null);
            await model.InitColumnSelection(model.UserName);
            
            model.GetAllDevices();

            if (TempData["showSuccessNotification"] != null && (bool)TempData["showSuccessNotification"] == true)
            {
                ViewBag.showSuccessNotification = true;
                ViewBag.successNotification = TempData["successNotification"];
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(DeviceModel deviceModel)
        {
            BuildDeviceModel(deviceModel);

            return View(deviceModel);
        }

        [HttpPost]
        public ActionResult SearchAjax(DeviceModel deviceModel)
        {
            BuildDeviceModel(deviceModel);
            var modelJson = new DeviceModelJson(deviceModel);
            return Json(modelJson.Devices, JsonRequestBehavior.AllowGet);     
        }

        [HttpPost]
        public JsonResult SearchAjaxJson(SearchFilterModel searchFilter)
        {

            //List<Employee> holders = getHolders();
            bool isAllPropEmpty = searchFilter.GetType().GetProperties().All(p => p.GetValue(searchFilter) == null);
            if (isAllPropEmpty)
            {
                searchFilter = null;
            }
            DeviceModel deviceModel = new DeviceModel();
            deviceModel.SearchFilter = searchFilter;
            BuildDeviceModelJson(deviceModel);
            
            var modelJson = new DeviceModelJson(deviceModel);
            UpdateEmployeeToList(modelJson.Devices);
            return Json(modelJson.Devices, JsonRequestBehavior.AllowGet);
        }
        
        private void BuildDeviceModel(DeviceModel deviceModel)
        {
            BuildUp<DeviceModel>(deviceModel);
            deviceModel.GetSearchedDevicesJson();
            if (deviceModel.SearchFilter != null)
            {
                deviceModel.PopulateData(deviceModel.SearchFilter.OsId, deviceModel.SearchFilter.ManufacturerId);
            }
            else
            {
                deviceModel.PopulateData();
            }
        }

        private void BuildDeviceModelJson(DeviceModel deviceModel)
        {
            BuildUp<DeviceModel>(deviceModel);
            deviceModel.GetSearchedDevicesJson();
            if (deviceModel.SearchFilter != null)
            {
                deviceModel.PopulateData(deviceModel.SearchFilter.OsId, deviceModel.SearchFilter.ManufacturerId);
            }
            else
            {
                deviceModel.PopulateData();
            }
        }

        [HttpGet]
        public ActionResult Submit(int? id)
        {
            var model = LoadModelUser<DeviceModel, int?>(id);
            int? holdId = null;
            if (model.Device != null && model.Device.Id.HasValue) { holdId = model.Device.HoldId; }
            HoldModel<DeviceRepo> holdModel = LoadModelUser<HoldModel<DeviceRepo>, int?>(holdId);
            return View(holdModel);
        }

        [HttpGet]
        public ActionResult IsAvailable(int id)
        {
            try
            {
                DeviceModel deviceModel = LoadModelUser<DeviceModel, int?>(null);
                deviceModel.IsAvailable(id);
                return RedirectToAction("Search", "Device");
            }
            catch (Exception exception)
            {
                return Json(new { isSuccess = false, Msg = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Submit(HoldModel<DeviceRepo> holdModel)
        {
            BuildUp<HoldModel<DeviceRepo>>(holdModel);
            if (!holdModel.DoesHoldItemNumberExist())
            {
                ModelState.AddModelError("NameUniqueness", "An item with this number does not exist.");
            }

            if (ModelState.IsValid)
            {
                holdModel.Submit();

                TempData["showSuccessNotification"] = true;
                TempData["successNotification"] = "You have successfully submitted a device!";
                return RedirectToAction("Search", "Device");
            }
            else
            {
                holdModel.PopulateData();
                return View(holdModel);
            }
        }

        [HttpPost]
        public JsonResult GetOSs()
        {
            OsModel OSModel = LoadModel<OsModel>();
            IEnumerable<OSs> OSs = OSModel.GetAll();

            var data = OSs.Select(OS => new
            {
                Value = OS.Id,
                Text = OS.Name
            });

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetModels(int idManufacturer)
        {
            var model = LoadModel<ModelModel>();
            var result = model.GetModels(idManufacturer);
            var data = result.Select(a => new
            {
                Value = a.Id,
                Text = a.Name
            });

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetManufacturers(int idOs)
        {
            var model = LoadModel<ManufacturerModel>();
            var result = model.GetManufacturer(idOs);
            var data = result.Select(a => new
            {
                Value = a.Id,
                Text = a.Name
            });

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetColors()
        {
            var colorModel = LoadModel<ColorModel>();
            var colors = colorModel.GetAll();

            var data = colors.Select(c => new
            {
                Value = c.Id,
                Text = c.Name
            });

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetTowns()
        {
            var townModel = LoadModel<TownModel>();
            var towns = townModel.GetAll();

            var data = towns.Select(c => new
            {
                Value = c.TownId,
                Text = c.Name
            });

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetScreenSizes()
        {
            var screenSizeModel = LoadModel<ScreenSizeModel>();
            var screenSizes = screenSizeModel.GetAll();

            var data = screenSizes.Select(sz => new
            {
                Value = sz.Id,
                Text = sz.Size.ToString("0.##")
            });

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetWidths()
        {
            var resolutionModel = LoadModel<ResolutionModel>();
            var widths = resolutionModel.GetAllWidths();

            var data = widths.Select(w => new
            {
                Value = w.Id,
                Text = w.Width
            });

            return Json(data);
        }

        [HttpPost]
        public JsonResult GetHeights()
        {
            var resolutionModel = LoadModel<ResolutionModel>();
            var heights = resolutionModel.GetAllHeights();

            var data = heights.Select(h => new
            {
                Value = h.Id,
                Text = h.Height
            });

            return Json(data);
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddColor(ColorProxy color)
        {
            var model = LoadModel<ColorModel>();
            var result = model.SaveColor(color);
            return Json(new { Success = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddOs(string name)
        {
            var model = LoadModel<OsModel>();
            var result = model.SaveOs(name);
            return Json(new { Success = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddModel(ModelProxy modelProxy)
        {
            var model = LoadModel<ModelModel>();
            bool result = model.SaveModel(modelProxy);
            return Json(new { Success = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddManufacturer(ManufacturerProxy manufacturerProxy)
        {
            var model = LoadModel<ManufacturerModel>();
            var result = model.SaveManufacturer(manufacturerProxy);
            return Json(new { Success = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddScreenSize(ScreenSizeProxy screenSizeProxy)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false });     
            }

            var model = LoadModel<ScreenSizeModel>();
            var result = model.SaveScreenSize(screenSizeProxy);
            return Json(new { Success = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddWidth(WidthProxy w)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false });
            }

            var model = LoadModel<ResolutionModel>();
            var result = model.SaveWidth(w);
            return Json(new { Success = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult AddHeight(HeightProxy h)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Success = false });
            }

            var model = LoadModel<ResolutionModel>();
            var result = model.SaveHeight(h);
            return Json(new { Success = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteColor(int id)
        {
            var model = LoadModel<ColorModel>();
            var result = model.DeleteColor(id);
            return Json(new { deleted = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteOs(int id)
        {
            var model = LoadModel<OsModel>();
            var result = model.DeleteOs(id);
            return Json(new { deleted = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteModel(int id)
        {
            var model = LoadModel<ModelModel>();
            var result = model.DeleteModel(id);
            return Json(new { deleted = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteManufacturer(int id)
        {
            var model = LoadModel<ManufacturerModel>();
            var result = model.DeleteManufacturer(id);
            return Json(new { deleted = result });
        }


        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteScreenSize(int id)
        {
            var model = LoadModel<ScreenSizeModel>();
            var result = model.DeleteScreenSize(id);
            return Json(new { deleted = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteWidth(int id)
        {
            var model = LoadModel<ResolutionModel>();
            var result = model.DeleteResolutionWidth(id);
            return Json(new { deleted = result });
        }

        [HttpPost]
        [Authorize(Roles = DMC.Common.AdminUserRole)]
        public JsonResult DeleteHeight(int id)
        {
            var model = LoadModel<ResolutionModel>();
            var result = model.DeleteResolutionHeight(id);
            return Json(new { deleted = result });
        }

        [HttpGet]
        public async Task<ActionResult> History()
        {
            var model = LoadModelUser<DeviceHistoryModel, int?>(null);
            model.Init(null);
            model.InitPagination(null);
            await model.InitColumnSelection(model.UserName);
            model.GetAllDeviceHistories();
            
            return View(model);
        }

        [HttpPost]
        public ActionResult History(DeviceHistoryModel deviceHistoryModel)
        {
            BuildDeviceHistoryModel(deviceHistoryModel);

            return View(deviceHistoryModel);
        }

        [HttpPost]
        public ActionResult HistoryAjax(DeviceHistoryModel deviceHistoryModel)
        {
            BuildDeviceHistoryModel(deviceHistoryModel);

            return PartialView("_DeviceHistoryGrid", deviceHistoryModel);
        }

        private void BuildDeviceHistoryModel(DeviceHistoryModel deviceHistoryModel)
        {
            BuildUp<DeviceHistoryModel>(deviceHistoryModel);
            deviceHistoryModel.InitPagination(deviceHistoryModel.GetSearchedDeviceHistoriesCount());
            deviceHistoryModel.SaveColumnSelection(deviceHistoryModel.UserName);
            deviceHistoryModel.GetSearchedDeviceHistories();
            deviceHistoryModel.PopulateData();
        }
    }
}
