using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using System.Collections.Generic;

namespace DeviceMate.Core.Services
{
    public interface IAccessoryService
    {
        AccessoryProxyList GetByFilter(AccessoryFilter filter);

        AccessoryProxy GetById(int id);

        void SubmitToHolder(int accessoryId, int userId, int teamId, bool isBusy, enTown town);

        void Remove(int id);

        bool Add(AccessoryProxy simpleAccessory);

        bool Edit(AccessoryProxy simpleAccessory);
    }
}
