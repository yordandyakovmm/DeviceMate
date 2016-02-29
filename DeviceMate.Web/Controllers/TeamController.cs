using DeviceMate.Web.Controllers.Abstract;
using DeviceMate.Web.Models;
using System.Web.Mvc;
using DMC = DeviceMate.Core;

namespace DeviceMate.Web.Controllers
{
    [Authorize(Roles = DMC.Common.AdminUserRole)]
    public class TeamController : BaseController
    {
        // GET: /Team/

        public ActionResult Index()
        {
            var model = LoadModelUser<TeamModel, int?>(null);
            return View(model);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int? id)
        {
            var model = LoadModelUser<TeamModel, int?>(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddOrEdit(TeamModel teamModel)
        {
            BuildUp<TeamModel>(teamModel);

            if (teamModel.IsTeamNameTaken())
            {
                ModelState.AddModelError("Team.Name", "This team name has already been taken.");
            }

            if (ModelState.IsValid)
            {
                if (teamModel.Team.Id.HasValue)
                {
                    teamModel.Edit();
                }
                else
                {
                    teamModel.Add();
                }

                TempData["showSuccessNotification"] = true;
                if (teamModel.Team.Id.HasValue)
                {
                    TempData["successNotification"] = "You have successfully edited a team!";
                }
                else
                {
                    TempData["successNotification"] = "You have successfully added a team!";
                }

                return RedirectToAction("Index");
            }
            else
            {
                //teamModel.PopulateData();
                return View(teamModel);
            }           
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            
                TeamModel teamModel = LoadModelUser<TeamModel, int?>(id);
                if(teamModel.Delete(id))
                {
                    return Json(new { isSuccess = true, Msg = "You have successfully deleted a team!" });
                }
                else
                {
                    return Json(new { isSuccess = false, Msg = "You can't delete this team!" });
                }
            
        }


    }
}
