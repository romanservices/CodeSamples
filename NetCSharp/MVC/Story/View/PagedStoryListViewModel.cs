using System.Collections.Generic;
using DotFramework.Domain;
using DotFramework.Services.PagedList;



namespace DotFramework.Web.Mvc.Api.Models
{
    public class PagedStoryListViewModel : PagedViewModel<Story>
    {
       
        public PagedStoryListViewModel(IPagedList<Story> stories, User currentUser):base(stories)
        {
            Stories = new List<StoryViewModel>();
            foreach (var story in stories.Items)
            {
                Stories.Add(new StoryViewModel(story, currentUser));
            }
        }
        public IList<StoryViewModel> Stories { get; set; }
}
}