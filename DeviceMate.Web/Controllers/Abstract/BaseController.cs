using DeviceMate.Core.Extensions;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.EmployeesInformation;
using DeviceMate.Web.Common;
using DeviceMate.Web.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DeviceMate.Web.Controllers.Abstract
{
    public class BaseController : Controller
    {
        protected static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Private Properties

        private DeviceContext context;
        private DeviceContext Context
        {
            get
            {
                if (this.context == null)
                {
                    this.context = new DeviceContext();
                }
                return this.context;
            }
        }

        private IUnityContainer UnityContainer
        {
            get
            {
                var container = HttpContext.Application["container"];
                if (container == null && !(container is IUnityContainer))
                {
                    throw new Exception("Unity container is not initialized.");
                }
                return container as IUnityContainer;
            }
        }
        #endregion

        #region protected function

        protected List<Employee> getHolders()
        {
            List<Employee> holders = new List<Employee>();
            List<User> user = Context.Users.Distinct().ToList<User>();
            List<User> adminUser = user.Where(u => u.IsAdmin).ToList();
            var query = from u in user
                        where u.Email != null && u.Email != " "
                        select new Employee() { Email = u.Email, Skype = u.Skype, Name = u.Name, PictureResourceId = u.PictureResourceId, PictureUrl = u.PictureUrl, Position = u.Position };
            holders.AddRange(query);
            query = from u in adminUser
                    where u.Email != null && u.Email != " "
                    select new Employee() { Email = u.Email, Skype = u.Skype, Name = u.Name, PictureResourceId = u.PictureResourceId, PictureUrl = u.PictureUrl, Position = u.Position };
            holders.AddRange(query);
            
            return holders;
        }

        protected void UpdateEmployeeToList<T> (List<T> list) where T : BaseJson 
        {
            List<Employee> holders = new List<Employee>();
            List<User> user = Context.Users.Distinct().ToList<User>();
            List<User> adminUser = user.Where(u => u.IsAdmin).ToList();
            var query = from u in user
                        where u.Email != null && u.Email != " "
                        select new Employee() { Email = u.Email, Skype = u.Skype, Name = u.Name, PictureResourceId = u.PictureResourceId, PictureUrl = u.PictureUrl, Position = u.Position };
            holders.AddRange(query);
            query = from u in adminUser
                    where u.Email != null && u.Email != " " 
                    select new Employee() { Email = u.Email, Skype = u.Skype, Name = u.Name, PictureResourceId = u.PictureResourceId, PictureUrl = u.PictureUrl, Position = u.Position };
            holders.AddRange(query);
            holders = holders.Distinct().ToList();
            Dictionary<string, Employee> eHolders = new Dictionary<string, Employee>();
            holders.ForEach(h => { if (!eHolders.Keys.Contains(h.Email)) eHolders.Add(h.Email, h); });
            list.ForEach(m => { if (eHolders.Keys.Contains(m.emailEmployee)) m.employee = eHolders[m.emailEmployee]; });
        }


        protected async Task<string> updateUserInfoFormGoogleApi(string email = null)
        {
            DeviceModel model = new DeviceModel();
            BuildUp<DeviceModel>(model);
            return await model.updateUserInfoFormGoogleApi(email);
        }
                

        #endregion

        #region Lifecycle Handlers

        protected override void OnException(ExceptionContext filterContext)
        {
            // Wrap Ajax responses
            if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Exception != null)
            {
                string message = filterContext.Exception.Message;
                logger.InfoFormat("AJAX request failed: {0}", message);

                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                filterContext.Result = new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Message = message,
#if DEBUG
                        filterContext.Exception.StackTrace
#endif
                    }
                };
            }
            // Log exception & redirect
            else
            {
                Exception ex = filterContext.Exception;
                if (ex is HttpUnhandledException && ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                logger.Error(ex.Message, ex);
                string innerErrorMessage = GetInnerMessage(ex);
                TempData["Exception"] = innerErrorMessage;
                //string innerErrorMessage = GetInnerMessage(ex);
                filterContext.Result = RedirectToAction("Error", "Home", new { area = String.Empty, exception=ex.Message });
            }

            // mark handled exception when ajax call or no debugger is attached
            if (filterContext.HttpContext.Request.IsAjaxRequest() || !System.Diagnostics.Debugger.IsAttached)
            {
                filterContext.ExceptionHandled = true;
            }

            //Log with NLog
            Exception nEx = filterContext.Exception.RecursiveGetInnerException();

            var nlog = NLog.LogManager.GetLogger("ExeptionHandler");
            nlog.LogException(nEx);
        }

        private string GetInnerMessage(Exception e)
        {
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }
            return e.Message;
        }

        #endregion

        #region Helpers

        protected internal T LoadModelUser<T>() where T : IModel, IUserModel
        {
            T result = UnityContainer.Resolve<T>(new ParameterOverride("context", this.Context));
            result.Init();
            InitUserModel<T>(result);

            return result;
        }

        protected internal M LoadModelUser<M, D>(D data) where M : IModel<D>, IUserModel
        {
            M result = this.UnityContainer.Resolve<M>(new ParameterOverride("context", this.Context));
            result.Init(data);
            InitUserModel<M>(result);

            return result;
        }

        protected internal M LoadModel<M, D>(D data) where M : IModel<D>
        {
            M result = this.UnityContainer.Resolve<M>(new ParameterOverride("context", this.Context));
            result.Init(data);

            return result;
        }

        protected T LoadModel<T>() where T : IModel
        {
            T result = UnityContainer.Resolve<T>(new ParameterOverride("context", this.Context));
            result.Init();

            return result;
        }

        protected internal T BuildUp<T>(T obj) where T : IUserModel
        {
            T result = this.UnityContainer.BuildUp<T>(obj, new ParameterOverride("context", this.Context));
            InitUserModel<T>(result);
            return result;
        }

        protected internal T InitUserModel<T>(T obj) where T : IUserModel
        {
            obj.IsAdmin = Roles.IsUserInRole("Admin");
            obj.UserName = HttpContext.User.Identity.Name;
            return obj;
        }

       

        #endregion
    }
}