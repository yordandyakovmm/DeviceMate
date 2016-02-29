using DeviceMate.Core.Services;
using DeviceMate.Models.Domain;
using DeviceMate.Models.Entities;
using DeviceMate.Objects.Repositories;
using System.Data.Entity;
using System.Linq;

namespace DeviceMate.Services
{
    public class FilterService : IFilterService
    {


        #region Fields
        private readonly DeviceContext _context;
        private readonly AccessoryRepo _accessoryRepo;
        private readonly DeviceRepo _deviceRepo;
        private readonly UserRepo _userRepo;
        
        private readonly TownRepo _townRepo;
        private readonly OSRepo _osRepo;
        private readonly TeamRepo _teamRepo;
        private readonly ScreenSizeRepo _screenSizeRepo;
        private readonly ResolutionRepo _resolutionRepo;
        private readonly ResolutionWidthRepo _resolutionWidthRepo;
        private readonly ResolutionHeightRepo _resolutionHeightRepo;
        private readonly ColorRepo _colorRepo;
        private readonly DeviceTypeRepo _deviceTypeRepo;
        private readonly AccessoryDescriptionRepo _accessoryDescriptionRepo;
        private readonly AccessoryTypeRepo _accessoryTypeRepo;
        #endregion

        #region Constructor
        public FilterService()
        {
            _context = new DeviceContext();
            _accessoryRepo = new AccessoryRepo(_context);
            _deviceRepo = new DeviceRepo(_context);
            _userRepo = new UserRepo(_context);

            _townRepo = new TownRepo(_context);
            _osRepo = new OSRepo(_context);
            _teamRepo = new TeamRepo(_context);
            _screenSizeRepo = new ScreenSizeRepo(_context);
            _resolutionRepo = new ResolutionRepo(_context);
            _resolutionWidthRepo = new ResolutionWidthRepo(_context);
            _resolutionHeightRepo = new ResolutionHeightRepo(_context);
            _colorRepo = new ColorRepo(_context);
            _deviceTypeRepo = new DeviceTypeRepo(_context);
            _accessoryDescriptionRepo = new AccessoryDescriptionRepo(_context);
            _accessoryTypeRepo = new AccessoryTypeRepo(_context);
        }
        #endregion

        #region IFilterService methods

        public FilterOptions GetDeviceFilterOptions()
        {
            IQueryable<Device> devices = _deviceRepo.GetNoTracking()
                                                    .Include("DeviceType")
                                                    .Include("Color")
                                                    .Include("Model.Manufacturer.OSs")
                                                    .Include("Hold.Team")
                                                    .Include("Hold.Town")
                                                    .Include("ScreenSize")
                                                    .Include("Resolution.ResolutionHeightOption")
                                                    .Include("Resolution.ResolutionWidthOption"); 
            
            FilterOptions filterOptions = new FilterOptions();

            filterOptions.Cities = devices.Where(d => d.Hold.Town != null)
                                            .Select(d => d.Hold.Town)
                                            .Distinct()
                                            .Select(t => new LocationProxy(){ Id = t.TownId, Name = t.Name})
                                            .ToList();
            filterOptions.Colors = devices.Where(d => d.Color != null)
                                            .Select(d => d.Color)
                                            .Distinct()
                                            .Select(c => new ColorProxy() { Id = c.Id, Name = c.Name })
                                            .ToList();
            filterOptions.ScreenSize = devices.Where(d => d.ScreenSize != null)
                                                .Select(d => d.ScreenSize)
                                                .Distinct()
                                                .Select(sz => new ScreenSizeProxy() { Id = sz.Id, Size = sz.Size })
                                                .ToList();
            filterOptions.ScreenHeight = devices.Where(d => d.Resolution.ResolutionHeightOption != null)
                                                    .Select(d => d.Resolution.ResolutionHeightOption)
                                                    .Distinct()
                                                    .Select(h => new ResolutionDimention() { Id = h.Id, Dimention = h.Height })
                                                    .ToList();
            filterOptions.ScreenWidth = devices.Where(d => d.Resolution.ResolutionWidthOption != null)
                                                    .Select(d => d.Resolution.ResolutionWidthOption)
                                                    .Distinct()
                                                    .Select(w => new ResolutionDimention() { Id = w.Id, Dimention = w.Width })
                                                    .ToList();
            filterOptions.Teams = devices.Where(d => d.Hold.Team != null)
                                            .Select(d => d.Hold.Team)
                                            .Distinct()
                                            .Select(t => new TeamProxy() { Id = t.Id, Name = t.Name })
                                            .ToList();
            filterOptions.Platforms = devices.Where(d => d.Model.Manufacturer.OSs != null)
                                                .Select(d => d.Model.Manufacturer.OSs)
                                                .Distinct()
                                                .Select(o => new Platform() { Id = o.Id, Name = o.Name })
                                                .ToList();
            filterOptions.DeviceType = devices.Select(d => d.DeviceType)
                                                .Distinct()
                                                .Select(t => new DeviceTypeProxy() { Id = t.Id, Name = t.Name })
                                                .ToList();
                                                
            foreach (Platform platform in filterOptions.Platforms)
            {
                platform.Manufacturers = devices.Where(d => d.Model.Manufacturer.OsId == platform.Id)
                                                .Select(d => d.Model.Manufacturer)
                                                .Distinct()
                                                .Select(m => new ManufacturerProxy() { Id = m.Id, Name = m.Name })
                                                .ToList();

                foreach (ManufacturerProxy manufacturer in platform.Manufacturers)
                {
                    manufacturer.Models = devices.Where(d => d.Model.ManufacturerId == manufacturer.Id)
                                                .Select(d => d.Model)
                                                .Distinct()
                                                .Select(m => new ModelProxy() { Id = m.Id, Name = m.Name })
                                                .ToList();
                }
            }

            return filterOptions;
        }

        public FilterOptions GetAccessoryFilterOptions()
        {
            IQueryable<Accessory> accessories = _accessoryRepo.GetNoTracking()
                                                    .Include("AccessoryDescription")
                                                    .Include("AccessoryType")
                                                    .Include("OSs")
                                                    .Include("Color")
                                                    .Include("Hold.Team")
                                                    .Include("Hold.Town");

            FilterOptions filterOptions = new FilterOptions();

            filterOptions.Descriptions = accessories.Where(a => a.AccessoryDescription != null)
                                                    .Select(a => a.AccessoryDescription)
                                                    .Distinct()
                                                    .Select(d => new AccessoryDescriptionProxy() { Id = d.Id, Name = d.Description })
                                                    .ToList();
            filterOptions.Cities = accessories.Where(a => a.Hold.Town != null)
                                            .Select(a => a.Hold.Town)
                                            .Distinct()
                                            .Select(t => new LocationProxy() { Id = t.TownId, Name = t.Name })
                                            .ToList();
            filterOptions.Colors = accessories.Where(a => a.Color != null)
                                            .Select(a => a.Color)
                                            .Distinct()
                                            .Select(c => new ColorProxy() { Id = c.Id, Name = c.Name })
                                            .ToList();
            filterOptions.Teams = accessories.Where(a => a.Hold.Team != null)
                                            .Select(a => a.Hold.Team)
                                            .Distinct()
                                            .Select(t => new TeamProxy() { Id = t.Id, Name = t.Name })
                                            .ToList();
            filterOptions.Platforms = accessories.Where(a => a.OSs != null)
                                                .Select(a => a.OSs)
                                                .Distinct()
                                                .Select(o => new Platform() { Id = o.Id, Name = o.Name })
                                                .ToList();
            filterOptions.AccessoryType = accessories.Select(a => a.AccessoryType)
                                                    .Distinct()
                                                    .Select(t => new AccessoryTypeProxy() { Id = t.Id, Name = t.Name })
                                                    .ToList();

            return filterOptions;
        }

        public FilterOptions GetAllDeviceFilterOptions()
        {
            FilterOptions filterOptions = new FilterOptions();

            filterOptions.Cities = _townRepo.GetNoTracking()
                                            .Select(t => new LocationProxy(){ Id = t.TownId, Name = t.Name })
                                            .ToList();
            filterOptions.Colors = _colorRepo.GetNoTracking()
                                            .Select(c => new ColorProxy() { Id = c.Id, Name = c.Name })
                                            .ToList();
            filterOptions.Resolution = _resolutionRepo.GetNoTracking()
                                            .Select(r => new
                                            {
                                                Id = r.Id,
                                                WidthId = r.ResolutionWidthId,
                                                Width = r.ResolutionWidthOption.Width,
                                                HeightId = r.ResolutionHeightId,
                                                Height = r.ResolutionHeightOption.Height
                                            })
                                            .ToList()
                                            .Select(ar => new ResolutionProxy()
                                            {
                                                Id = ar.Id,
                                                Name = string.Format("{0}x{1}", ar.Width, ar.Height),
                                                Width = new ResolutionDimention() { Id = ar.WidthId, Dimention = ar.Width },
                                                Height = new ResolutionDimention() { Id = ar.HeightId, Dimention = ar.Height}
                                            })
                                            .ToList();
            filterOptions.ScreenWidth = _resolutionWidthRepo.GetNoTracking()
                                            .Select(rw => new
                                            {
                                                Id = rw.Id,
                                                Width = rw.Width
                                            })
                                            .ToList()
                                            .Select(w => new ResolutionDimention()
                                            {
                                                Id = w.Id,
                                                Dimention = w.Width
                                            })
                                            .ToList();
            filterOptions.ScreenHeight = _resolutionHeightRepo.GetNoTracking()
                                            .Select(rh => new
                                            {
                                                Id = rh.Id,
                                                Width = rh.Height
                                            })
                                            .ToList()
                                            .Select(h => new ResolutionDimention()
                                            {
                                                Id = h.Id,
                                                Dimention = h.Width
                                            })
                                            .ToList();
            filterOptions.ScreenSize = _screenSizeRepo.GetNoTracking()
                                            .Select(ss => new ScreenSizeProxy() { Id = ss.Id, Size = ss.Size })
                                            .ToList();
            filterOptions.Teams = _teamRepo.GetNoTracking()
                                            .Select(t => new TeamProxy() { Id = t.Id, Name = t.Name })
                                            .ToList();
            filterOptions.DeviceType = _deviceTypeRepo.GetNoTracking()
                                            .Select(dt => new DeviceTypeProxy() { Id = dt.Id, Name = dt.Name })
                                            .ToList();
            filterOptions.Platforms = _osRepo.GetNoTracking()
                                            .Include("Accessories")
                                            .Include("Manufacturers.Models.Devices")
                                            .Select(o => new Platform()
                                            {
                                                Id = o.Id,
                                                Name = o.Name,
                                                IsRemovable = (!o.Manufacturers.Any() && !o.Accessories.Any()),
                                                Manufacturers = o.Manufacturers.Select(m => new ManufacturerProxy()
                                                {
                                                    Id = m.Id,
                                                    Name = m.Name,
                                                    IsRemovable = !m.Models.Any(),
                                                    Models = m.Models.Select(mod => new ModelProxy()
                                                    {
                                                        Id = mod.Id,
                                                        Name = mod.Name,
                                                        IsRemovable = !mod.Devices.Any()

                                                    })
                                                    .ToList()
                                                })
                                                .ToList()
                                            })
                                            .ToList();

            return filterOptions;
        }

        public FilterOptions GetAllAccessoryFilterOptions()
        {
            FilterOptions filterOptions = new FilterOptions();

            filterOptions.Descriptions = _accessoryDescriptionRepo.GetNoTracking()
                                            .Include("Accessories")
                                            .Select(ad => new AccessoryDescriptionProxy()
                                            {
                                                Id = ad.Id,
                                                Name = ad.Description,
                                                IsRemovable = !ad.Accessories.Any()
                                            })
                                            .ToList();
            filterOptions.AccessoryType = _accessoryTypeRepo.GetNoTracking()
                                            .Include("Accessories")
                                            .Select(at => new AccessoryTypeProxy()
                                            {
                                                Id = at.Id,
                                                Name = at.Name,
                                                IsRemovable = !at.Accessories.Any()
                                            })
                                            .ToList();
            filterOptions.Cities = _townRepo.GetNoTracking()
                                            .Select(t => new LocationProxy() { Id = t.TownId, Name = t.Name })
                                            .ToList();
            filterOptions.Colors = _colorRepo.GetNoTracking()
                                            .Select(c => new ColorProxy() { Id = c.Id, Name = c.Name })
                                            .ToList();
            filterOptions.Teams = _teamRepo.GetNoTracking()
                                            .Select(t => new TeamProxy() { Id = t.Id, Name = t.Name })
                                            .ToList();
            filterOptions.Platforms = _osRepo.GetNoTracking()
                                            .Include("Accessories")
                                            .Include("Manufacturers")
                                            .Select(o => new Platform()
                                            {
                                                Id = o.Id,
                                                Name = o.Name,
                                                IsRemovable = (!o.Manufacturers.Any() && !o.Accessories.Any())
                                            })
                                            .ToList();

            return filterOptions;
        }

        public FilterOptions GetUserFilterOptions()
        {
            IQueryable<User> users = _userRepo.GetNoTracking()
                                                .Include("Team")
                                                .Include("Town");

            FilterOptions filterOptions = new FilterOptions();

            filterOptions.Teams = users.Where(u => u.Team != null)
                                            .Select(u => u.Team)
                                            .Distinct()
                                            .Select(t => new TeamProxy() { Id = t.Id, Name = t.Name })
                                            .ToList();

            filterOptions.Cities = users.Where(u => u.Town != null)
                                            .Select(u => u.Town)
                                            .Distinct()
                                            .Select(t => new LocationProxy() { Id = t.TownId, Name = t.Name })
                                            .ToList();

            return filterOptions;
        }

        #endregion
    }
}
