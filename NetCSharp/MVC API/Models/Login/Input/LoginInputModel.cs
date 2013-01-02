using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Web.Mvc.Models;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class LoginInputModel :BaseInputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int? StoryID { get; set; }
    }
}