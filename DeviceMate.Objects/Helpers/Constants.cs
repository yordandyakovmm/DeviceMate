using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMate.Objects.Helpers
{
    public static class Constants
    {
        public static class Application
        {
            public const string Name = "DeviceMate";
        }

        public static class Status
        {
            public const string Active = "Active";
            public const string Inactive = "Inactive";
        }

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

        public static class Teams
        {
            public const int DefaultTeamId = 1;

            public const int AcculynxTeamId = 2;
            public const int AgileFrameworksTeamId = 3;
            public const int AmcomTeamId = 4;
            public const int AndroidTeamId = 5;
            public const int FarmChemTeamId = 6;
            public const int MyMedsTeamId = 7;
            public const int MyMedsV2TeamId = 8;
            public const int PickPointzTeamId = 9;
            public const int StoryWorksTeamId = 10;
            public const int W3ITeamId = 11;
        }
    }
}
