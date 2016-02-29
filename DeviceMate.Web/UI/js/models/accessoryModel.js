(function() {
  'use strict';

  DeviceMateApp.factory('accessoryModel', ['configSvc', function(configSvc) {
    function Accessory(rawObj) {
      this.itemID = rawObj.Id;
      this.itemNumber = rawObj.AccessoryNumber;
      this.name = rawObj.Name;
      this.model = rawObj.ModelName;
      this.manufacturer = rawObj.ManufacturerName;
      this.info = rawObj.Info;
      this.dateTaken = rawObj.DateTaken;

      if (rawObj.Description) {
        this.description = rawObj.Description.Name;
      }

      if (rawObj.Platform) {
        this.osType = rawObj.Platform.Name;
        //this.osVersion = rawObj.Platform.Version;
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

        // add a property that will set appripriate class for styling DOM elements (item identifier column) depending on the item availability
        this.isBusyClass = rawObj.Holder.IsBusy ? 'color-notavailable' : 'color-available';
      }
    }

    return Accessory;
  }]);
})();
