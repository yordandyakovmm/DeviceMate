using DeviceMate.Models.Entities;
using DeviceMate.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Repositories
{
    public class AccessoryRepo : BaseRepo<Accessory>, IHoldable
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public AccessoryRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        #region Public methods

        public override int GetAllCount()
        {
            return GetAllQuery().Count();
        }

        public IEnumerable<Accessory> GetAll(int page = 1, int pageSize = int.MaxValue)
        {
            return GetAllQuery()
                .OrderByDescending(a => a.Hold.HoldDate)
                .Skip(pageSize*(page - 1))
                .Take(pageSize);
        }

        public IEnumerable<Accessory> GetAllJson()
        {
            return GetAllQuery()
                .OrderByDescending(a => a.Hold.HoldDate);
                
        }

        private IQueryable<Accessory> GetAllQuery()
        {
            return this.Context.Accessories
                .Include("Hold")
                .Include("AccessoryDescription")
                .Include("AccessoryType");
        }

        public int SearchCount(string number, string serialNumber, int? typeId,
            int? descriptionId, string email, int? teamId, int? townId, int? osId, int? colorId)
        {
            return SearchQuery(number, serialNumber, typeId, descriptionId, email, teamId, townId, osId, colorId).Count();
        }

        public IEnumerable<Accessory> Search(string number, string serialNumber, int? typeId,
            int? descriptionId, string email, int? teamId,
            int? townId, int? osId, int? colorId,
            string Expression = null, string Column = null, int Direction = 0, int Page = 1, int PageSize = int.MaxValue)
        {
            var q = SearchQuery(number, serialNumber, typeId, descriptionId, email, teamId, townId, osId, colorId);

            var query = q;

            q = q.Order(Expression, Column, Direction, () => query.OrderByDescending(a => a.Hold.HoldDate));
            if (PageSize != 0)
            {
                q = q.Paginate(Page, PageSize);
            }

            return q.ToList();
        }

        private IQueryable<Accessory> SearchQuery(string number, string serialNumber, int? typeId,
            int? descriptionId, string email, int? teamId, int? townId, int? osId, int? colorId)
        {
            IQueryable<Accessory> accessories = this.Context.Accessories.Include("Hold");

            if (!string.IsNullOrWhiteSpace(number))
            {
                accessories = accessories.Where(a => a.Number.ToLower() == number.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(serialNumber))
            {
                accessories = accessories.Where(a => a.SerialNumber.ToLower() == serialNumber.ToLower());
            }
            if (typeId.HasValue)
            {
                accessories = accessories.Where(a => a.TypeId == typeId.Value);
            }
            if (descriptionId.HasValue)
            {
                accessories = accessories.Where(a => a.AccessoryDescriptionId == descriptionId.Value);
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                email = email.ToLower().Trim();
                string[] tokens = email.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 1)
                {
                    email = string.Join(".", tokens);
                }
                accessories = accessories.Where(d => d.Hold.Email.ToLower().Contains(email));
            }
            if (teamId.HasValue)
            {
                accessories = accessories.Where(a => a.Hold.TeamId == teamId.Value);
            }
            if (townId.HasValue)
            {
                accessories = accessories.Where(a => a.Hold.TownID == townId.Value);
            }
            if (osId.HasValue)
            {
                accessories = accessories.Where(a => a.OsId == osId.Value);
            }
            if (colorId.HasValue)
            {
                accessories = accessories.Where(a => a.ColorId == colorId.Value);
            }

            return accessories;
        }

        public Accessory GetById(int id)
        {
            return this.Context.Accessories.Where<Accessory>(d => d.Id == id).FirstOrDefault();
        }

        public void Edit(Accessory accessory)
        {
            Accessory oldAccessory = this.Context.Set<Accessory>().Find(accessory.Id);

            oldAccessory.Number = accessory.Number;
            oldAccessory.TypeId = accessory.TypeId;
            oldAccessory.SerialNumber = accessory.SerialNumber;
            oldAccessory.AccessoryDescriptionId = accessory.AccessoryDescriptionId;

            this.Context.SaveChanges();
        }

        public int Delete(int id)
        {
            Accessory accessory = this.Context.Accessories.First(a => a.Id == id);
            this.Context.Accessories.Remove(accessory);
            int rowsAffected = this.Context.SaveChanges();
            return rowsAffected;
        }

        public HashSet<string> GetNumbers()
        {
            HashSet<string> numbers = new HashSet<string>(this.Context.Accessories.Select(a => a.Number.ToLower()));
            return numbers;
        }

        public Hold GetHoldByNumber(string number)
        {
            Accessory accessory = this.Context.Accessories.Include("Hold").First(a => a.Number == number);
            return accessory.Hold;
        }

        public string GetNumberByHoldId(int id)
        {
            string number = this.Context.Accessories.First(a => a.AccessoryHoldId == id).Number;
            return number;
        }

        public string GetNumberById(int id)
        {
            string number = this.Context.Accessories.First(a => a.Id == id).Number;
            return number;
        }

        public int GetHoldIdById(int id)
        {
            int holdId = this.Context.Accessories.Include("Hold").First(a => a.Id == id).Hold.Id;
            return holdId;
        }

        #endregion  
    }
}