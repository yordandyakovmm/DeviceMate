using DeviceMate.Core;
using DeviceMate.Core.Extensions;
using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using DeviceMate.Objects.Repositories;
using DeviceMate.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;

namespace DeviceMate.Services
{
    public class AccessoryService : IAccessoryService
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly AccessoryRepo _accessoryRepo;
        private readonly UserRepo _userRepo;
        private readonly HoldRepo _holdRepo;
        private readonly AccessoryHoldsHistoryRepo _accessoryHistoryRepo;
        private readonly DeviceContext _context;
        #endregion

        #region Constructor
        public AccessoryService(IUserService userService)
        {
            _context = new DeviceContext();
            _accessoryRepo = new AccessoryRepo(_context);
            _userRepo = new UserRepo(_context);
            _holdRepo = new HoldRepo(_context);
            _accessoryHistoryRepo = new AccessoryHoldsHistoryRepo(_context);
            _userService = userService;
        }
        #endregion

        #region IAccessoryService

        public AccessoryProxyList GetByFilter(AccessoryFilter filter)
        {
            IQueryable<Accessory> accessoryQuery = _accessoryRepo.GetNoTracking()
                                                .Include("AccessoryType")
                                                .Include("OSs")
                                                .Include("Hold.Team")
                                                .Include("Hold.Town")
                                                .Include("AccessoryDescription")
                                                .Include("Color");

            if (filter.TownId.HasValue)
            {
                accessoryQuery = accessoryQuery.Where(a => a.Hold.TownID == filter.TownId);
            }
            if (filter.OsId.HasValue)
            {
                accessoryQuery = accessoryQuery.Where(a => a.OsId == filter.OsId);
            }
            if (filter.TeamId.HasValue)
            {
                accessoryQuery = accessoryQuery.Where(a => a.Hold.TeamId == filter.TeamId);
            }
            if (filter.ColorId.HasValue)
            {
                accessoryQuery = accessoryQuery.Where(a => a.ColorId == filter.ColorId);
            }
            if (filter.TypeId.HasValue)
            {
                accessoryQuery = accessoryQuery.Where(a => a.TypeId == filter.TypeId);
            }
            if (filter.IsAvailable.HasValue)
            {
                accessoryQuery = accessoryQuery.Where(a => !a.Hold.IsBusy == filter.IsAvailable);
            }

            if (filter.Keywords != null && filter.Keywords.Length > 0)
            {
                foreach (string keyword in filter.Keywords)
                {
                    accessoryQuery = accessoryQuery.Where(a => a.Number.ToLower().Contains(keyword) ||
                                                a.AccessoryDescription.Description.ToLower().Contains(keyword) ||
                                                a.AccessoryType.Name.ToLower().Contains(keyword) ||
                                                a.SerialNumber.ToLower().Contains(keyword) ||
                                                a.Hold.Email.ToLower().Contains(keyword));
                }
            }

            if (filter.Sort != null && filter.Sort.Count() > 0)
            {
                IList<string> sortExpression = new List<string>();
                foreach (KeyValuePair<enSortColumn, enSortOrder> sortItem in filter.Sort)
                {
                    string accessoryProperty = GetSortColumn(sortItem.Key);

                    if (!string.IsNullOrEmpty(accessoryProperty))
                    {
                        sortExpression.Add(string.Format("{0} {1}", accessoryProperty, sortItem.Value.ToString().ToUpper()));
                    }
                }

                if (sortExpression.Count > 0)
                {
                    accessoryQuery = accessoryQuery.OrderBy(string.Join(", ", sortExpression));
                }
            }

            IEnumerable<Accessory> accessories = accessoryQuery.AsEnumerable();

            AccessoryProxyList accessoriesList = new AccessoryProxyList();

            #region Accessories paging
            accessoriesList.AvailableCount = accessories.Count(d => !d.Hold.IsBusy);

            if (filter.Offset.HasValue)
            {
                int maxItems = filter.Limit.HasValue ?
                                filter.Limit.Value : Common.DefaultPageSize;

                accessories = PagingHelper<Accessory>.GetCurrentPage(accessories, accessoriesList, filter.Offset.Value, maxItems);
            }
            #endregion

            IList<string> holdEmails = accessories.Select(d => d.Hold.Email).Distinct().ToList();
            IList<User> holders = _userService.GetByEmails(holdEmails);

            accessoriesList.Accessories = accessories.ConvertToAccessoryProxies(holders);

            return accessoriesList;
        }

        public AccessoryProxy GetById(int id)
        {
            Accessory accessory = _accessoryRepo.GetById(id);
            User holder = _userService.GetByEmail(accessory.Hold.Email);
            return accessory.ConvertToAccessoryProxy(holder);
        }

        public void SubmitToHolder(int accessoryId, int userId, int teamId, bool isBusy, enTown town)
        {
            Accessory accessory = _accessoryRepo.Get(d => d.Id == accessoryId)
                                                .FirstOrDefault();

            if (accessory == null)
            {
                throw new ArgumentException(string.Format("Device with ID {0} does not exists.", accessoryId));
            }

            User user = _userService.GetById(userId);

            if (user == null)
            {
                throw new ArgumentException(string.Format("User with ID {0} does not exists.", userId));
            }

            if (town == enTown.Unknown)
            {
                town = user.TownId.HasValue ? (enTown)user.TownId : enTown.Sofia;
            }

            AccessoryHoldsHistory holdHistory = new AccessoryHoldsHistory()
            {
                AccessoryId = accessory.Id,
                HoldDate = DateTime.Now,
                Email = user.Email,
                TeamId = teamId,
                IsBusy = isBusy,
                TownId = (int)town

            };

            accessory.Hold.HoldDate = DateTime.Now;
            accessory.Hold.Email = user.Email;
            accessory.Hold.IsBusy = isBusy;
            accessory.Hold.TeamId = teamId;
            accessory.Hold.TownID = (int)town;

            _accessoryHistoryRepo.Add(holdHistory);
            _accessoryRepo.Update(accessory);
            var a = _accessoryRepo.SaveChanges();
        }

        public void Remove(int id)
        {
            Accessory accessory = _accessoryRepo.Get(d => d.Id == id)
                                                .Include("Hold")
                                                .Include("AccessoryHoldsHistories")
                                                .FirstOrDefault();

            if (accessory != null)
            {
                _holdRepo.DeleteWithoutSave(accessory.Hold);
                _accessoryHistoryRepo.DeleteWithoutSave(accessory.AccessoryHoldsHistories);
                _accessoryRepo.DeleteWithoutSave(accessory);
                _accessoryRepo.SaveChanges();
            }
        }

        public bool Add(AccessoryProxy simpleAccessory)
        {
            Accessory accessory = simpleAccessory.ConvertToAccessory();

            if (accessory.Hold.TownID == 0)
            {
                User user = _userService.GetByEmail(accessory.Hold.Email);
                accessory.Hold.TownID = user.TownId.HasValue ? user.TownId.Value : (int)enTown.Sofia;
            }

            if (accessory != null)
            {
                _accessoryRepo.Add(accessory);
                _holdRepo.Add(accessory.Hold);
                _accessoryHistoryRepo.Add(accessory.AccessoryHoldsHistories);
                _accessoryRepo.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Edit(AccessoryProxy simpleAccessory)
        {
            Accessory accessory = _accessoryRepo.GetById(simpleAccessory.Id);

            if (accessory != null)
            {
                accessory.UpdateWithAccessoryProxy(simpleAccessory);
                _accessoryRepo.Update(accessory);
                _accessoryRepo.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Helper methods

        private string GetSortColumn(enSortColumn column)
        {
            string accessoryProperty;
            switch (column)
            {
                case enSortColumn.Available:
                    accessoryProperty = "Hold.IsBusy";
                    break;
                case enSortColumn.Color:
                    accessoryProperty = "Color.Name";
                    break;
                case enSortColumn.DateTaken:
                    accessoryProperty = "Hold.HoldDate";
                    break;
                case enSortColumn.Os:
                    accessoryProperty = "OSs.Name";
                    break;
                case enSortColumn.Team:
                    accessoryProperty = "Hold.Team.Name";
                    break;
                case enSortColumn.Town:
                    accessoryProperty = "Hold.Town.Name";
                    break;
                case enSortColumn.Email:
                    accessoryProperty = "Hold.Email";
                    break;
                case enSortColumn.Info:
                    accessoryProperty = "SerialNumber";
                    break;
                case enSortColumn.Type:
                    accessoryProperty = "AccessoryType.Name";
                    break;
                case enSortColumn.Description:
                    accessoryProperty = "AccessoryDescription.Description";
                    break;
                default:
                    accessoryProperty = string.Empty;
                    break;
            }
            return accessoryProperty;
        }

        #endregion
    }
}
