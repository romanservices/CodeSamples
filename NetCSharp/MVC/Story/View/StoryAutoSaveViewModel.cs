using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Domain;

namespace DotFramework.Web.Mvc.Models
{
    public class StoryAutoSaveViewModel :BaseViewModel
    {
        private readonly StoryAutoSave _storyAutoSave;
        public StoryAutoSaveViewModel(StoryAutoSave storyAutoSave)
        {
            _storyAutoSave = storyAutoSave;
        }
        public string Content
        {
            get { return _storyAutoSave.Content; }
        }
        public string Title
        {
            get { return _storyAutoSave.Title; }
        }
        public string Synopsis
        {
            get { return _storyAutoSave.Synopsis; }
        }
        public string DraftDate
        {
            get { return _storyAutoSave.DraftDate.ToShortDateString(); }
        }
        public int? Id
        {
            get { return _storyAutoSave.Id; }
        }
        public int UserId
        {
            get { return _storyAutoSave.UserId; }
        }
    }
}