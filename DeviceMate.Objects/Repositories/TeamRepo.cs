using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Entities;

namespace DeviceMate.Objects.Repositories
{
    public class TeamRepo : BaseRepo<Team>
    {
        #region Construction
        [Microsoft.Practices.Unity.InjectionConstructor]
        public TeamRepo(DeviceContext context)
            : base(context)
        {
        }
        #endregion

        public virtual IEnumerable<Team> GetAll()
        {
            return Context.Teams.OrderBy(d => d.Name);
        }

        //public Team GetById(int id)
        //{
        //    return Context.Teams.Where<Team>(d => d.Id == id).FirstOrDefault();
        //}

        public int Delete(int id)
        {
            try
            {
                Team team = this.Context.Teams.First(d => d.Id == id);
                this.Context.Teams.Remove(team);
                int rowsAffected = this.Context.SaveChanges();
                return rowsAffected;
            }
            catch (Exception) { return 0; }
        }


    }
}
