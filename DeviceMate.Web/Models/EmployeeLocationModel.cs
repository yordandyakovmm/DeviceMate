using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DeviceMate.Web.Models
{
    public class EmployeeLocationModel : IUserModel
    {
        public EmployeeLocationModel(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
        public string PremiseName { get; set; }
        public string LocationAsSvg { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsLocationFound { get; set; }

        #region IUserModel Members
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        #endregion

        #region Methods
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Error! Missing email address. Please contact the support.";
                return false;
            }

            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(Email))
            {
                ErrorMessage = "Error! The email address is not valid. Please contact the support.";
                return false;
            }

            return true;
        }

        public bool FindLocation(string mapsPath)
        {
            var mapsDirectory = new DirectoryInfo(mapsPath);
            FileInfo[] maps;

            try
            {
                maps = mapsDirectory.GetFiles("*.svg");
            }
            catch (UnauthorizedAccessException e)
            {
                ErrorMessage = "Permissions denied! Please contact the support.";
                return false;
            }

            foreach (var map in maps)
            {
                var locationAsSvg = GetLocationAsSvg(map.FullName);
                if (string.IsNullOrWhiteSpace(locationAsSvg))
                {
                    continue;
                }

                LocationAsSvg = locationAsSvg;
                PremiseName = map.Name.Substring(0, map.Name.IndexOf(".svg"));
                IsLocationFound = true;
                return true;
            }

            ErrorMessage = "Employee's location was not found. Please contact the support.";
            return false;
        }

        private string GetLocationAsSvg(string filePath)
        {
            const string xmlns = "http://www.w3.org/2000/svg";
            XNamespace xlink = "http://www.w3.org/1999/xlink";
            var xd = XDocument.Load(filePath);
            var root = xd.Root;
            if (root == null)
            {
                return null;
            }

            var targetLink = root.Descendants("{" + xmlns + "}a")
                .FirstOrDefault(x => x.Attribute(xlink + "href") != null && x.Attribute(xlink + "href").Value == string.Format("mailto:{0}", Email));
            if (targetLink == null)
            {
                return null;
            }

            var firstRect = targetLink.Descendants("{" + xmlns + "}rect").FirstOrDefault();
            if (firstRect == null)
            {
                return null;
            }

            firstRect.SetAttributeValue("fill", "#FFFF00");

            //var g = targetLink.Descendants("{" + xmlns + "}g").FirstOrDefault();
            //if (g == null)
            //{
            //    return null;
            //}

            //g.SetAttributeValue("fill", "#FF0000");

            return xd.ToString();
        }
        #endregion
    }
}