using System.Collections.Generic;
using DotFramework.Domain;


namespace DotFramework.Web.Mvc.Api.Models
{
    public class ReviewViewModel
    {
        private readonly Review _innerReview;
        private IList<ReviewChildCommentsViewModel> _childCommentsViewModels; 
      

        public ReviewViewModel(Review review)
        {
            _innerReview = review;
            _childCommentsViewModels = new List<ReviewChildCommentsViewModel>();
        }
        public string Reviewer
        {
            get
            {
                if(_innerReview.Reviewer != null)
                return _innerReview.Reviewer.FullName;
                return "No Name";
            }
        }
        public string ReviewDate
        {
            get { return _innerReview.ReviewDate.ToShortDateString(); }
        }
        public string Comments
        {
            get { return _innerReview.Comments; }
        }
        public int ReviewRating
        {
            get { return (int)_innerReview.ReviewRating; }
        }
        public string StoryTitle
        {
            get { return _innerReview.Story.Title; }
        }
        public int StoryID
        {
            get { return _innerReview.Story.Id; }
        }
        public int ReviewID { get { return _innerReview.Id; } }
        public int VoteCount
        {
            get
            {
                var count = 0;
                if (_innerReview.ReviewVotes == null) return count;
                foreach (var reviewVote in _innerReview.ReviewVotes)
                {
                    if (reviewVote.ReviewVoteUp)
                        count++;
                    if (!reviewVote.ReviewVoteUp)
                        count--;
                }
                return count;
            }
        }
    
        public IList<ReviewChildCommentsViewModel> ReviewChildComments
        {
            get {
                foreach (var reviewChildComment in _innerReview.ReviewChildComments)
                {
                    _childCommentsViewModels.Add(new ReviewChildCommentsViewModel(reviewChildComment));
                }
                return _childCommentsViewModels;
            }
        }
       
    }
}