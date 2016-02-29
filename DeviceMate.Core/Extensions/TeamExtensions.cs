using DeviceMate.Core.Helpers;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Core.Extensions
{
    public static class TeamExtensions
    {
        public static TeamProxy ConvertToTeamProxy(this Team team, IEnumerable<User> users)
        {
            if (team != null)
            {
                TeamProxy teamProxy = new TeamProxy()
                {
                    Id = team.Id,
                    Name = team.Name,
                    Users = users.Select(u => new UserProxy()
                            {
                                Id = u.Id,
                                Name = u.Name,
                                Email = u.Email,
                                Skype = u.Skype,
                                PictureUrl = u.PictureUrl,
                                Position = u.Position,
                                IsDeleted = u.StatusId == (int)enUserStatus.Inactive,
                                Location = !u.TownId.HasValue ? null : new LocationProxy()
                                {
                                    Id = u.Town.TownId,
                                    Name = u.Town.Name
                                }
                            })
                .ToList()
                };

                return teamProxy;
            }
            else
            {
                return null;
            }
        }

        public static IList<TeamProxy> ConvertToTeamProxies(this IEnumerable<Team> teams, IEnumerable<User> users)
        {
            IList<TeamProxy> accessoriyProxies = new List<TeamProxy>();

            foreach (Team team in teams)
            {
                IList<string> holdEmails = team.Holds.Select(h => h.Email).Distinct().ToList();
                IEnumerable<User> teamUsers = users.Where(u => holdEmails.Contains(u.Email));
                TeamProxy teamProxy = team.ConvertToTeamProxy(teamUsers);

                if (teamProxy != null)
                {
                    accessoriyProxies.Add(teamProxy);
                }
            }

            return accessoriyProxies;
        }

        public static Team ConvertToTeam(this TeamProxy teamProxy)
        {
            if (teamProxy != null)
            {
                Team team = new Team()
                {
                    Id = teamProxy.Id,
                    Name = teamProxy.Name
                };

                return team;
            }
            else
            {
                return null;
            }
        }

        public static void UpdateWithTeamProxy(this Team team, TeamProxy teamProxy)
        {
            if (teamProxy != null)
            {
                team.Name = teamProxy.Name;
            }
        }

    }
}
