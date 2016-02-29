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
using System.Text.RegularExpressions;

namespace DeviceMate.Services
{
    public class DeviceService : IDeviceService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IOsService _osService;
        private readonly IResolutionService _resolutionService;
        private readonly DeviceRepo _deviceRepo;
        private readonly HoldRepo _holdRepo;
        private readonly DeviceHoldsHistoryRepo _historyRepo;
        private readonly OSRepo _osRepo;
        private readonly ResolutionRepo _resolutionRepo;
        private readonly DeviceContext _context;

        #endregion

        #region Constructor

        public DeviceService(IUserService userService,
                            IOsService osService,
                            IResolutionService resolutionService)
        {
            _context = new DeviceContext();
            _deviceRepo = new DeviceRepo(_context);
            _holdRepo = new HoldRepo(_context);
            _historyRepo = new DeviceHoldsHistoryRepo(_context);
            _osRepo = new OSRepo(_context);
            _resolutionRepo = new ResolutionRepo(_context);
            _userService = userService;
            _osService = osService;
            _resolutionService = resolutionService;
        }

        #endregion

        #region IDeviceService methods

        public DeviceProxyList GetByFilter(DeviceFilter filter)
        {
            IQueryable<Device> deviceQuery = _deviceRepo.GetNoTracking()
                                                .Include("DeviceType")
                                                .Include("Color")
                                                .Include("Model.Manufacturer.OSs")
                                                .Include("Hold.Team")
                                                .Include("Hold.Town")
                                                .Include("ScreenSize")
                                                .Include("Resolution.ResolutionHeightOption")
                                                .Include("Resolution.ResolutionWidthOption");

            if (filter.TownId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.Hold.TownID == filter.TownId);
            }
            if (filter.OsId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.Model.Manufacturer.OsId == filter.OsId);
            }
            if (filter.ManufacturerId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.Model.ManufacturerId == filter.ManufacturerId);
            }
            if (filter.ModelId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.ModelId == filter.ModelId);
            }
            if (filter.TeamId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.Hold.TeamId == filter.TeamId);
            }
            if (filter.ScreenSizeId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.ScreenSizeId == filter.ScreenSizeId);
            }
            if (filter.ResolutionWidthId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.Resolution.ResolutionWidthId == filter.ResolutionWidthId);
            }
            if (filter.ResolutionHeightId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.Resolution.ResolutionHeightId == filter.ResolutionHeightId);
            }
            if (filter.ColorId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.ColorId == filter.ColorId);
            }
            if (filter.TypeId.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => d.DeviceTypeId == filter.TypeId);
            }
            if (filter.IsAvailable.HasValue)
            {
                deviceQuery = deviceQuery.Where(d => !d.Hold.IsBusy == filter.IsAvailable);
            }

            if (filter.Keywords != null && filter.Keywords.Length > 0)
            {
                string firstKeyword = filter.Keywords.First();
                int osId = _osService.GetIdByName(firstKeyword);

                if (filter.OsId.HasValue && filter.Keywords.Length == 1 && IsOsVersionKeyword(firstKeyword))
                {
                    deviceQuery = deviceQuery.Where(d => d.OsVersion.ToLower().Contains(firstKeyword));
                }
                else if (filter.Keywords.Length == 2 && osId != 0 && IsOsVersionKeyword(filter.Keywords[1]))
                {
                    string osVersion = filter.Keywords[1];

                    deviceQuery = deviceQuery.Where(d => d.Model.Manufacturer.OsId == osId &&
                                                        d.OsVersion.Contains(osVersion));
                }
                else
                {
                    foreach (string keyword in filter.Keywords)
                    {
                        deviceQuery = deviceQuery.Where(d => d.Name.ToLower().Contains(keyword) ||
                                                    d.Number.ToLower().Contains(keyword) ||
                                                    d.OsVersion.ToLower().Contains(keyword) ||
                                                    d.Model.Name.ToLower().Contains(keyword) ||
                                                    d.Model.Manufacturer.Name.ToLower().Contains(keyword) ||
                                                    d.Model.Manufacturer.OSs.Name.ToLower().Contains(keyword) ||
                                                    d.Hold.Email.ToLower().Contains(keyword));
                    }
                }
            }

            if (filter.Sort != null && filter.Sort.Count() > 0)
            {
                IList<string> sortExpression = new List<string>();
                foreach (KeyValuePair<enSortColumn, enSortOrder> sortItem in filter.Sort)
                {
                    string deviceProperty = GetSortColumn(sortItem.Key);

                    if (!string.IsNullOrEmpty(deviceProperty))
                    {
                        sortExpression.Add(string.Format("{0} {1}", deviceProperty, sortItem.Value.ToString().ToUpper()));
                    }
                }

                if (sortExpression.Count > 0)
                {
                    deviceQuery = deviceQuery.OrderBy(string.Join(", ", sortExpression));
                }
            }

            IEnumerable<Device> devices = deviceQuery.AsEnumerable();

            DeviceProxyList devicesList = new DeviceProxyList();

            #region Devices paging
            devicesList.AvailableCount = devices.Count(d => !d.Hold.IsBusy);

            if (filter.Offset.HasValue)
            {
                int maxItems = filter.Limit.HasValue ?
                                filter.Limit.Value : Common.DefaultPageSize;

                devices = PagingHelper<Device>.GetCurrentPage(devices, devicesList, filter.Offset.Value, maxItems);
            }
            #endregion

            IList<string> holdEmails = devices.Select(d => d.Hold.Email).Distinct().ToList();
            IList<User> holders = _userService.GetByEmails(holdEmails);

            devicesList.Devices = devices.ConvertToDeviceProxies(holders);

            return devicesList;
        }

        public DeviceProxy GetById(int id)
        {
            Device device = _deviceRepo.GetNoTracking(d => d.Id == id)
                                        .Include("Model.Manufacturer.OSs")
                                        .FirstOrDefault();

            if (device == null)
            {
                throw new ArgumentException(string.Format("Device with ID {0} does not exists.", id));
            }
            
            User holder = _userService.GetByEmail(device.Hold.Email);
            return device.ConvertToDeviceProxy(holder);
        }

        public void SubmitToHolder(int deviceId, int userId, int teamId, bool isBusy, enTown town)
        {
            Device device = _deviceRepo.Get(d => d.Id == deviceId)
                                        .Include("Hold")
                                        .Include("Model.Manufacturer")
                                        .FirstOrDefault();
            
            if (device == null)
            {
                throw new ArgumentException(string.Format("Device with ID {0} does not exists.", deviceId));
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

            //Set other devices possessed by the user to Available, unless the team is Spok
            IList<Hold> userHolds = _holdRepo.Get(h => h.Email == user.Email &&
                                                        h.Team.Name != Config.SpokTeamName &&
                                                        h.IsBusy &&
                                                        h.Devices.Any(d => d.Model.Manufacturer.OsId == device.Model.Manufacturer.OsId))
                                                        .ToList();

            if (userHolds != null)
            {
                foreach (Hold userHold in userHolds)
                {
                    userHold.IsBusy = false;
                    _holdRepo.Update(userHold);
                }
            }


            DeviceHoldsHistory holdHistory = new DeviceHoldsHistory()
            {
                DeviceId = device.Id,
                HoldDate = DateTime.Now,
                Email = user.Email,
                TeamId = teamId,
                IsBusy = isBusy,
                TownID = (int)town
            };

            device.Hold.HoldDate = DateTime.Now;
            device.Hold.Email = user.Email;
            device.Hold.TeamId = teamId;
            device.Hold.IsBusy = isBusy;
            device.Hold.TownID = (int)town;

            _historyRepo.Add(holdHistory);
            _deviceRepo.Update(device);

            _deviceRepo.SaveChanges();
        }

        public void Delete(int id)
        {
            Device device = _deviceRepo.Get(d => d.Id == id)
                                        .Include("Hold")
                                        .Include("DeviceHoldsHistories")
                                        .FirstOrDefault();

            if (device != null)
            {
                _holdRepo.DeleteWithoutSave(device.Hold);
                _historyRepo.DeleteWithoutSave(device.DeviceHoldsHistories);
                _deviceRepo.DeleteWithoutSave(device);
                _deviceRepo.SaveChanges();
            }
        }

        public bool Add(DeviceProxy simpleDevice)
        {
            if (simpleDevice.Resolution.Id == 0)
            {
                simpleDevice.Resolution.Id = _resolutionService.GetIdByWidthAndHeight(simpleDevice.Resolution.Width.Id, simpleDevice.Resolution.Height.Id);
            }

            Device device = simpleDevice.ConvertToDevice();

            if (device.Hold.TownID == 0)
            {
                User user = _userService.GetByEmail(device.Hold.Email);
                device.Hold.TownID = user.TownId.HasValue ? user.TownId.Value : (int)enTown.Sofia;
            }

            if (device != null)
            {
                _deviceRepo.Add(device);
                _holdRepo.Add(device.Hold);
                _historyRepo.Add(device.DeviceHoldsHistories);
                _deviceRepo.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Edit(DeviceProxy simpleDevice)
        {
            Device device = _deviceRepo.GetById(simpleDevice.Id);

            if (device != null)
            {
                device.UpdateWithDeviceProxy(simpleDevice);
                _deviceRepo.Update(device);
                _deviceRepo.SaveChanges();
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
            string deviceProperty;
            switch (column)
            {
                case enSortColumn.Name:
                    deviceProperty = "Name";
                    break;
                case enSortColumn.Available:
                    deviceProperty = "Hold.IsBusy";
                    break;
                case enSortColumn.Color:
                    deviceProperty = "Color.Name";
                    break;
                case enSortColumn.DateTaken:
                    deviceProperty = "Hold.HoldDate";
                    break;
                case enSortColumn.Model:
                    deviceProperty = "Model.Name";
                    break;
                case enSortColumn.Info:
                    deviceProperty = "SerialNumber";
                    break;
                case enSortColumn.Os:
                    deviceProperty = "Model.Manufacturer.OSs.Name";
                    break;
                case enSortColumn.OsVersion:
                    deviceProperty = "OsVersion";
                    break;
                case enSortColumn.ResolutionHeight:
                    deviceProperty = "Resolution.ResolutionHeightOption.Height";
                    break;
                case enSortColumn.ResolutionWidth:
                    deviceProperty = "Resolution.ResolutionWidthOption.Width";
                    break;
                case enSortColumn.ScreenSize:
                    deviceProperty = "ScreenSize.Size";
                    break;
                case enSortColumn.Team:
                    deviceProperty = "Hold.Team.Name";
                    break;
                case enSortColumn.Town:
                    deviceProperty = "Hold.Town.Name";
                    break;
                case enSortColumn.Type:
                    deviceProperty = "DeviceType.Name";
                    break;
                case enSortColumn.Email:
                    deviceProperty = "Hold.Email";
                    break;
                default:
                    deviceProperty = string.Empty;
                    break;
            }
            return deviceProperty;
        }

        private bool IsOsVersionKeyword(string keyword)
        {
            Regex versionRegEx = new Regex(@"^\d*(\.?\d*)(\.?\d*)$");
            return versionRegEx.IsMatch(keyword);
        }

        #endregion
    }
}
