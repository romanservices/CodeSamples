using System.Collections.Generic;
using DotFramework.Domain;
using DotFramework.Domain.Filters;
using SharpArch.Domain.PersistenceSupport;

namespace DotFramework.Infrastructure
{
    public interface IStoryRepository : ILinqRepositoryWithTypedId<Story, int>
    {
        IEnumerable<Story> Search(StoryFilter filter, int page, int numPerPage, ref int totalRecords);
        IList<Story> Search(StoryFilter filter);
        IList<Story> SearchQuery(StoryFilter filter);
    }
}
