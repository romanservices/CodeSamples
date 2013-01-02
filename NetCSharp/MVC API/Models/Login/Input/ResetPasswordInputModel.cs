using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class ResetPasswordInputModel 
    {
        [Required, DisplayName("Reset Token")]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, DisplayName("Confirm Password"), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}