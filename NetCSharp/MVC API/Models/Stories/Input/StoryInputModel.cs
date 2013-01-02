using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Domain;
using DotFramework.Domain.Enumerable;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class StoryInputModel :BaseInputModel
    {
        public int StoryID { get; set; }
        public string StoryContent { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public PublishStatus PublishStatus { get; set; }
        
    }
}