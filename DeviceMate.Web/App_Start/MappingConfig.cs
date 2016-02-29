using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Proxies;
using DeviceMate.Web.Models;

namespace DeviceMate.Web
{
    internal static class MappingConfig
    {
        internal static void RegisterMappings()
        {
            // Load map entity -> proxy
            // Save map proxy -> entity            

            Mapper.CreateMap<Color, NamedFeatureProxy>();
            Mapper.CreateMap<NamedFeatureProxy, Color>();

            Mapper.CreateMap<DeviceType, NamedFeatureProxy>();
            Mapper.CreateMap<NamedFeatureProxy, DeviceType>();

            Mapper.CreateMap<Hold, HoldProxy>();
            Mapper.CreateMap<HoldProxy, Hold>();

            Mapper.CreateMap<Device, DeviceProxy>()
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.OSVersion, opt => opt.MapFrom(src => src.OsVersion))
                .ForMember(dest => dest.DeviceTypeId, opt => opt.MapFrom(src => src.DeviceTypeId))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
                .ForMember(dest => dest.HoldId, opt => opt.MapFrom(src => src.HoldId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.ScreenSizeId, opt => opt.MapFrom(src => src.ScreenSizeId))
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.SerialNumber));

            Mapper.CreateMap<DeviceProxy, Device>()
                .AfterMap((src, dest) =>
                {
                    if (dest.Hold == null) dest.Hold = new Hold();
                });

            Mapper.CreateMap<AccessoryDescription, AccessoryDescriptionProxy>();
            Mapper.CreateMap<AccessoryDescriptionProxy, AccessoryDescription>();

            Mapper.CreateMap<AccessoryType, AccessoryTypeProxy>();
            Mapper.CreateMap<AccessoryTypeProxy, AccessoryType>();

            Mapper.CreateMap<Accessory, AccessoryProxy>()
                .ForMember(dest => dest.AccessoryTypeId, opt => opt.MapFrom(src => src.TypeId));
            Mapper.CreateMap<AccessoryProxy, Accessory>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.AccessoryTypeId));
        }
    }
}