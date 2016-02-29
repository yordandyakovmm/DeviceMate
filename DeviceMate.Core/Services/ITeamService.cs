using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using System.Collections.Generic;

namespace DeviceMate.Core.Services
{
    public interface ITeamService
    {
        TeamProxyList GetByFilter(TeamFilter filter);

        TeamProxy GetById(int id);

        int GetIdByName(string name);

        void Delete(int id);

        bool Add(TeamProxy simpleTeam);

        bool Edit(TeamProxy simpleTeam);
    }
}
