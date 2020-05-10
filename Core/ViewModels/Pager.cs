using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModels
{
    public class Pager
    {
        public int Page { get; set; }
        public int PageSize { get; set; }        
        public int OffSet { get; set; }
        public string OrderBy { get; set; }
        public string SearchText { get; set; }

        public Pager(int page,int pagesize, string orderby, string searchtext)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pagesize < 1 ? 1 : pagesize;
            OffSet = (Page - 1) * PageSize;
            OrderBy = orderby;
            SearchText = searchtext;
        }
    }
}
