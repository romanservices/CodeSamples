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
    public class StoryContentPageViewModel
    {
        private Story _story;
        private int _index;
        private int _height;
        private int _width;
        private int _wordCount;
        public StoryContentPageViewModel(Story story, int index, int height, int width)
        {
            _story = story;
            _index = index;
            _width = width;
            _height = height;


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
        private List<HtmlLine> ContentList
        {
            get
            {
                var contents = new List<HtmlLine>();
                var document = new HtmlDocument();
                document.LoadHtml(_story.StoryContent.Content);
                try
                {
                    var nodesToSplit = document.DocumentNode.SelectNodes("//p");
                    foreach (var node in nodesToSplit)
                    {
                        var lines = node.SplitIntoLines(MobileScreenCalculator.HtmlWordCount(_width, _height));
                        contents.AddRange(lines);
                    }
                    return contents;
                }
                catch (Exception)
                {
                    document.LoadHtml("<p>"+_story.StoryContent.Content+"</p>");

                    var nodesToSplit = document.DocumentNode.SelectNodes("//p");
                    foreach (var node in nodesToSplit)
                    {
                        var lines = node.SplitIntoLines(MobileScreenCalculator.HtmlWordCount(_width,_height));
                        contents.AddRange(lines);
                    }
                    return contents;
                    
                  
                }
               
            }
        }
        public int PageCount
        {
            get { return ContentList.Count; }
        }
        public string Content
        {
            get
            {
             
                return ContentList[_index].ToString();
            }
        }
    }
}
