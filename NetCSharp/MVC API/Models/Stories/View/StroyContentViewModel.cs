using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotFramework.Domain;
using DotFramework.Domain.Enumerable;
using DotFramework.Web.Mvc.Api.ApiHelper;
using HtmlAgilityPack;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class StoryContentViewModel
    {
        private Story _story;
        public StoryContentViewModel(Story story)
        {
            _story = story;
        }
        public string Title
        {
            get { return _story.Title; }
        }
        public string Synopsis
        {
            get { return _story.Synopsis; }
        }
        public string Author
        {
            get { return _story.DisplayAuthor; }
        }
        
        public string Content
        {
            get
            {
               
                
                return _story.StoryContent.Content;
            }
        }
    }
}
