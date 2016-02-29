using DeviceMate.Core;
using DeviceMate.Core.Extensions;
using DeviceMate.Core.Helpers;
using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using DeviceMate.Objects.EmployeesInformation;
using DeviceMate.Objects.Repositories;
using DeviceMate.Services.Cahce;
using DeviceMate.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace DeviceMate.Services
{
    public class UserService : IUserService
    {
        #region Fields
        public readonly UserRepo _userRepository;
        public readonly DeviceRepo _deviceRepository;
        public readonly DeviceHoldsHistoryRepo _deviceHistoryRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public UserService()
        {
            _context = new DeviceContext();
            _userRepository = new UserRepo(_context);
            _deviceRepository = new DeviceRepo(_context);
            _deviceHistoryRepo = new DeviceHoldsHistoryRepo(_context);
        }
        #endregion

        #region IUserService methods

        public UserProxyList GetByFilter(UserFilter filter)
        {
            IQueryable<User> userQuery = _userRepository.GetNoTracking()
                                                        .Include("Town");

            if (filter.StatusId.HasValue)
            {
                userQuery = userQuery.Where(u => u.StatusId == (int)filter.StatusId.Value);
            }
            if (filter.IsAdmin.HasValue)
            {
                userQuery = userQuery.Where(u => u.IsAdmin == filter.IsAdmin.Value);
            }
            if (filter.TeamId.HasValue)
            {
                userQuery = userQuery.Where(u => u.TeamId == filter.TeamId.Value);
            }
            if (filter.TownId.HasValue)
            {
                userQuery = userQuery.Where(u => u.TownId == filter.TownId);
            }

            if (filter.Keywords != null && filter.Keywords.Length > 0)
            {
                foreach (string keyword in filter.Keywords)
                {
                    userQuery = userQuery.Where(u =>
                        u.Name.ToLower().Contains(keyword) ||
                        u.Email.ToLower().Contains(keyword));
                }
            }

            if (filter.Sort != null && filter.Sort.Count() > 0)
            {
                IList<string> sortExpression = new List<string>();
                foreach (KeyValuePair<enSortColumn, enSortOrder> sortItem in filter.Sort)
                {
                    string userProperty = GetSortColumn(sortItem.Key);

                    if (!string.IsNullOrEmpty(userProperty))
                    {
                        sortExpression.Add(string.Format("{0} {1}", userProperty, sortItem.Value.ToString().ToUpper()));
                    }
                }

                if (sortExpression.Count > 0)
                {
                    userQuery = userQuery.OrderBy(string.Join(", ", sortExpression));
                }
            }

            IEnumerable<User> users = userQuery.AsEnumerable();

            UserProxyList usersList = new UserProxyList();

            #region Users paging
            if (filter.Offset.HasValue)
            {
                int maxItems = filter.Limit.HasValue ?
                                filter.Limit.Value : Common.DefaultPageSize;

                users = PagingHelper<User>.GetCurrentPage(users, usersList, filter.Offset.Value, maxItems);
            }
            else
            {
                usersList.TotalItems =
                    usersList.ItemsPerPage =
                    users.Count();
            }
            #endregion
            
            usersList.Users = users.ConvertToUserProxies();

            return usersList;
        }

        public UserProxy GetUserProxyById(int id, bool getHistory = false)
        {
            User user = _userRepository.GetNoTracking(u => u.Id == id && u.StatusId == (int)enUserStatus.Active).FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentException(string.Format("User with ID {0} does not exist", id));
            }

            IList<Device> userDevices = _deviceRepository.GetNoTracking(d => d.Hold.Email == user.Email).ToList();

            IList<DeviceHoldsHistory> userHistory = null;
            
            if (getHistory)
            {
                userHistory = _deviceHistoryRepo.GetNoTracking(h => h.Email == user.Email)
                                                .Include("Device")
                                                .ToList();
            }

            return user.ConvertToUserProxy(userDevices, userHistory);
        }

        public UserProxy GetUserProxyByEmail(string email)
        {
            User user = _userRepository.Get(u => u.Email == email)
                                        .Include("Team")
                                        .Include("Town")
                                        .FirstOrDefault();
            
            if (user == null)
            {
                throw new ArgumentException(string.Format("User with E-mail {0} does not exist", email));
            }

            return user.ConvertToUserProxy();
        }

        public User GetByEmail(string email)
        {
            User user = _userRepository.GetNoTracking(u => u.Email == email).FirstOrDefault();
            return user;
        }

        public IList<User> GetByEmails(IList<string> emails)
        {
            IList<User> users = _userRepository.GetNoTracking(u => emails.Contains(u.Email) && u.StatusId == (int)enUserStatus.Active).ToList();
            return users;
        }

        public User GetById(int id)
        {
            return _userRepository.GetNoTracking(u => u.Id == id).FirstOrDefault();
        }

        public async Task<User> Save(User user)
        {
            user.ModifiedDate = DateTime.Now;
            if (user.Id == 0)
            {
                if (!IsValidUserName(user.Email))
                {
                    throw new InvalidOperationException(string.Format("User with e-mail {0} already exists", user.Email));
                }

                _userRepository.Create(user);
            }
            else
            {
                _userRepository.Update(user);
                _userRepository.SaveChanges();
            }

            await UpdateFromEmplyeeData(ignoreTimeout:true);
            user = _userRepository.GetNoTracking(u => u.Email == user.Email).FirstOrDefault();

            return user;
        }

        public bool SetStatus(int userId, enUserStatus status)
        {
            User user = _userRepository.Get(u => u.Id == userId).FirstOrDefault();

            if (user == null)
            {
                return false;
            }
            else
            {
                user.StatusId = (int)status;
            }

            user.ModifiedDate = DateTime.Now;
            _userRepository.Update(user);
            _userRepository.SaveChanges();
            return true;
        }

        public bool Delete(int userId)
        {
            User user = _userRepository.Get(u => u.Id == userId).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            user.ModifiedDate = DateTime.Now;
            _userRepository.Delete(user);
            return true;
        }
        
        public async Task<User> Add(string email)
        {
            return await Save(new User()
            {
                Email = email,
                Name = UserHelper.GetNameByEmail(email),
                IsAdmin = false,
                StatusId = (int)enUserStatus.Active,
                TeamId = Config.DefaultTeamId,
                ModifiedDate = DateTime.Now
            });
        }

        public bool CheckIfExists(string email)
        {
            return _userRepository.GetNoTracking(u => u.Email == email).Any();
        }

        public bool SetAdminStatus(int userId, bool isAdmin, int? teamId = null)
        {
            User user = _userRepository.Get(u => u.Id == userId).FirstOrDefault();
            
            if (user == null)
            {
                throw new InvalidOperationException(string.Format("User with ID {0} does not exist", userId));
            }

            if (isAdmin && !teamId.HasValue)
            {
                throw new ArgumentNullException("teamId", "Admin user must have team.");
            }
            else if (isAdmin)
            {
                user.TeamId = teamId;
            }
            else
            {
                user.TeamId = null;
            }

            user.IsAdmin = isAdmin;
            user.ModifiedDate = DateTime.Now;

            _userRepository.Update(user);
            _userRepository.SaveChanges();

            return true;
        }

        #region Update from MentorMate emplyee data
        public async Task<bool> UpdateFromEmplyeeData(string email)
        {
            IList<string> emails = new List<string>();
            emails.Add(email);
            return await UpdateFromEmplyeeData(emails, true, false);
        }

        public async Task<bool> UpdateFromEmplyeeData(IEnumerable<string> emails = null, bool ignoreTimeout = false, bool updateAll = false)
        {
            ICacheManager cacheManager = new MemoryCacheManager();
            DateTime lastUpdateTime = DateTime.MinValue;
            if (cacheManager.IsSet(Common.EmployeeUpdateTimeCache))
            {
                lastUpdateTime = cacheManager.Get<DateTime>(Common.EmployeeUpdateTimeCache);
            }

            if (ignoreTimeout || lastUpdateTime.AddHours(Common.EmployeeUpdatePeriodInHours) < DateTime.UtcNow)
            {
                cacheManager.Set(Common.EmployeeUpdateTimeCache, DateTime.UtcNow, Common.AvarageCacheTimeInMunites);

                Dictionary<string, Employee> holders = null;
                List<string> userEmails = new List<string>();

                if (emails != null && emails.Any())
                {
                    userEmails = emails.ToList();
                }
                else
                {
                    userEmails.AddRange(_userRepository.GetNoTracking().Select(u => u.Email).Distinct().ToList());
                    userEmails = userEmails.Distinct().ToList();
                }

                try
                {
                    EmployeesInfoExtractor extractor = new EmployeesInfoExtractor(userEmails);
                    holders = await extractor.Extract(updateAll);

                    List<User> users = _userRepository.Get(m => holders.Keys.Contains(m.Email))
                                                    .ToList();

                    users.ForEach(u =>
                    {
                        if (holders.Keys.Contains(u.Email) && holders[u.Email] != null)
                        {
                            u.Skype = holders[u.Email].Skype;
                            u.PictureUrl = holders[u.Email].PictureUrl;
                            u.PictureResourceId = holders[u.Email].PictureResourceId;
                            u.Position = holders[u.Email].Position;
                            u.Name = holders[u.Email].Name;
                            u.ModifiedDate = DateTime.Now;

                            if (holders[u.Email].Town != enTown.Unknown)
                            {
                                u.TownId = (int)holders[u.Email].Town;
                            }

                            _userRepository.Update(u);
                        }
                    });

                    if (updateAll)
                    {
                        users = _userRepository.Get(u => u.Email != null).ToList();

                        List<Employee> newEmployees = holders.Where(h => !users.Any(u => u.Email == h.Key) &&
                                                                            h.Value.Town != enTown.Unknown)
                                                                .Select(h => h.Value)
                                                                .ToList();

                        foreach (Employee employee in newEmployees)
                        {
                            _userRepository.Add(new User()
                            {
                                Email = employee.Email,
                                Skype = employee.Skype,
                                Name = employee.Name,
                                PictureUrl = employee.PictureUrl,
                                PictureResourceId = employee.PictureResourceId,
                                Position = employee.Position,
                                TownId = (int)employee.Town,
                                StatusId = (int)enUserStatus.Active,
                                IsAdmin = false,
                                ModifiedDate = DateTime.Now
                            });
                        }
                    }

                    _userRepository.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Logging.Logger.LogException(ex);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        #endregion
        
        #endregion

        #region Helper methods
        private bool IsValidUserName(string userName)
        {
            return !_userRepository.GetNoTracking(u => u.Email == userName).Any();
        }

        private string GetSortColumn(enSortColumn column)
        {
            string userProperty;
            switch (column)
            {
                case enSortColumn.Name:
                    userProperty = "Name";
                    break;
                case enSortColumn.Email:
                    userProperty = "Email";
                    break;
                case enSortColumn.Town:
                    userProperty = "Town.Name";
                    break;
                case enSortColumn.Team:
                    userProperty = "Team.Name";
                    break;
                default:
                    userProperty = string.Empty;
                    break;
            }
            return userProperty;
        }
        #endregion
    }
}
