
namespace DotFramework.Web.Mvc.Api.Models
{
    public class ReviewInputModel :BaseInputModel
    {
        public string Comments { get; set; }
        public int ReviewRating { get; set; }
        public int StoryID { get; set; }
        public int? Vote { get; set; }
    }
}