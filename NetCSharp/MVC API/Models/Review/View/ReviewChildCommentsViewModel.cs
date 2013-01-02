using System;
using DotFramework.Domain;


namespace DotFramework.Web.Mvc.Api.Models
{
    public class ReviewChildCommentsViewModel :BaseViewModel
    {
        private readonly ReviewChildComment _reviewChildComment;
        public ReviewChildCommentsViewModel(ReviewChildComment reviewChildComment)
        {
            _reviewChildComment = reviewChildComment;
        }
        public string Comment { get { return _reviewChildComment.Comment; } }
        public String CommentDate { get { return _reviewChildComment.CommentDate.ToShortDateString(); } }
        public string Commenter { get { return _reviewChildComment.Commenter.FullName; } }
        public int ReviewChildCommentID { get { return _reviewChildComment.Id; } }
        public int VoteCount
        {
            get
            {
                var count = 0;
                if (_reviewChildComment.ReviewChildCommentVotes == null) return count;
                foreach (var reviewChildCommentVote in _reviewChildComment.ReviewChildCommentVotes)
                {
                    if (reviewChildCommentVote.ReviewChildVoteUp)
                        count++;
                    if (!reviewChildCommentVote.ReviewChildVoteUp)
                        count--;
                }
                return count;
            }
        }

    }
}