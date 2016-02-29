using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Core.Extensions
{
    public static class DeviceExtensions
    {
        public static DeviceProxy ConvertToDeviceProxy(this Device device, User holder = null)
        {
            if (device != null)
            {
                DeviceProxy deviceProxy = new DeviceProxy()
                {
                    Id = device.Id,
                    Name = device.Name,
                    ModelName = device.Model.Name,
                    ManufacturerName = device.Model.Manufacturer.Name,
                    Info = device.SerialNumber,
                    DeviceNumber = device.Number,
                    DateTaken = device.Hold.HoldDate,

                    Holder = holder != null ? device.Hold.ConvertToHoldProxy(holder) : null,

                    Model = new ModelProxy()
                    {
                        Id = device.ModelId,
                        Name = device.Model.Name
                    },
                    Manufacturer = new ManufacturerProxy()
                    {
                        Id = device.Model.Manufacturer.Id,
                        Name = device.Model.Manufacturer.Name
                    },
                    Color = device.ColorId == null ? null : new ColorProxy()
                    {
                        Id = device.ColorId.Value,
                        Name = device.Color.Name
                    },
                    ScreenSize = !device.ScreenSizeId.HasValue ? null : new ScreenSizeProxy()
                    {
                        Id = device.ScreenSize.Id,
                        Size = device.ScreenSize.Size
                    },
                    Resolution = device.ResolutionId == null ? null : new ResolutionProxy()
                    {
                        Id = device.Resolution.Id,

                        Width = new ResolutionDimention()
                        {
                            Id = device.Resolution.ResolutionWidthId,
                            Dimention = device.Resolution.ResolutionWidthOption.Width
                        },
                        Height = new ResolutionDimention()
                        {
                            Id = device.Resolution.ResolutionHeightId,
                            Dimention = device.Resolution.ResolutionHeightOption.Height
                        }
                    },
                    Type = new DeviceTypeProxy()
                    {
                        Id = device.DeviceTypeId,
                        Name = device.DeviceType.Name
                    },
                    Platform = new Platform()
                    {
                        Id = device.Model.Manufacturer.OsId,
                        Name = device.Model.Manufacturer.OSs.Name,
                        Version = device.OsVersion
                    }
                };

                return deviceProxy;
            }
            else
            {
                return null;
            }
        }

        public static IList<DeviceProxy> ConvertToDeviceProxies(this IEnumerable<Device> devices, IEnumerable<User> holders = null)
        {
            if (devices != null)
            {
                IList<DeviceProxy> deviceProxies = new List<DeviceProxy>();

                foreach (Device device in devices)
                {
                    User holder = holders != null ? holders.Where(u => u.Email == device.Hold.Email).FirstOrDefault() : null;
                    DeviceProxy deviceProxy = device.ConvertToDeviceProxy(holder);
                    if (deviceProxy != null)
                    {
                        deviceProxies.Add(deviceProxy);
                    }
                }

                return deviceProxies;
            }
            else
            {
                return null;
            }
        }

        public static Device ConvertToDevice(this DeviceProxy deviceProxy)
        {
            if (deviceProxy != null)
            {
                Device device = new Device()
                {
                    Id = deviceProxy.Id,
                    Number = deviceProxy.DeviceNumber,
                    Name = deviceProxy.Name,
                    SerialNumber = deviceProxy.Info,
                    OsVersion = deviceProxy.Platform.Version,
                    HoldId = deviceProxy.Holder.Id != 0 ? deviceProxy.Holder.Id : 0,
                    Hold = new Hold()
                    {
                        Id = deviceProxy.Holder.Id,
                        Email = deviceProxy.Holder.Email,
                        HoldDate = DateTime.Now,
                        IsBusy = deviceProxy.Holder.IsBusy,
                        TeamId = deviceProxy.Holder.Team.Id,
                        TownID = deviceProxy.Holder.Location != null ? deviceProxy.Holder.Location.Id : 0
                    },
                    ModelId = deviceProxy.Model.Id,
                    DeviceTypeId = deviceProxy.Type.Id,
                    ResolutionId = deviceProxy.Resolution != null ? (int?)deviceProxy.Resolution.Id : null,
                    ScreenSizeId = deviceProxy.ScreenSize != null ? (int?)deviceProxy.ScreenSize.Id : null,
                    ColorId = deviceProxy.Color != null ? (int?)deviceProxy.Color.Id : null
                };

                device.DeviceHoldsHistories.Add(new DeviceHoldsHistory()
                {
                    Email = device.Hold.Email,
                    HoldDate = device.Hold.HoldDate,
                    IsBusy = device.Hold.IsBusy,
                    TeamId = device.Hold.TeamId,
                    TownID = device.Hold.TownID,
                });

                return device;
            }
            else
            {
                return null;
            }
        }

        public static void UpdateWithDeviceProxy(this Device device, DeviceProxy deviceProxy)
        {
            if (deviceProxy != null)
            {
                device.Name = deviceProxy.Name;
                device.SerialNumber = deviceProxy.Info;
                device.Number = deviceProxy.DeviceNumber;
                device.DeviceTypeId = deviceProxy.Type.Id;
                device.ModelId = deviceProxy.Model.Id;

                device.ScreenSizeId = deviceProxy.ScreenSize != null ? (int?)deviceProxy.ScreenSize.Id : null;
                device.ResolutionId = deviceProxy.Resolution != null ? (int?)deviceProxy.Resolution.Id : null;
                device.ColorId = deviceProxy.Color != null ? (int?)deviceProxy.Color.Id : null;

                if (deviceProxy.Platform != null)
                {
                    device.OsVersion = deviceProxy.Platform.Version;
                }
            }
        }
    }
}
