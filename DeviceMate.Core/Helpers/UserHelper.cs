using DeviceMate.Core.Extensions;
using System;
using System.Linq;

namespace DeviceMate.Core.Helpers
{
    public static class UserHelper
    {
        public static string GetNameByEmail(string email)
        {
            int nameMaxLength = 100;

            int indexOfAt = email.IndexOf('@');
            string emailName = email.Substring(0, email.Length - (email.Length - indexOfAt));

            string[] emailNameTokens = emailName.Split(new char[] { '.', '-' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(token => token.ToUpperFirstLetter()).ToArray();

            string name = string.Join(" ", emailNameTokens);

            if (name.Length > nameMaxLength)
            {
                name = name.Substring(0, nameMaxLength);
            }

            return name;
        }
    }
}
