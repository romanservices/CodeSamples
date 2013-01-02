using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class UserInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}