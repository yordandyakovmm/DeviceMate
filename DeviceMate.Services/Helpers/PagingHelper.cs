using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceMate.Models.Domain.Abstract;

namespace DeviceMate.Services.Helpers
{
    public static class PagingHelper<T> where T : class
    {
        public static IEnumerable<T> GetCurrentPage(IEnumerable<T> items, PagedItems pagedItem, int offset, int itemsPerPage)
        {
            pagedItem.TotalItems = items.Count();
            pagedItem.ItemsPerPage = itemsPerPage;

            IEnumerable<T> itemsPage = items
                                        .Skip(offset)
                                        .Take(itemsPerPage);

            if (itemsPage.Count() == 0)
            {
                itemsPage = items.Take(itemsPerPage);
            }

            pagedItem.CurrentPage = (int)Math.Ceiling((double)(offset + 1) / (double)itemsPerPage);
            return itemsPage;
        }
    }
}
