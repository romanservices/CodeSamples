using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class StoryContentPageInputModel : BaseInputModel
    {
        public int Index { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}