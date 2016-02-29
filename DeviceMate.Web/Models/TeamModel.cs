using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Objects.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Web.Models
{
    public class TeamModel : BaseModel<TeamRepo, int?>, IUserModel
    {

        // IUserModel
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }

        public TeamProxy Team { get; set; }

        public IEnumerable<Team> Teams { get; set; }

        public override void Init(int? id)
        {
            if (id.HasValue)
            {
                Team t = Repo.GetById(id.Value);
                this.Team = new TeamProxy()
                {
                    Id = t.Id,
                    Name = t.Name
                };
            }
            else
            {
                this.Team = new TeamProxy()
                {
                    Id = null,
                    Name = ""
                };
            }
            Teams = Repo.GetAll();
        }

        public bool Delete(int id)
        {
            return this.Repo.Delete(id) > 0;
        }

        public void Edit()
        {
            Team team = this.Repo.GetById(this.Team.Id.Value);
            team.Name = this.Team.Name;
            this.Repo.SaveChanges();
        }

        public void Add()
        {
            Team team = new Team
            {
                Name = this.Team.Name
            };
            this.Repo.Add(team);
            this.Repo.SaveChanges();
        }

        public bool IsTeamNameTaken()
        {
            var allTeams = this.Repo.GetAll();
            return allTeams.Any(t => t.Name.ToLower() == Team.Name.ToLower().Trim());
        }
    }
}



