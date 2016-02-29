using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DeviceMate.Models.Entities;
using WebGrease.Css.Extensions;

namespace DeviceMate.Web.Models
{
    public class ColumnSelector
    {
        public IList<int> UserGridColumnsIds { get; set; }
        public IList<UsersGridColumn> UserGridColumns { get; set; }

        public ColumnSelector()
        {
            UserGridColumnsIds = new List<int>();
            UserGridColumns = new List<UsersGridColumn>();
        }

        public string UserGridColumnsIdsSer
        {
            get
            {
                return string.Join(",", UserGridColumnsIds);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                string[] ids = value.Split(',');
                foreach (var idAsString in ids)
                {
                    int id;
                    if (int.TryParse(idAsString, out id))
                    {
                        UserGridColumnsIds.Add(id);
                    }
                }
            }
        }

        public bool IsColumnVisible(string columnName)
        {
            var column = UserGridColumns.SingleOrDefault(x => x.GridColumn.Name == columnName);
            return column != null && column.Visible;
        }

        public MultiSelectList Columns
        {
            get
            {
                var items = UserGridColumns.OrderBy(x => x.GridColumn.Id).Select(x => new {x.Id, x.GridColumn.Name});
                var selectedItemsIds = UserGridColumns.Where(x => x.Visible).Select(x => x.Id);
                return new MultiSelectList(items, "Id", "Name", selectedItemsIds);
            }
        }

    }
}