using System.Collections.Generic;
using DotFramework.Domain;
using DotFramework.Services.PagedList;


namespace DotFramework.Web.Mvc.Api.Models
{
    public class StoryListViewModel 
    {
       
        public StoryListViewModel(IList<Story> stories, User currentUser)
        {
            Stories = new List<StoryViewModel>();
            foreach (var story in stories)
            {
                Stories.Add(new StoryViewModel(story, currentUser));
            }
        }
        public IList<StoryViewModel> Stories { get; set; }
}
}