using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeviceMate.Web.Models
{
    public class Pager
    {
        const int NumberOfPageLinks = 5;
        const int DefaultPageSize = 20;

        private int _pageSize = DefaultPageSize;

        public int Page { get; set; }
        public int NumberOfPages { get; set; }
        public int FirstPageNumber { get; set; }
        public int LastPageNumber { get; set; }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public int NextPage
        {
            get
            {
                return Page + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                return Page - 1;
            }
        }

        public bool IsFirstPage
        {
            get
            {
                return Page == 1;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return Page - 1 >= 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return Page + 1 <= NumberOfPages;
            }
        }

        public bool IsLastPage
        {
            get
            {
                return NumberOfPages == 0 || Page == NumberOfPages;
            }
        }

        public IList<SelectListItem> PageSizes
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem {Text = 10.ToString(), Value = 10.ToString(), Selected = PageSize == 10},
                    new SelectListItem {Text = 20.ToString(), Value = 20.ToString(), Selected = PageSize == 20},
                    new SelectListItem {Text = 30.ToString(), Value = 30.ToString(), Selected = PageSize == 30},
                    new SelectListItem {Text = 50.ToString(), Value = 50.ToString(), Selected = PageSize == 50}
                };
            }
        }

        public void Init(int totalNumberOfItems)
        {
            // Page property is already populated during binding via the default constructor.

            NumberOfPages = (int)Math.Ceiling(totalNumberOfItems / (double)PageSize);
            if (Page <= 0)
            {
                Page = 1;
            } 
            else if (Page > NumberOfPages)
            {
                Page = NumberOfPages;
            }

            SetFirstLastPages();
        }

        private void SetFirstLastPages()
        {
            int firstPage;
            int lastPage;
            var deltaFirst = 0;
            var deltaLast = 0;

            var twoPagesBack = Page - NumberOfPageLinks / 2;
            var twoPagesForward = Page + NumberOfPageLinks / 2;

            if (twoPagesBack < 1)
            {
                deltaFirst = 1 - twoPagesBack;
                firstPage = 1;
            }
            else
            {
                firstPage = twoPagesBack;
            }

            if (twoPagesForward > NumberOfPages)
            {
                deltaLast = twoPagesForward - NumberOfPages;
                lastPage = NumberOfPages;
            }
            else
            {
                lastPage = twoPagesForward;
            }

            if (firstPage - deltaLast >= 1)
            {
                firstPage -= deltaLast;
            }

            if (lastPage + deltaFirst <= NumberOfPages)
            {
                lastPage += deltaFirst;
            }

            FirstPageNumber = firstPage;
            LastPageNumber = lastPage;
        }
    }
}