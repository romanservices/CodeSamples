using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class StoryAutoSaveViewModel :BaseViewModel
    {
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string StoryContent { get; set; }
    }
}