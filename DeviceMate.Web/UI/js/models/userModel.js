(function() {
  'use strict';

  DeviceMateApp.factory('userModel', [function() {
    function User(rawObj) {
      if (!rawObj.Team) {
        rawObj.Team = {};
      }

      this.Email = rawObj.Email;
      this.Skype = rawObj.Skype;
      this.PictureUrl = rawObj.PictureUrl;
      this.Position = rawObj.Position;
      this.IsAdmin = rawObj.IsAdmin;
      this.IsDeleted = rawObj.IsDeleted;
      this.Location = rawObj.Location;
      this.Message = rawObj.Message;
      this.Id = rawObj.Id;
      this.Name = rawObj.Name;
      this.TeamName = rawObj.Team.Name;
      this.TeamId = rawObj.Team.Id;
    }

    return User;
  }]);
})();
