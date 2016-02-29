using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Helpers;

namespace DeviceMate.Objects.Repositories
{
    public class DeviceRepo : BaseRepo<Device>, IHoldable
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public DeviceRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        #region Public methods

        public void Save(Device device)
        {
            Device d = GetById(device.Id);
            d.Name = device.Name;
            d.Number = device.Number;
            d.DeviceTypeId = device.DeviceTypeId;
            d.OsVersion = device.OsVersion;
            d.ColorId = device.ColorId;
            d.ModelId = device.ModelId;
            d.SerialNumber = device.SerialNumber;
            d.ScreenSizeId = device.ScreenSizeId;
            d.ResolutionId = device.ResolutionId;
            if (device.HoldId != 0)
            {
                d.Hold.TownID = device.Hold.TownID;
            }
            Context.SaveChanges();
        }

        public int Delete(int id)
        {
            Device device = this.Context.Devices.First(d => d.Id == id);
            this.Context.Devices.Remove(device);
            int rowsAffected = this.Context.SaveChanges();
            return rowsAffected;
        }

        public Device GetByNumber(string number)
        {
            return Context.Devices.Where<Device>(d => d.Number == number).FirstOrDefault();
        }

        public Device GetById(int id)
        {
            return Context.Devices.Where<Device>(d => d.Id == id).FirstOrDefault();
        }

        public HashSet<string> GetNumbers()
        {
            HashSet<string> numbers = new HashSet<string>(this.Context.Devices.Select(a => a.Number.ToLower()));
            return numbers;
        }

        public Hold GetHoldByNumber(string number)
        {
            Device device = this.Context.Devices.Include("Hold").First(a => a.Number == number);
            return device.Hold;
        }

        public int GetHoldIdById(int id)
        {
            int holdId = this.Context.Devices.Include("Hold").First(d => d.Id == id).Hold.Id;
            return holdId;
        }

        public string GetNumberByHoldId(int id)
        {
            string number = this.Context.Devices.First(a => a.HoldId == id).Number;
            return number;
        }

        public string GetNumberById(int id)
        {
            string number = this.Context.Devices.First(a => a.Id == id).Number;
            return number;
        }

        public int SearchCount(string number, int? type, IEnumerable<int> modelId, int? manufacturerId,
            int? osId, string name, int? colorId, int? teamId, string serialNumber, string email, string OSVersion,
            int? availableID, int? townId, int? screenSizeId, int? resolutionWidthId, int? resolutionHeightId)
        {
            return SearchQuery(number, type, modelId, manufacturerId, osId, name, colorId, teamId, serialNumber, email, OSVersion, availableID, townId, screenSizeId, resolutionWidthId, resolutionHeightId).Count();
        }

        public List<Device> Search(string number, int? type, IEnumerable<int> modelId, int? manufacturerId,
            int? osId, string name, int? colorId, int? teamId, string serialNumber, string email, string OSVersion, int? availableID, int? townId,
            int? screenSizeId, int? resolutionWidthId, int? resolutionHeightId,
            string Expression = null, string Column = null, int Direction = 0, int Page = 1, int PageSize = int.MaxValue)
        {
            var q = SearchQuery(number, type, modelId, manufacturerId, osId, name, colorId, teamId, serialNumber,
                email, OSVersion, availableID, townId, screenSizeId, resolutionWidthId, resolutionHeightId);

            var query = q;
            if (PageSize != 0)
            {
                q = q.Order(Expression, Column, Direction, () => query.OrderByDescending(a => a.Hold.HoldDate));
                q = q.Paginate(Page, PageSize);
            }

            return q.ToList();
        }

        private IQueryable<Device> SearchQuery(string number, int? type, IEnumerable<int> modelId, int? manufacturerId,
            int? osId, string name, int? colorId, int? teamId, string serialNumber, string email, string OSVersion,
            int? availableID, int? townId, int? screenSizeId, int? resolutionWidthId, int? resolutionHeightId)
        {
            IQueryable<Device> devices = this.Context.Devices
                   .Include("Color")
                   .Include("Hold")
                   .Include("DeviceType")
                   .Include("Model")
                   .Include("Model.Manufacturer")
                   .Include("Model.Manufacturer.OSs");


            if (!string.IsNullOrWhiteSpace(number))
            {
                devices = devices.Where(d => d.Number.ToLower() == number.ToLower().Trim());
            }
            if (modelId != null)
            {
                devices = devices.Where(d => modelId.Contains(d.ModelId));
            }
            if (manufacturerId.HasValue)
            {
                devices = devices.Where(d => d.Model.Manufacturer.Id == manufacturerId.Value);
            }
            if (osId.HasValue)
            {
                devices = devices.Where(d => d.Model.Manufacturer.OSs.Id == osId.Value);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                devices = devices.Where(d => d.Name.ToLower().Contains(name.ToLower().Trim()));
            }
            if (colorId.HasValue)
            {
                devices = devices.Where(d => d.ColorId == colorId.Value);
            }
            if (teamId.HasValue)
            {
                devices = devices.Where(d => d.Hold.TeamId == teamId.Value);
            }
            if (townId.HasValue && townId > 0)
            {
                devices = devices.Where(d => d.Hold.TownID == townId.Value);
            }
            if (!string.IsNullOrWhiteSpace(serialNumber))
            {
                devices = devices.Where(d => d.SerialNumber.ToLower() == serialNumber.ToLower().Trim());
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                email = email.ToLower().Trim();
                string[] tokens = email.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 1)
                {
                    email = string.Join(".", tokens);
                }
                devices = devices.Where(d => d.Hold.Email.ToLower().Contains(email));
            }
            if (!string.IsNullOrWhiteSpace(OSVersion))
            {
                devices = devices.Where(d => d.OsVersion.StartsWith(OSVersion.Trim()));
            }
            if (type.HasValue)
            {
                devices = devices.Where(d => d.DeviceType.Id == type.Value);
            }
            if (availableID != null)
            {
                devices = devices.Where(d => d.Hold.IsBusy == (availableID != 1));
            }
            if (screenSizeId != null)
            {
                devices = devices.Where(d => d.ScreenSizeId == screenSizeId.Value);
            }
            if (resolutionWidthId != null)
            {
                devices = devices.Where(d => d.Resolution.ResolutionWidthId == resolutionWidthId.Value);
            }
            if (resolutionHeightId != null)
            {
                devices = devices.Where(d => d.Resolution.ResolutionHeightId == resolutionHeightId.Value);
            }
            return devices;
        }

        public override int GetAllCount()
        {
            return GetAllQuery().Count();
        }

        public IEnumerable<Device> GetAll(int page = 1, int pageSize = int.MaxValue)
        {
            return GetAllQuery()
                .OrderByDescending(a => a.Hold.HoldDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
        }

        public IEnumerable<Device> GetAllJson()
        {
            return GetAllQuery()
                .OrderByDescending(a => a.Hold.HoldDate);
        }

        private IQueryable<Device> GetAllQuery()
        {
            return this.Context.Devices
                .Include("Color")
                .Include("Hold")
                .Include("Model");
        }
        #endregion  
    }
}
