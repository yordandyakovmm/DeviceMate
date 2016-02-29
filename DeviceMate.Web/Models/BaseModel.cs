using DeviceMate.Models.Entities;
using DeviceMate.Objects.EmployeesInformation;
using DeviceMate.Objects.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceMate.Web.Models
{
    public abstract class BaseModel<TRepo> : IModel
    {
        #region Properties

        [Dependency]
        public TRepo Repo { get; set; }

        #endregion

        #region Methods

        public virtual void Init() { }

        #endregion
    }

    public abstract class BaseModel<TRepo, TEntity> : IModel<TEntity>

    {
        #region Properties

        [Dependency]
        public TRepo Repo { get; set; }
        #endregion

        #region Methods
        public virtual void Init(TEntity entity) { }
        #endregion
    }

    public interface ISortingPaginationColumnSelection
    {
        Sorter Sorter { get; set; }
        Pager Pager { get; set; }
        ColumnSelector ColumnSelector { get; set; }

        void InitPagination(int? totalNumberOfItems);
        Task InitColumnSelection(string userEmail);
    }

    public abstract class BaseModelWithSortingAndPaginationAndColumnSelection<TRepo, TEntityType, TEntity> : BaseModel<TRepo, TEntity>, ISortingPaginationColumnSelection
        where TEntityType : class
        where TRepo : BaseRepo<TEntityType>
    {
        #region Properties
        public Sorter Sorter { get; set; }
        public Pager Pager { get; set; }
        public ColumnSelector ColumnSelector { get; set; }
        public int TotalNumberOfDevices { protected set; get; }

        [Dependency]
        public UserRepo UserRepo { get; set; }
        [Dependency]
        public GridRepo GridRepo { get; set; }
        [Dependency]
        public GridColumnRepo GridColumnRepo { get; set; }
        [Dependency]
        public UserGridColumnRepo UserGridColumnRepo { get; set; }
        #endregion

        #region Methods
        public void InitPagination(int? totalNumberOfItems)
        {
            Pager = Pager ?? new Pager();
            Pager.Init(totalNumberOfItems.HasValue ? totalNumberOfItems.Value : Repo.GetAllCount());
        }

        public void SaveColumnSelection(string userEmail)
        {
            Grid currentGrid = this.GridRepo.GetByName(typeof(TEntityType).Name);
            if (currentGrid == null)
            {
                throw new NotSupportedException(string.Format("Grid '{0}' is not supported by the application.", typeof(TEntityType).Name));
            }

            IList<GridColumn> columnsOfGrid = this.GridColumnRepo.GetByGridId(currentGrid.Id);
            if (!columnsOfGrid.Any())
            {
                throw new ArgumentException(string.Format("Grid ID '{0}' does not have any columns."));
            }

            User currentUser = this.UserRepo.GetByEmail(userEmail);
            if (currentUser == null)
            {
                throw new ArgumentException(string.Format("User '{0}' was not found in database.", userEmail));
            }

            ColumnSelector.UserGridColumns = this.UserGridColumnRepo.GetByUserIdAndGridId(currentUser.Id, currentGrid.Id);
            bool hasChanges = false;
            foreach (var userGridColumn in ColumnSelector.UserGridColumns)
            {
                bool found = false;
                foreach (var userColumnId in ColumnSelector.UserGridColumnsIds)
                {
                    if (userGridColumn.Id != userColumnId)
                    {
                        continue;
                    }

                    if (userGridColumn.Visible)
                    {
                        found = true;
                        continue;
                    }

                    userGridColumn.Visible = true;
                    hasChanges = true;
                    found = true;
                    break;
                }

                if (found)
                {
                    continue;
                }

                userGridColumn.Visible = false;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UserGridColumnRepo.SaveChanges();
            }
        }

        public async Task InitColumnSelection(string userEmail)
        {
            Grid currentGrid = this.GridRepo.GetByName(typeof(TEntityType).Name);
            if (currentGrid == null)
            {
                throw new NotSupportedException(string.Format("Grid '{0}' is not supported by the application.", typeof(TEntityType).Name));
            }

            IList<GridColumn> columnsOfGrid = this.GridColumnRepo.GetByGridId(currentGrid.Id);
            if (!columnsOfGrid.Any())
            {
                throw new ArgumentException(string.Format("Grid ID '{0}' does not have any columns."));
            }

            User currentUser = this.UserRepo.GetByEmail(userEmail);
            if (currentUser == null)
            {
                currentUser = new User { Email = userEmail };
                this.UserRepo.Add(currentUser);
                this.UserRepo.SaveChanges();
                await updateUserInfoFormGoogleApi();
            }

            IList<UsersGridColumn> userColumnsOfGrid = this.UserGridColumnRepo.GetByUserIdAndGridId(currentUser.Id, currentGrid.Id);
            IList<UsersGridColumn> userColumnsOfGridCache = userColumnsOfGrid.ToList();
            bool hasNewColumns = false;
            foreach (var column in columnsOfGrid)
            {
                if (userColumnsOfGrid.Any(userColumn => column.Id == userColumn.GridColumnId))
                {
                    continue; 
                }

                var newUserGridColumn = new UsersGridColumn
                {
                    GridColumn = column, 
                    User = currentUser, 
                    Visible = true
                };
                hasNewColumns = true;
                this.UserGridColumnRepo.Add(newUserGridColumn);
                userColumnsOfGridCache.Add(newUserGridColumn);
            }


            if (hasNewColumns)
            {
                this.UserGridColumnRepo.SaveChanges();
            }

            ColumnSelector = new ColumnSelector
            {
                UserGridColumns = userColumnsOfGridCache,
                UserGridColumnsIds = userColumnsOfGridCache.Where(x => x.Visible).Select(x => x.Id).ToList()
            };
        }

        public async Task<string> updateUserInfoFormGoogleApi(string email = null)
        {
            Dictionary<string, Employee> holders = null;
            List<string> userEmails = new List<string>();
            if (string.IsNullOrEmpty(email))
            {
                userEmails.AddRange(UserRepo.Context.Users.Select(u => u.Email).Distinct().ToList<string>());
            }
            else
            {
                userEmails.Add(email);
            }
            try
            {
                EmployeesInfoExtractor extractor = new EmployeesInfoExtractor(userEmails);
                holders = await extractor.Extract(string.IsNullOrEmpty(email)); //Update all user data if no concrete e-mail is provided.
                UserRepo.Context.Users.Where(m => holders.Keys.Contains(m.Email)).ToList<User>().ForEach(m => { if (holders[m.Email] == null) return; m.Skype = holders[m.Email].Skype; m.PictureUrl = holders[m.Email].PictureUrl; m.PictureResourceId = holders[m.Email].PictureResourceId; m.Position = holders[m.Email].Position; m.Name = holders[m.Email].Name; });
                UserRepo.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }

            return "OK";

        }

        #endregion
    }
}