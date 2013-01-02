using System.Collections.Generic;
using DotFramework.Domain;
using DotFramework.Services.PagedList;


namespace DotFramework.Web.Mvc.Models
{
    public class StoryListViewModel 
    {
       
        public StoryListViewModel(IList<Story> stories)
        {
            Stories = new List<StoryViewModel>();
            foreach (var story in stories)
            {
                Stories.Add(new StoryViewModel(story));
            }
        }
        public IList<StoryViewModel> Stories { get; set; }
}
}