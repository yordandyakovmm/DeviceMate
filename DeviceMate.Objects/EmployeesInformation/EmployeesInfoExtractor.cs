using DeviceMate.Core;
using DeviceMate.Models.Enums;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SIO = System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace DeviceMate.Objects.EmployeesInformation
{
    public class EmployeesInfoExtractor
    {
        public EmployeesInfoExtractor(List<string> emails)
        {
            this.emails = emails;

            this.employees = new Dictionary<string, Employee>();
            this.resourceIdEmail = new Dictionary<string, string>();
        }

        public async Task<Dictionary<string, Employee>> Extract(bool updateAll)
        {
            if (this.emails == null || this.emails.Count != 0)
            {
                ExtractEmployeeInfo(updateAll);
                if (this.resourceIdEmail.Count > 0)
                {
                    await ExtractEmployeePictures(updateAll);
                }
            }

            return this.employees;
        }

        private async Task ExtractEmployeePictures(bool updateAll)
        {
            if (updateAll)
            {
                RemoveUnusedImages(this.resourceIdEmail);
            }

            ServiceAccountCredential credential = GetCredential();

            BaseClientService.Initializer initializer = new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "MentorMate-DeviceMate/2.0"
            };

            using (DriveService service = new DriveService(initializer))
            {
                foreach (string fileId in this.resourceIdEmail.Keys)
                {
                    string[] userImages = SIO.Directory.GetFiles(Config.ImagesDirectoryPath, string.Format("{0}*", fileId), SIO.SearchOption.TopDirectoryOnly);

                    if (userImages.Length == 1)
                    {
                        string fileName = SIO.Path.GetFileName(userImages.First());
                        
                        if (this.employees.ContainsKey(this.resourceIdEmail[fileId]))
                        {
                            this.employees[this.resourceIdEmail[fileId]].PictureUrl = string.Format(Common.UserImagePattern, fileName);
                        }
                    }
                    else
                    {
                        File image = GetFile(service, fileId);

                        if (image != null)
                        {
                            string fileExtension = image.FileExtension;

                            if ((string.IsNullOrEmpty(image.FileExtension) || image.FileExtension == "net") &&
                                image.MimeType.Contains("image/"))
                            {
                                fileExtension = image.MimeType.Replace("image/", string.Empty);
                            }

                            string fileName = string.Format("{0}.{1}", image.Id, fileExtension);
                            string filePath = string.Format("{0}\\{1}", Config.ImagesDirectoryPath, fileName);

                            if (this.employees.ContainsKey(this.resourceIdEmail[fileId]))
                            {
                                this.employees[this.resourceIdEmail[fileId]].PictureUrl = string.Format(Common.UserImagePattern, fileName);
                            }

                            using (SIO.FileStream fileStream = SIO.File.Create(filePath))
                            {
                                await DownloadFileAsync(image, fileStream, credential.Token.AccessToken);
                            }
                        }
                    }
                }
            }
        }

        public async static Task DownloadFileAsync(File file, SIO.FileStream fileStream, string accessToken)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(file.DownloadUrl));
                
                request.Headers.Add("Authorization: Bearer " + accessToken);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await response.GetResponseStream().CopyToAsync(fileStream);
                }
            }
            catch
            {
                return;
            }

        }

        private static File GetFile(DriveService service, string fileId)
        {
            try
            {
                return service.Files.Get(fileId).Execute();
            }
            catch
            {
                return null;
            }
        }

        private void ExtractEmployeeInfo(bool updateAll)
        {
            SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");

            ServiceAccountCredential credential = GetCredential();
            GDataRequestFactory requestFactory = new GDataRequestFactory("DeviceMate");
            requestFactory.CustomHeaders.Add("Authorization: Bearer " + credential.Token.AccessToken);
            service.RequestFactory = requestFactory;

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = service.Query(query);

            SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries.First(entry => entry.Title.Text == ConfigurationManager.AppSettings["SpreadsheetName"].ToString());

            WorksheetFeed wsFeed = spreadsheet.Worksheets;

            NameValueCollection worksheetWebSection = (NameValueCollection)ConfigurationManager.GetSection("worksheetSection");

            string[] worksheetNames = worksheetWebSection.AllKeys
                                                         .Select(key => worksheetWebSection[key])
                                                         .ToArray();

            foreach (var worksheetName in worksheetNames)
            {
                var worksheet = (WorksheetEntry)(wsFeed.Entries.First(entry => entry.Title.Text == worksheetName));
                workWorksheet(service, worksheet, updateAll);
            }
        }

        private void workWorksheet(SpreadsheetsService service, WorksheetEntry worksheet, bool updateAll)
        {
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink.Replace("/values", "/full"));
            cellQuery.MinimumRow = uint.Parse(ConfigurationManager.AppSettings["MinimumRow"]);
            cellQuery.MinimumColumn = uint.Parse(ConfigurationManager.AppSettings["MinimumColumn"]);
            cellQuery.MaximumColumn = uint.Parse(ConfigurationManager.AppSettings["MaximumColumn"]);
            CellFeed cellFeed = service.Query(cellQuery);

            uint nameColumn = ((CellEntry)cellFeed.Entries.Where(entry => ((CellEntry)entry).Value.Contains("Name")).First()).Column;
            uint maxColumn = nameColumn;
            uint pictureColumn = ((CellEntry)cellFeed.Entries.Where(entry => ((CellEntry)entry).Value.Contains("Picture")).First()).Column;
            maxColumn = maxColumn < pictureColumn ? pictureColumn : maxColumn;
            uint positionColumn = ((CellEntry)cellFeed.Entries.Where(entry => ((CellEntry)entry).Value.Contains("Position")).First()).Column;
            maxColumn = maxColumn < positionColumn ? positionColumn : maxColumn;
            uint emailColumn = ((CellEntry)cellFeed.Entries.Where(entry => ((CellEntry)entry).Value.Contains("E-mail")).First()).Column;
            maxColumn = maxColumn < emailColumn ? emailColumn : maxColumn;
            uint skypeColumn = ((CellEntry)cellFeed.Entries.Where(entry => ((CellEntry)entry).Value.Contains("Skype")).First()).Column;
            maxColumn = maxColumn < skypeColumn ? skypeColumn : maxColumn;

            string currentName = string.Empty;
            string currentPictureResourceId = string.Empty;
            string currentPosition = string.Empty;
            string currentEmail = string.Empty;
            string currentSkype = string.Empty;

            foreach (CellEntry cell in cellFeed.Entries)
            {
                if (cell.Column == nameColumn)
                {
                    currentName = cell.Value.Trim();
                }
                else if (cell.Column == pictureColumn)
                {
                    currentPictureResourceId = cell.InputValue.Trim();
                }
                else if (cell.Column == positionColumn)
                {
                    currentPosition = cell.Value.Trim();
                }
                else if (cell.Column == emailColumn)
                {
                    currentEmail = cell.Value.Trim().ToLower();
                }
                else if (cell.Column == skypeColumn)
                {
                    currentSkype = cell.Value.Trim();
                }

                if (cell.Column == maxColumn)
                {
                    if ((this.emails != null || this.emails.Contains(currentEmail)) &&
                        currentPictureResourceId != null &&
                        currentPictureResourceId.StartsWith("=HYPERLINK"))
                    {
                        Employee e = new Employee
                        {
                            Name = currentName,
                            Email = currentEmail,
                            PictureResourceId = this.GetResourceId(currentPictureResourceId),
                            Position = currentPosition,
                            Skype = currentSkype
                        };

                        switch (worksheet.Title.Text)
                        {
                            case SOFIA_WORKSHEET_NAME:
                                e.Town = enTown.Sofia;
                                break;
                            case PLOVDIV_WORKSHEET_NAME:
                                e.Town = enTown.Plovdiv;
                                break;
                            case VELIKO_TARNOVO_WORKSHEET_NAME:
                                e.Town = enTown.VelikoTarnovo;
                                break;
                            case VARNA_WORKSHEET_NAME:
                                e.Town = enTown.Varna;
                                break;
                            default:
                                e.Town = enTown.Unknown;
                                break;
                        }

                        if (updateAll || (this.emails != null && this.emails.Contains(currentEmail)))
                        {
                            this.employees.Add(currentEmail, e);

                            if (!string.IsNullOrWhiteSpace(e.PictureResourceId))
                            {
                                this.resourceIdEmail.Add(e.PictureResourceId, currentEmail);
                            }

                            if (this.emails != null)
                            {
                                this.emails.Remove(currentEmail);

                                if (this.emails.Count == 0)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetResourceId(string hyperlink)
        {
            int first = hyperlink.IndexOf('"');
            int second = hyperlink.IndexOf('"', first + 1);
            hyperlink = hyperlink.Substring(first + 1, second - first - 1);
            string firstOpeningDelimiter = "/file/d/";
            string firstEnclosingDelimiter = "/edit";

            int firstOpeningDelimiterIndex = hyperlink.IndexOf(firstOpeningDelimiter);
            int firstEnclosingDelimiterIndex = hyperlink.LastIndexOf(firstEnclosingDelimiter);

            if (firstEnclosingDelimiterIndex != -1 && firstEnclosingDelimiterIndex != -1)
            {
                int resourceIdStartingIndex = firstOpeningDelimiterIndex + firstOpeningDelimiter.Length;
                string resourceId = hyperlink.Substring(resourceIdStartingIndex, firstEnclosingDelimiterIndex - resourceIdStartingIndex);
                return resourceId;
            }

            //Due to changes in the way Google drive builds links to files, the opening delimiter is now 'open?id=', previous was 'leaf?id='.
            string secondOpeningDelimiter = "open?id=";
            string secondClosingDelimiter = "&hl=en_US";

            int secondOpeningDelimiterIndex = hyperlink.IndexOf(secondOpeningDelimiter);
            int secondEnclosingDelimiterIndex = hyperlink.LastIndexOf(secondClosingDelimiter);
            if (secondOpeningDelimiterIndex != -1)
            {
                if (secondEnclosingDelimiterIndex == -1)
                {
                    secondEnclosingDelimiterIndex = hyperlink.Length;
                }
                int resourceIdStartingIndex = secondOpeningDelimiterIndex + secondOpeningDelimiter.Length;
                string resourceId = hyperlink.Substring(resourceIdStartingIndex, secondEnclosingDelimiterIndex - resourceIdStartingIndex);
                return resourceId;
            }

            return string.Empty;
        }

        private List<string> emails;
        private Dictionary<string, Employee> employees;
        private Dictionary<string, string> resourceIdEmail;
        private const string SPREADSHEET_NAME = "MentorMate Employee Directory";
        private const string SOFIA_WORKSHEET_NAME = "Sofia, Bulgaria";
        private const string PLOVDIV_WORKSHEET_NAME = "Plovdiv, Bulgaria";
        private const string VELIKO_TARNOVO_WORKSHEET_NAME = "Veliko Tarnovo, Bulgaria";
        private const string VARNA_WORKSHEET_NAME = "Varna, Bulgaria";
        private const string MINNEAPOLIS_WORKSHEET_NAME = "Minneapolis, MN, USA";

        private ServiceAccountCredential GetCredential()
        {
            X509Certificate2 cert = new X509Certificate2(Config.GoogleOAuthCertPath, "notasecret", X509KeyStorageFlags.Exportable);

            ServiceAccountCredential.Initializer serviceAccountCredentialInitializer =
                new ServiceAccountCredential.Initializer(Config.GoogleOAuthSheetsEmail)
                {
                    Scopes = new[] { Config.GoogleOAuthScope }
                }
                .FromCertificate(cert);

            ServiceAccountCredential credential = new ServiceAccountCredential(serviceAccountCredentialInitializer);

            if (!credential.RequestAccessTokenAsync(System.Threading.CancellationToken.None).Result)
            {
                throw new InvalidOperationException("Access token request failed.");
            }

            return credential;
        }

        private void RemoveUnusedImages(Dictionary<string, string> userImageIds)
        {
            string[] imagePaths = SIO.Directory.GetFiles(Config.ImagesDirectoryPath);
            
            foreach (string imagePath in imagePaths)
            {
                if (!userImageIds.Keys.Any(id => imagePath.Contains(id)))
                {
                    SIO.File.Delete(imagePath);
                }
            }
        }

    }
}