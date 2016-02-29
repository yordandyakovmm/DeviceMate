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
    public class TeamService : ITeamService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly TeamRepo _teamRepo;
        private readonly UserRepo _userRepo;
        private readonly DeviceContext _context;

        #endregion

        #region Constructor

        public TeamService(IUserService userService)
        {
            _context = new DeviceContext();
            _teamRepo = new TeamRepo(_context);
            _userRepo = new UserRepo(_context);
            _userService = userService;
        }

        #endregion

        #region ITeamService methods

        public TeamProxyList GetByFilter(TeamFilter filter)
        {
            IQueryable<Team> teamQuery = _teamRepo.GetNoTracking()
                                                .Include("Holds.Town")
                                                .Include("Holds.Devices");

            if (filter.TownId.HasValue)
            {
                teamQuery = teamQuery.Where(t => t.Holds.Any(h => h.TownID == filter.TownId));
            }

            if (filter.Keywords != null && filter.Keywords.Length > 0)
            {
                foreach (string keyword in filter.Keywords)
                {
                    teamQuery = teamQuery.Where(t => t.Name.ToLower().Contains(keyword));
                }
            }

            if (filter.Sort != null && filter.Sort.Count() > 0)
            {
                IList<string> sortExpression = new List<string>();
                foreach (KeyValuePair<enSortColumn, enSortOrder> sortItem in filter.Sort)
                {
                    string teamProperty = GetSortColumn(sortItem.Key);

                    if (!string.IsNullOrEmpty(teamProperty))
                    {
                        sortExpression.Add(string.Format("{0} {1}", teamProperty, sortItem.Value.ToString().ToUpper()));
                    }
                }

                if (sortExpression.Count > 0)
                {
                    teamQuery = teamQuery.OrderBy(string.Join(", ", sortExpression));
                }
            }

            IEnumerable<Team> teams = teamQuery.AsEnumerable();

            TeamProxyList teamsList = new TeamProxyList();

            #region Teams paging
            if (filter.Offset.HasValue)
            {
                int maxItems = filter.Limit.HasValue ?
                                filter.Limit.Value : Common.DefaultPageSize;

                teams = PagingHelper<Team>.GetCurrentPage(teams, teamsList, filter.Offset.Value, maxItems);
            }
            else
            {
                teamsList.TotalItems =
                    teamsList.ItemsPerPage =
                    teams.Count();
            }
            #endregion

            IList<User> holders = new List<User>();

            if (filter.GetUsers)
            {
                IList<string> holdEmails = teams.SelectMany(t => t.Holds.Select(h => h.Email)).Distinct().ToList();
                holders = _userService.GetByEmails(holdEmails);
            }

            teamsList.Teams = teams.ConvertToTeamProxies(holders);

            return teamsList;
        }

        public TeamProxy GetById(int id)
        {
            Team team = _teamRepo.GetById(id);
            IList<string> userEmails = team.Holds.Select(h => h.Email).ToList();
            IEnumerable<User> users = _userService.GetByEmails(userEmails);
            return team.ConvertToTeamProxy(users);
        }

        public int GetIdByName(string name)
        {
            Team team = _teamRepo.GetNoTracking(t => t.Name == name).FirstOrDefault();
            
            if (team != null)
            {
                return team.Id;
            }
            else
            {
                return 0;
            }
        }

        public void Delete(int id)
        {
            if (id == Common.DefaultTeamId)
            {
                throw new ArgumentException(string.Format("The team with ID {0} cannot be deleted.", id));
            }

            Team team = _teamRepo.Get(t => t.Id == id)
                                    .Include("Holds")
                                    .FirstOrDefault();

            if (team != null)
            {
                foreach (Hold hold in team.Holds)
                {
                    hold.TeamId = Common.DefaultTeamId;
                }

                _teamRepo.DeleteWithoutSave(team);
                _teamRepo.SaveChanges();
            }
        }

        public bool Add(TeamProxy simpleTeam)
        {
            Team team = simpleTeam.ConvertToTeam();

            if (team != null)
            {
                _teamRepo.Add(team);
                _teamRepo.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Edit(TeamProxy simpleTeam)
        {
            Team team = _teamRepo.GetById(simpleTeam.Id);
            
            if (team != null)
            {
                team.UpdateWithTeamProxy(simpleTeam);
                _teamRepo.Update(team);
                _teamRepo.SaveChanges();
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
            string teamProperty;
            switch (column)
            {
                case enSortColumn.Name:
                    teamProperty = "Name";
                    break;
                default:
                    teamProperty = string.Empty;
                    break;
            }
            return teamProperty;
        }
        #endregion
    }
}
