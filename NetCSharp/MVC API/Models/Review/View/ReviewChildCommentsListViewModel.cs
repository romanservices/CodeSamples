using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Domain;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class ReviewChildCommentsListViewModel : BaseViewModel
    {

        public ReviewChildCommentsListViewModel(IEnumerable<ReviewChildComment> reviewChildComments)
        {
            ReviewChild = new List<ReviewChildCommentsViewModel>();
            foreach (var review in reviewChildComments)
            {
                ReviewChild.Add(new ReviewChildCommentsViewModel(review));
            }
        }

        public IList<ReviewChildCommentsViewModel> ReviewChild { get; set; }
    }
}