using System;
using System.Collections.Generic;
using DotFramework.Domain.Enumerable;
using SharpArch.Domain.DomainModel;

namespace DotFramework.Domain
{
    public class Story :Entity
    {
        //Values
        public virtual string Title { get; set; }
        public virtual string Synopsis { get; set; }
        public virtual string StringAuthor { get; set; }
        public virtual double Price { get; set; }
        public virtual DateTime? DateCreated { get; set; }
        public virtual DateTime? DatePublished { get; set; }
        public virtual DateTime? DateRemoved { get; set; }
 
        //Enum
        public virtual PublishStatus PublishStatus { get; set; }
        public virtual GenreCategory GenreCategory { get; set; }
        public virtual Rating Rating { get; set; }
        //references
        public virtual StoryContent StoryContent { get; set; }
        public virtual CoverArt CoverArt { get; set; }
        public virtual AudioBook AudioBook { get; set; }
        public virtual User UserAuthor { get; set; }
        public virtual StoryReviewAverage StoryReviewAverage { get; set; }
     
        //HasMany
        public virtual Genre Genre { get; set; }
        public virtual IList<Review> Reviews { get; set; }
   
        public virtual IList<StoryBadge> StoryBadges { get; set; }
        public virtual IList<PurchasedStories> Purchasers { get; set; }
        public virtual IList<StoryVote> StoryVotes { get; set; }

        //No setter
        public virtual string DisplayAuthor
        {
            get { return StringAuthor; }

        }
       

        public Story()
        {
            StoryBadges = new List<StoryBadge>();
            PublishStatus = PublishStatus.Public;
            Reviews = new List<Review>();
            Purchasers = new List<PurchasedStories>();
            StoryVotes = new List<StoryVote>();
           
        }

    }
}
