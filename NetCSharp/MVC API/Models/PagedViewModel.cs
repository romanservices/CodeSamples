using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Services.PagedList;
using DotFramework.Web.Mvc.Api.Models;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class PagedViewModel<T> : BaseViewModel
    {
        public int CurrentPage { get; set; }
        public int NumPages { get; set; }
        public int NumPerPage { get; set; }
        public bool HasMorePages { get { return CurrentPage < NumPages; }}
        public int NextPage { get; set; }
        public int PrevPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }

        public PagedViewModel(IPagedList<T> pagedList)
        {
            if (pagedList == null) return;
            CurrentPage = pagedList.CurrentPage;
            NumPages = pagedList.TotalPages;
            NumPerPage = pagedList.PageSize;
            HasNext = pagedList.IsNextPage;
            HasPrev = pagedList.IsPreviousPage;
            NextPage = pagedList.CurrentPage + 1;
            PrevPage = pagedList.CurrentPage - 1;
           
        }
    }
}