using DeviceMate.Core.Helpers;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;

namespace DeviceMate.Web.Models
{
    public class AdminUserModel : BaseModel<UserRepo, int?>, IUserModel
    {
        #region Properties

        [Dependency]
        public StatusRepo StatusRepo { get; set; }
        [Dependency]
        public TeamRepo TeamRepo { get; set; }
        [Dependency]
        public UserGridColumnRepo gridColumnsRepo { get; set; }

        public List<User> AdminUsers { get; set; }
        public AdminUserProxy AdminUser { get; set; }

        public List<SelectListItem> Statuses { get; set; }
        public List<SelectListItem> Teams { get; set; }

        public bool IsAdmin { get; set; }
        public string UserName { get; set; }

        #endregion Properties

        #region Methods

        public override void Init(int? userId)
        {
            this.PopulateData();

            if (userId.HasValue)
            {
                User user = this.Repo.Get(u => u.Id == userId.Value).FirstOrDefault();
                this.AdminUser = new AdminUserProxy
                {
                    Id = user.Id,
                    UserName = user.Email,
                    Email = user.Email,
                    PictureUrl = user.PictureUrl,
                    StatusId = user.StatusId,
                    TeamId = user.TeamId,
                    IsAdmin = user.IsAdmin
                };
            }
            else
            {
                this.AdminUser = new AdminUserProxy();
            }
        }

        public void PopulateData()
        {
            List<SelectListItem> statuses = new List<SelectListItem>();
            statuses.Add(new SelectListItem { Text = "Not selected", Value = string.Empty, Selected = true });
            statuses.AddRange(StatusRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }));
            this.Statuses = statuses;

            List<SelectListItem> teams = new List<SelectListItem>();
            teams.Add(new SelectListItem { Text = "Not selected", Value = string.Empty, Selected = true });
            teams.AddRange(TeamRepo.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }));
            this.Teams = teams;
        }

        public void LoadAllUsers()
        {
            this.AdminUsers = this.Repo.GetNoTracking()
                                        .OrderBy(u => u.Email)
                                        .ToList();
        }

        public int Add()
        {
            User adminUser = new User
            {
                Name = UserHelper.GetNameByEmail(this.AdminUser.Email),
                Email = this.AdminUser.Email,
                StatusId = this.AdminUser.StatusId,
                TeamId = this.AdminUser.TeamId,
                IsAdmin = this.AdminUser.IsAdmin,
                ModifiedDate = DateTime.Now
            };
            return this.Repo.Create(adminUser);
        }

        public void Edt()
        {
            User adminUser = this.Repo.Get(u => u.Id == this.AdminUser.Id.Value).FirstOrDefault();
            adminUser.Name = UserHelper.GetNameByEmail(this.AdminUser.Email);
            adminUser.Email = this.AdminUser.Email;
            adminUser.StatusId = this.AdminUser.StatusId;
            adminUser.TeamId = this.AdminUser.TeamId;
            adminUser.IsAdmin = this.AdminUser.IsAdmin;
            adminUser.ModifiedDate = DateTime.Now;
            this.Repo.SaveChanges();
        }

        public void Delete(int id)
        {
            User user =  this.Repo.Get(u => u.Id == id)
                                .Include("UsersGridColumns")
                                .FirstOrDefault();

            if (user != null)
            {
                gridColumnsRepo.DeleteWithoutSave(user.UsersGridColumns);
                this.Repo.Delete(user);
            }
        }

        #endregion
    }
}