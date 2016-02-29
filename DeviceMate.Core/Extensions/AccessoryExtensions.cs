using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceMate.Core.Extensions
{
    public static class AccessoryExtensions
    {
        public static AccessoryProxy ConvertToAccessoryProxy(this Accessory accessory, User holder = null)
        {
            if (accessory != null)
            {
                AccessoryProxy accessoryProxy = new AccessoryProxy()
                {
                    Id = accessory.Id,
                    Name = accessory.Number,
                    Info = accessory.SerialNumber,
                    DateTaken = accessory.Hold.HoldDate,

                    Type = new AccessoryTypeProxy()
                    {
                        Id = accessory.TypeId,
                        Name = accessory.AccessoryType.Name
                    },
                    Description = new AccessoryDescriptionProxy()
                    {
                        Id = accessory.AccessoryDescriptionId,
                        Name = accessory.AccessoryDescription.Description
                    },
                    Holder = holder != null ? accessory.Hold.ConvertToHoldProxy(holder) : null,
                    Platform = !accessory.OsId.HasValue ? null : new Platform()
                    {
                        Id = accessory.OsId.Value,
                        Name = accessory.OSs.Name
                    },
                    Color = !accessory.ColorId.HasValue ? null : new ColorProxy()
                    {
                        Id = accessory.ColorId.Value,
                        Name = accessory.Color.Name
                    }
                };
                return accessoryProxy;
            }
            else
            {
                return null;
            }
        }

        public static IList<AccessoryProxy> ConvertToAccessoryProxies(this IEnumerable<Accessory> accessories, IEnumerable<User> holders)
        {
            IList<AccessoryProxy> accessoryProxies = new List<AccessoryProxy>();
            
            foreach (Accessory accessory in accessories)
            {
                User holder = holders.Where(u => u.Email == accessory.Hold.Email).FirstOrDefault();
                AccessoryProxy accessoryProxy = accessory.ConvertToAccessoryProxy(holder);
                
                if (accessoryProxy != null)
                {
                    accessoryProxies.Add(accessoryProxy);
                }
            }

            return accessoryProxies;
        }

        public static Accessory ConvertToAccessory(this AccessoryProxy accessoryProxy)
        {
            if (accessoryProxy != null)
            {
                Accessory accessory = new Accessory()
                {
                    Id = accessoryProxy.Id,
                    Number = accessoryProxy.Name,
                    SerialNumber = accessoryProxy.Info,
                    AccessoryDescriptionId = accessoryProxy.Description.Id,
                    TypeId = accessoryProxy.Type.Id,
                    Hold = new Hold()
                    {
                        Id = accessoryProxy.Holder.Id,
                        Email = accessoryProxy.Holder.Email,
                        HoldDate = DateTime.Now,
                        IsBusy = accessoryProxy.Holder.IsBusy,
                        TeamId = accessoryProxy.Holder.Team.Id,
                        TownID = accessoryProxy.Holder.Location != null ? accessoryProxy.Holder.Location.Id : 0
                    },
                    OsId = accessoryProxy.Platform != null ? (int?)accessoryProxy.Platform.Id : null,
                    ColorId = accessoryProxy.Color != null ? (int?)accessoryProxy.Color.Id : null
                };

                accessory.AccessoryHoldsHistories.Add(new AccessoryHoldsHistory()
                {
                    Email = accessory.Hold.Email,
                    HoldDate = accessory.Hold.HoldDate,
                    IsBusy = accessory.Hold.IsBusy,
                    TeamId = accessory.Hold.TeamId,
                    TownId = accessory.Hold.TownID,
                });

                return accessory;
            }
            else
            {
                return null;
            }
        }

        public static void UpdateWithAccessoryProxy(this Accessory accessory, AccessoryProxy accessoryProxy)
        {
            if (accessoryProxy != null)
            {
                accessory.Number = accessoryProxy.Name;
                accessory.SerialNumber = accessoryProxy.Info;
                accessory.TypeId = accessoryProxy.Type.Id;
                accessory.AccessoryDescriptionId = accessoryProxy.Description.Id;

                accessory.ColorId = accessoryProxy.Color != null ? (int?)accessoryProxy.Color.Id : null;
                accessory.OsId = accessoryProxy.Platform != null ? (int?)accessoryProxy.Platform.Id : null;
            }
        }
    }
}
