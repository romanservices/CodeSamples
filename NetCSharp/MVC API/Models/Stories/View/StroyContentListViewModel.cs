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
    public class StoryContentListViewModel
    {
        private Story _story;
        private int _index;
        private int _height;
        private int _width;
        private int _wordCount;
        public StoryContentListViewModel(Story story, int index, int height, int width)
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
        public List<string> ContentList
        {
            get
            {
                var contents = new List<string>();
                var document = new HtmlDocument();
                document.LoadHtml(_story.StoryContent.Content);
                try
                {
                    var nodesToSplit = document.DocumentNode.SelectNodes("//p");
                    foreach (var node in nodesToSplit)
                    {
                        var lines = node.SplitIntoLines(MobileScreenCalculator.HtmlWordCount(_width, _height));
                        foreach (var htmlLine in lines)
                        {
                            contents.Add(htmlLine.ToString());
                        }
                        //contents.AddRange(lines);
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
                        foreach (var htmlLine in lines)
                        {
                           contents.Add(htmlLine.ToString()); 
                        }
                    }
                    return contents;
                    
                  
                }
               
            }
        }
        public int PageCount
        {
            get { return ContentList.Count; }
        }
       
    }
}
