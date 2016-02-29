using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Models.Enums;
using System.Collections.Generic;

namespace DeviceMate.Core.Services
{
    public interface IDeviceService
    {
        DeviceProxyList GetByFilter(DeviceFilter filter);

        DeviceProxy GetById(int id);

        void SubmitToHolder(int deviceId, int userId, int teamId, bool isBusy, enTown town);

        void Delete(int id);

        bool Add(DeviceProxy simpleDevice);

        bool Edit(DeviceProxy simpleDevice);
    }
}