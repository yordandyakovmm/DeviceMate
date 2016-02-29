(function() {
  'use strict';

  DeviceMateApp.factory('teamModel', [function() {

    function Team(rawObj) {
      this.Id = rawObj.Id;
      this.Name = rawObj.Name;
      this.Users = rawObj.Users;
    }

    return Team;
  }]);
})();

