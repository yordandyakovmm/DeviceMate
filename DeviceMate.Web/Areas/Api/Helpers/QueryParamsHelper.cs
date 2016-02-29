using DeviceMate.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeviceMate.Web.Areas.Api.Helpers
{
    public static class QueryParamsHelper
    {
        #region Public methods
        public static string[] GetKeywords(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                char delimiter = ' ';
                IList<string> valuesResult = new List<string>();
                string[] valueSplit = keyword.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string val in valueSplit)
                {
                    string trimmedVal = val.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedVal))
                    {
                        valuesResult.Add(trimmedVal.ToLower());
                    }
                }

                return valuesResult.ToArray();
            }
            else
            {
                return null;
            }
        }

        public static Dictionary<enSortColumn, enSortOrder> GetSort(string sortColumns)
        {
            if (!string.IsNullOrEmpty(sortColumns))
            {
                string[] sortColumnsSplit = sortColumns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Dictionary<enSortColumn, enSortOrder> sortParams = new Dictionary<enSortColumn, enSortOrder>();

                foreach (string column in sortColumnsSplit)
                {
                    enSortColumn deviceColumn;
                    enSortOrder sortOrder;

                    string[] sortParam = column.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    string sortColumn = sortParam[0];
                    string sortDirection = sortParam[1];

                    if (!string.IsNullOrWhiteSpace(sortColumn) &&
                        !string.IsNullOrWhiteSpace(sortDirection) &&
                        Enum.TryParse<enSortColumn>(sortColumn, true, out deviceColumn) &&
                        Enum.TryParse<enSortOrder>(sortDirection, true, out sortOrder))
                    {
                        sortParams.Add(deviceColumn, sortOrder);
                    }
                }

                return sortParams;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}