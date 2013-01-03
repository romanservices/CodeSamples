using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotFramework.Web.Mvc.Models.StoryBookTitle.Response
{
    public class RequestToContributeResponseModel :JsonResponseModel
    {
        public bool IsLocked { get; set; }
        public string Status { get; set; }
    }
}