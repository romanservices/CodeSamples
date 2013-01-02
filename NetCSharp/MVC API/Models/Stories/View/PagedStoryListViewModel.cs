using System.Collections.Generic;
using DotFramework.Domain;
using DotFramework.Services.PagedList;


namespace DotFramework.Web.Mvc.Models
{
    public class PagedStoryListViewModel : PagedViewModel<Story>
    {
       
        public PagedStoryListViewModel(IPagedList<Story> stories):base(stories)
        {
            Stories = new List<StoryViewModel>();
            foreach (var story in stories.Items)
            {
                Stories.Add(new StoryViewModel(story));
            }
        }
        public IList<StoryViewModel> Stories { get; set; }
}
}