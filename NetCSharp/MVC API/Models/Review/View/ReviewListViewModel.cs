using System.Collections.Generic;
using DotFramework.Domain;


namespace DotFramework.Web.Mvc.Api.Models
{
    public class ReviewListViewModel : BaseViewModel
    {
       
        public ReviewListViewModel(IList<Review> reviews)
        {
            Reviews = new List<ReviewViewModel>();
            foreach (Review review in reviews)
            {
                Reviews.Add(new ReviewViewModel(review));
            }
        }
        public IList<ReviewViewModel> Reviews { get; set; }
}
}