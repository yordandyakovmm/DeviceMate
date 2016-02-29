
namespace DeviceMate.Core.Helpers
{
    public static class GoogleImageHelper
    {
        public static string GetImageViewUrl(string fileId)
        {
            if (!string.IsNullOrWhiteSpace(fileId))
            {
                //fileId = fileId.Replace(Common.ResourceFilePrefix, string.Empty);
                return string.Format(Common.UserImagePattern, fileId);
            }
            else
            {
                return null;
            }
        }
    }
}
