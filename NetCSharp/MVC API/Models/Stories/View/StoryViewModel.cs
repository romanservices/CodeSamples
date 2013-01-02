using System.Globalization;
using System.Web;
using DotFramework.Domain;
using System.Linq;
using DotFramework.Web.Mvc.Helpers;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class StoryViewModel :BaseViewModel
    {
        private readonly Story _innerStory;
        private readonly User _currentUser;

        public StoryViewModel(Story story, User currentUser)
        {
            _innerStory = story;
            _currentUser = currentUser;
        }

        public bool Own
        {
            get
            {
                return _currentUser.PurchasedStories.Any(s => s.Story.Id == _innerStory.Id);
            }
        }

        public string StoryTitle
        {
            get { return _innerStory.Title; }
        }

        public int TitleID
        {
            get { return _innerStory.Id; }
        }

        public string Synopsis
        {
            get { return _innerStory.Synopsis; }
        }
        public string CoverArtUrl
        {
            get
            {
                if (_innerStory.CoverArt != null)
                {
                    return "/Image/ShowImage?imageid="+_innerStory.CoverArt.Id;
                }
                return "/Image/ShowImage?imageid=0";
            }
        }
        public bool IsDraft
        {
            get
            {
                return _innerStory.PublishStatus == PublishStatus.Draft;
            }
        }
      

        public int AverageRating
        {
            get { return _innerStory.StoryReviewAverage != null ? _innerStory.StoryReviewAverage.AverageRating : 0; }
        }

        public int SortOrder
        {
            get { return _innerStory.StoryReviewAverage != null ? _innerStory.StoryReviewAverage.SortingOrder : 0; }
        }

        public int ReviewCount
        {
            get { return _innerStory.StoryReviewAverage != null ? _innerStory.StoryReviewAverage.ReviewCount : 0; }
        }
        public int PurchasedCount
        {
            get { return _innerStory.Purchasers != null ? _innerStory.Purchasers.Count : 0; }
        }
        public string Author
        {
            get { return _innerStory.DisplayAuthor; }
        }
        public string PublishDate
        {
            get { return _innerStory.DatePublished != null ? _innerStory.DatePublished.Value.ToShortDateString(): null; }
        }
        public string Price
        {
            get
            {
                if (_innerStory.Price == null || _innerStory.Price == 0)
                {
                    return "Free";
                }
                return "$ " + _innerStory.Price.ToString(CultureInfo.InvariantCulture);
            }
        }
       
        public string Genre
        {
            get { return _innerStory.Genre.GetDescription(); }
        }
        public int VoteCount
        {
            get
            {
                var count = 0;
                if (_innerStory.StoryVotes == null) return count;
                foreach (var storyVotes in _innerStory.StoryVotes)
                {
                    if (storyVotes.StoryVoteUp)
                        count++;
                    if (!storyVotes.StoryVoteUp)
                        count--;
                }
                return count;
            }
        }
      
    }
}