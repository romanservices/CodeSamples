
namespace DotFramework.Web.Mvc.Api.Models
{
    public class CommentInputModel : BaseInputModel
    {
        public string Comments { get; set; }
        public int ReviewID { get; set; }
        public int? Vote { get; set; }
    }
}