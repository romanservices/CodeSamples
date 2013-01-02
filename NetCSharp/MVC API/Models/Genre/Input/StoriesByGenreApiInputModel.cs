using DotFramework.Domain;
using DotFramework.Domain.Enumerable;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class TitlesByGenreApiInputModel 
    {
        public Genre Genre { get; set; }
        public PublishStatus PublishStatus { get; set; }
    }
}