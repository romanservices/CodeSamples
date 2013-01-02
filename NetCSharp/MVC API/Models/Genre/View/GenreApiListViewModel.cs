using System.Collections.Generic;
using DotFramework.Domain;
using DotFramework.Domain.DataModels;
using DotFramework.Domain.Enumerable;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class GenreApiListViewModel
    {

        public GenreApiListViewModel(IEnumerable<GenreStoryCount> genres)
        {
           Genres = new List<GenreApiViewModel>();
           foreach (var genre in genres)
            {
                Genres.Add(new GenreApiViewModel(genre));
            }
        }
        public IList<GenreApiViewModel> Genres { get; set; }
        public int AllStoryCount { get; set; }
}
}
