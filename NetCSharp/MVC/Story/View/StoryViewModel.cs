using System.Collections.Generic;
using System.Globalization;
using DotFramework.Domain;
using DotFramework.Domain.Enumerable;
using System.Linq;

namespace DotFramework.Web.Mvc.Models
{
    public class StoryViewModel : BaseViewModel
    {
        private readonly Domain.Story _innerStory;

        public StoryViewModel(Domain.Story story)
        {
            _innerStory = story;
        }

        public string StoryTitle
        {
            get { return _innerStory.Title; }
        }
        public int? StoryID
        {
            get { return _innerStory.Id; }
        }
    
        public string Synopsis
        {
            get { return _innerStory.Synopsis; }
        }
        public int ImageID
        {
            get
            {
                return _innerStory.CoverArt != null ? _innerStory.CoverArt.Id : 0;
            }
        }
        public string Genre
        {
            get { return _innerStory.Genre.GetDescription(); }
        }
        public string Content
        {
            get
            {
                if (_innerStory.StoryContent != null)
                {
                    return _innerStory.StoryContent.Content ?? "";
                }
                return "";
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
            get { return _innerStory.DatePublished != null ? _innerStory.DatePublished.Value.ToShortDateString() : null; }
        }
        public string Price
        {
            get
            {
                if (_innerStory.Price == null || _innerStory.Price == 0)
                {
                    return "Free";
                }
                return  _innerStory.Price.ToString(CultureInfo.InvariantCulture);
            }
        }
        public double PriceD
        {
            get { return _innerStory.Price; }
        }
        public PublishStatus PublishStatus
        {
            get { return _innerStory.PublishStatus; }
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