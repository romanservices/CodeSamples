using System;
using System.Collections.Generic;
using DotFramework.Domain;
using System.Linq;
using DotFramework.Domain.Filters;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using SharpArch.NHibernate;

namespace DotFramework.Infrastructure
{
    public class StoryRepository : LinqRepository<Story>, IStoryRepository
    {
        public IEnumerable<Story> Search(StoryFilter filter, int page, int numPerPage, ref int totalRecords)
        {
            Story storyAlias = null;
            var query = Session.Query<Story>();
            if (!String.IsNullOrEmpty(filter.SearchTerm))
            {
                var or = Restrictions.Disjunction();
                or.Add(Restrictions.On<Story>(u => storyAlias.Title).IsLike(filter.SearchTerm, MatchMode.Anywhere));
               
            }
            if (!String .IsNullOrEmpty(filter.AuthorName))
            {
                query = query.Where(s => s.StringAuthor == filter.AuthorName);
            }
            if(filter.AuthorID.HasValue)
            {
                query = query.Where(s =>s.UserAuthor.Id == filter.AuthorID.Value);
            }
            if(filter.Genre.HasValue)
            {
                query = query.Where(s => s.Genre == filter.Genre);
            }
            if(filter.PublishStatus.HasValue)
            {
                if (filter.PublishStatus.Value != PublishStatus.All)
                {
                    query = query.Where(s => s.PublishStatus == filter.PublishStatus.Value);
                }
            }
            if(filter.PurchaserID.HasValue)
            {
                query = query.Where(s => s.Purchasers.Any(p => p.User.Id == filter.PurchaserID.Value));
            }
            if (filter.ReviewerID.HasValue)
            {
                query = query.Where(s => s.Reviews.Any(p => p.Reviewer.Id == filter.ReviewerID.Value));
            }
            //query.(Transformers.DistinctRootEntity);
            int firstResult = (page * numPerPage) - numPerPage;
            totalRecords = query.Count();
            return query.Skip(firstResult).Take(numPerPage).ToList();
        }

        public IList<Story> Search(StoryFilter filter)
        {
            Story storyAlias = null;
            var query = Session.QueryOver<Story>();
            if (!String.IsNullOrEmpty(filter.SearchTerm))
            {
                var or = Restrictions.Disjunction();
                or.Add(Restrictions.On<Story>(u => u.Title).IsLike(filter.SearchTerm, MatchMode.Anywhere));
                or.Add(Restrictions.On<Story>(u => u.StringAuthor).IsLike(filter.SearchTerm, MatchMode.Anywhere));
               
                query.And(or);

            }
            if (filter.AuthorID.HasValue)
            {
                query = query.Where(s => s.UserAuthor.Id == filter.AuthorID.Value);
            }
            if (filter.Genre.HasValue)
            {
                query = query.Where(s => s.Genre == filter.Genre);
            }
            if (filter.PublishStatus.HasValue)
            {
                if (filter.PublishStatus.Value != PublishStatus.All)
                {
                    query = query.Where(s => s.PublishStatus == filter.PublishStatus.Value);
                }
            }
            if (filter.PurchaserID.HasValue)
            {
                query = query.Where(s => s.Purchasers.Any(p => p.User.Id == filter.PurchaserID.Value));
            }
            if (filter.ReviewerID.HasValue)
            {
                query = query.Where(s => s.Reviews.Any(p => p.Reviewer.Id == filter.ReviewerID.Value));
            }
            query.TransformUsing(Transformers.DistinctRootEntity);
           
            return query.List();
        }

        public IList<Story> SearchQuery(StoryFilter filter)
        {
            Story storyAlias = null;
            var query = Session.Query<Story>();
            if (!String.IsNullOrEmpty(filter.SearchTerm))
            {
                var or = Restrictions.Disjunction();
                or.Add(Restrictions.On<Story>(u => u.Title).IsLike(filter.SearchTerm, MatchMode.Anywhere));
                or.Add(Restrictions.On<Story>(u => u.StringAuthor).IsLike(filter.SearchTerm, MatchMode.Anywhere));

    

            }
            if (filter.AuthorID.HasValue)
            {
                query = query.Where(s => s.UserAuthor.Id == filter.AuthorID.Value);
            }
            if (filter.Genre.HasValue)
            {
                query = query.Where(s => s.Genre == filter.Genre);
            }
            if (filter.PublishStatus.HasValue)
            {
                if (filter.PublishStatus.Value != PublishStatus.All)
                {
                    query = query.Where(s => s.PublishStatus == filter.PublishStatus.Value);
                }
            }
            if (filter.PurchaserID.HasValue)
            {
                query = query.Where(s => s.Purchasers.Any(p => p.User.Id == filter.PurchaserID.Value));
            }
            if (filter.ReviewerID.HasValue)
            {
                query = query.Where(s => s.Reviews.Any(p => p.Reviewer.Id == filter.ReviewerID.Value));
            }
            

            return query.ToList();
        }
    }
}
