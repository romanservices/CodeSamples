using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotFramework.Web.Mvc.Api.Models.Stories.View
{
    public class UserDetailsViewModel : BaseViewModel
    {
        public UserViewModel UserViewModel { get; set; }
        public StoryViewModel StoryViewModel { get; set; }
    }
}