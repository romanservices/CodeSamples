using DotFramework.Domain.StoryBook;

namespace DotFramework.Web.Mvc.Models.StoryBookTitle.View
{
    public class TitleViewModel
    {
        private readonly Title _innerTitle;

        public TitleViewModel(Title title)
        {
            _innerTitle = title;
        }

        public string StoryTitle
        {
            get { return _innerTitle.StoryTitle; }
        }

        public int TitleID
        {
            get { return _innerTitle.Id; }
        }

        public string Synopsis
        {
            get { return _innerTitle.Synopsis; }
        }

        public string ClipCount
        {
            get { return _innerTitle.StoryClips.Count.ToString(); }
        }

        public bool IsPrivate
        {
            get { return _innerTitle.IsPrivate; }
        }

        public string Password
        {
            get { return _innerTitle.Password; }
        }

        public string StoryType
        {
            get { return _innerTitle.Genre.ToString(); }
        }

        public int AverageRating
        {
            get { return _innerTitle.TitleReviewAverage != null ? _innerTitle.TitleReviewAverage.AverageRating : 0; }
        }

        public int SortOrder
        {
            get { return _innerTitle.TitleReviewAverage != null ? _innerTitle.TitleReviewAverage.SortingOrder : 0; }
        }

        public int ReviewCount
        {
            get { return _innerTitle.TitleReviewAverage != null ? _innerTitle.TitleReviewAverage.ReviewCount : 0; }
        }
        public int PurchasedCount
        {
            get { return _innerTitle.Purchasers != null ? _innerTitle.Purchasers.Count : 0; }
        }

        public int VoteCount
        {
            get
            {
                var count = 0;
                if (_innerTitle.TitleVotes == null) return count;
                foreach (var titleVote in _innerTitle.TitleVotes)
                {
                    if (titleVote.TitleVoteUp)
                        count++;
                    if (!titleVote.TitleVoteUp)
                        count--;
                }
                return count;
            }
        }
      
    }
}