using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using System.Data.Entity.Infrastructure;

namespace DeviceMate.Objects.Repositories
{
    public class TownRepo : BaseRepo<Town>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public TownRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        #region Public methods

        public void EditTown(Town town)
        {
            this.Context.Towns.Attach(town);

            DbEntityEntry<Town> entry = Context.Entry(town);
            entry.Property(e => e.Name).IsModified = true;

            this.Context.SaveChanges();
        }

        public int Delete(int id)
        {
            Town town = this.Context.Towns.First(h => h.TownId == id);
            this.Context.Towns.Remove(town);
            int rowsAffected = this.Context.SaveChanges();
            return rowsAffected;
        }

        public Town GetById(int id)
        {
            Town town = this.Context.Towns.First(h => h.TownId == id);
            return town;
        }

        public IEnumerable<Town> GetAll()
        {
            return this.Context.Towns.ToList();            
        }

        #endregion        
    }
}
