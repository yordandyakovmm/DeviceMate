(function() {
  'use strict';

  DeviceMateApp.factory('deviceModel', ['configSvc', '$sce', function(configSvc, $sce) {
    function Device(rawObj) {
      this.itemID = rawObj.Id;
      this.itemNumber = rawObj.DeviceNumber;
      this.name = rawObj.Name;
      this.model = rawObj.ModelName;
      this.manufacturer = rawObj.ManufacturerName;
      this.info = rawObj.Info;
      this.dateTaken = rawObj.DateTaken;

      if (rawObj.Platform) {
        this.osType = rawObj.Platform.Name;
        this.osVersion = rawObj.Platform.Version;
      }

      if (rawObj.Type) {
        this.type = rawObj.Type.Name;
      }

      if (rawObj.Color) {
        this.color = rawObj.Color.Name;
        this.colorHex = configSvc.getHexValue(this.color);
      }

      if (rawObj.Holder) {
        this.location = rawObj.Holder.Location.Name;
        this.locationID = rawObj.Holder.Location.Id;
        this.email = rawObj.Holder.Email;
        this.contactSkype = rawObj.Holder.Skype;
        this.holderName = rawObj.Holder.FullName;
        this.team = rawObj.Holder.Team.Name;
        this.teamID = rawObj.Holder.Team.Id;
        this.holderProfileImage = rawObj.Holder.ImagePath;

        // add a property that will set appripriate class for styling DOM elements (device identifier column) depending on the device availability
        this.isBusyClass = rawObj.Holder.IsBusy ? 'color-notavailable' : 'color-available';
      }

      if (rawObj.ScreenSize) {
        this.screenSize = rawObj.ScreenSize.Name;
      }

      if (rawObj.Resolution) {
        this.height = rawObj.Resolution.Height.Dimention;
        this.width = rawObj.Resolution.Width.Dimention;
      }
    }

    return Device;
  }]);
})();
