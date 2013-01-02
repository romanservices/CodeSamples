using System.ComponentModel.DataAnnotations;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class ForgotPasswordInputModel 
    {
        [Required]
        public string Email { get; set; }
    }
}