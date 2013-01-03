using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Domain.StoryBook;
using DotFramework.Web.Mvc.Models.User;

namespace DotFramework.Web.Mvc.Models.StoryBookTitle.View
{
    public class TitleListViewModel : BaseViewModel
    {
       
        public TitleListViewModel(IList<Title> titles)
        {
            Titles = new List<TitleViewModel>();
            foreach (Title title in titles)
            {
                Titles.Add(new TitleViewModel(title));
            }
        }
        public IList<TitleViewModel> Titles { get; set; }
}
}