

using DotFramework.Domain;
using DotFramework.Domain.DataModels;
using DotFramework.Domain.Enumerable;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class GenreApiViewModel 
    {
        private readonly GenreStoryCount _innerGenre;

        public GenreApiViewModel(GenreStoryCount genre)
        {
            _innerGenre = genre;
        }

        public string Genre
        {
            get { return _innerGenre.Genre.GetDescription(); }
        }
        public int StoryCount
        {
            get { return _innerGenre.StoryCount; }
        }


     

       
    }
}
