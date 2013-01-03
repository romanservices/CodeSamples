using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Domain.Enumerable;

namespace DotFramework.Web.Mvc.Models.StoryBookTitle.Input
{
    public class TitleInputModel :BaseInputModel
    {
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public PublishStatus PublishStatus { get; set; }
        public Genre Genre { get; set; }
        public string Password { get; set; }
    }
}