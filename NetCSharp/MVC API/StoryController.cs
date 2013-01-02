using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using DotFramework.Domain;
using DotFramework.Domain.DataModels;
using DotFramework.Domain.Enumerable;
using DotFramework.Infrastructure;
using DotFramework.Services;
using DotFramework.Services.EmailService;
using DotFramework.Services.StoryBookService;
using DotFramework.Services.UserService;
using DotFramework.Web.Mvc.Api.ApiHelper;
using DotFramework.Web.Mvc.Api.Models;
using DotFramework.Web.Mvc.Controllers;
using DotFramework.Web.Mvc.Helpers;
using BaseController = DotFramework.Web.Mvc.Controllers.API.BaseController;


namespace DotFramework.Web.Mvc.Api
{

    public class StoryController : BaseController
    {
        private readonly IStoryService _storyService;
        private readonly IReviewService _reviewService;

        private readonly IUserService _userService;
   
        public StoryController(IStoryService storyService, IUserService userService) : base(userService)
        {
            _storyService = storyService;
        }
        /// <summary>
        /// Gets the stories.
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [OptionalAuthorizeAttribute(true)]
        public PagedStoryListViewModel GetStories()
        {
            var stories = _storyService.GetStories(new StoryFilter(), 0, 10);

            return new PagedStoryListViewModel(stories, CurrentUser);
        }
        /// <summary>
        /// Gets the top stories.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [OptionalAuthorizeAttribute(true)]
        public StoryListViewModel GetTopStories(BaseInputModel inputModel)
        {
            if (!inputModel.Count.HasValue)
                inputModel.Count = 10;
            var stories = _storyService.GetTopStories((int) inputModel.Count);
            foreach (var storey in stories)
            {
                if (storey.CoverArt == null)
                {
                    var coverArt = new CoverArt
                                       {
                                           Cover = ResizeImage.ResizeImageFile(@"Content\img\64x64.gif",32,32)
                                       };
                    storey.CoverArt = coverArt;

                }
               
            }
            return new StoryListViewModel(stories, CurrentUser);
        }
 
        /// <summary>
        /// Gets the new stories.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [OptionalAuthorizeAttribute(true)]
        public StoryListViewModel GetNewStories(BaseInputModel inputModel)
        {
            if (!inputModel.Count.HasValue)
                inputModel.Count = 10;
            var stories = _storyService.GetTopStories((int)inputModel.Count);

            return new StoryListViewModel(stories, CurrentUser);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [OptionalAuthorizeAttribute(true)]
        public StoryListViewModel GetMyOwnedStories()
        {
            var filter = new StoryFilter
                             {
                                 PurchaserID = CurrentUser.Id
                             };
            var stories = _storyService.GetStoriesQuery(filter);
            foreach (var storey in stories)
            {
                if (storey.CoverArt == null)
                {
                    var coverArt = new CoverArt
                    {
                        Cover = ResizeImage.ResizeImageFile("\Content\img\64x64.gif", 32, 32)
                    };
                    storey.CoverArt = coverArt;

                }

            }
            return new StoryListViewModel(stories, CurrentUser);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [OptionalAuthorizeAttribute(true)]
        public StoryListViewModel GetMyCreatedStories()
        {
            var filter = new StoryFilter
            {
                AuthorID = CurrentUser.Id,
                PublishStatus = PublishStatus.All
            };
            var stories = _storyService.GetStoriesQuery(filter);
            foreach (var storey in stories)
            {
                if (storey.CoverArt == null)
                {
                    var coverArt = new CoverArt
                    {
                        Cover = ResizeImage.ResizeImageFile("contennt\img\64x64.gif", 32, 32)
                    };
                    storey.CoverArt = coverArt;

                }

            }
            return new StoryListViewModel(stories, CurrentUser);
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [OptionalAuthorizeAttribute(true), HttpPost]
        public StoryViewModel GetDetails(BaseInputModel inputModel)
        {
            if (!inputModel.Id.HasValue)
                inputModel.Id = 0;
            var story = _storyService.GetStoryByID((int)inputModel.Id);
            var vm = new StoryViewModel(story, CurrentUser);
            vm.AccountBalance = CurrentUser.AccountBalance;
            vm.StoryPrice = story.Price;
            return vm;
        }
        /// <summary>
        /// Gets the story content A.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [HttpPost]
        [OptionalAuthorizeAttribute(true)]
        public StoryContentViewModel GetStoryScrollContent(BaseInputModel inputModel)
        {
            if (!inputModel.Id.HasValue)
                inputModel.Id = 0;
            var story = _storyService.GetStoryByID((int)inputModel.Id);
            return new StoryContentViewModel(story);
        }




        /// <summary>
        /// Gets the story content page.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [HttpPost]
        [OptionalAuthorizeAttribute(true)]
        public StoryContentPageViewModel GetStoryContentPage(StoryContentPageInputModel inputModel)
        {
            if (!inputModel.Id.HasValue)
                inputModel.Id = 0;
            var story = _storyService.GetStoryByID((int)inputModel.Id);
            return new StoryContentPageViewModel(story, inputModel.Index, inputModel.Height, inputModel.Width);
        }

        /// <summary>
        /// Gets the content of the story.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [HttpPost]
        [OptionalAuthorizeAttribute(true)]
        public StoryContentListViewModel GetStoryListContent(StoryContentPageInputModel inputModel)
        {
            if (!inputModel.Id.HasValue)
                inputModel.Id = 0;
            var story = _storyService.GetStoryByID((int)inputModel.Id);
            return new StoryContentListViewModel(story, inputModel.Index, inputModel.Height, inputModel.Width);
        }


        /// <summary>
        /// Buys the specified input model.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [HttpPost]
        [OptionalAuthorizeAttribute(true)]
        public BaseViewModel Buy(BaseInputModel inputModel)
        {
            if (!inputModel.Id.HasValue)
                inputModel.Id = 0;
            var story = _storyService.GetStoryByID((int)inputModel.Id);
            var result = _storyService.BuyStory(story, CurrentUser);
            var rv = new BaseViewModel
                         {
                             Success = result
                         };
            return rv;
        }
        /// <summary>
        /// Gets the stories by genre.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [OptionalAuthorizeAttribute(true)]
        public PagedStoryListViewModel GetStoriesByGenre(BaseInputModel inputModel)
        {
            if (!inputModel.Page.HasValue)
                inputModel.Page = 1;
            var filter = new StoryFilter();
            if (inputModel.Id.HasValue)
            {
                 filter = new StoryFilter
                                 {
                                     Genre = (Genre) inputModel.Id
                                 };
            }
            var story = _storyService.GetStories(filter,(int)inputModel.Page,25);
            return new PagedStoryListViewModel(story, CurrentUser);
            
        }

        /// <summary>
        /// Saves the story.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [OptionalAuthorizeAttribute(true)]
        public BaseViewModel SaveStory(StoryInputModel inputModel)
        {
            if (!inputModel.Genre.HasValue)
                inputModel.Genre = Genre.ShortStory;
            var content = inputModel.StoryContent;
            var vm = new BaseViewModel();
            var story = _storyService.GetStoryByID(inputModel.StoryID) ?? new Story
                                                                              {
                                                                                  Title = inputModel.Title,
                                                                                  Synopsis = inputModel.Synopsis,
                                                                                  PublishStatus = inputModel.PublishStatus,
                                                                                  Genre = (Genre)inputModel.Genre,
                                                                                  UserAuthor = CurrentUser
                                                                                  

                                                                              };
            var storyContent = story.StoryContent ?? new StoryContent();
            storyContent.Content = content;
            story.StoryContent = storyContent;
            if (story.DatePublished == null)
                story.DatePublished = DateTime.UtcNow;
            if (story.DateCreated == null)
                story.DateCreated = DateTime.UtcNow;
            try
            {
                _storyService.SaveStory(story);
                vm.Success = true;
            }
            catch (Exception e)
            {
                vm.Success = false;
                vm.Message = e.Message;
            }
            return vm;
        }

        /// <summary>
        /// Saves the story.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [OptionalAuthorizeAttribute(true)]
        public StoryAutoSaveViewModel CheckAutoSave()
        {
            var story = _storyService.GetAutoSave(CurrentUser.Id);
            var vm = new StoryAutoSaveViewModel();
            if (story == null)
            {
                vm.Success = false;
                return vm;
            }

            vm.Success = true;
            vm.StoryContent = story.Content;
            vm.Title = story.Title;
            vm.Synopsis = story.Synopsis;
            return vm;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [OptionalAuthorizeAttribute(true)]
        public void AutoSave(StoryInputModel inputModel)
        {
            if (String.IsNullOrEmpty(inputModel.Title) && String.IsNullOrEmpty(inputModel.Synopsis) && String.IsNullOrEmpty(inputModel.StoryContent))
            {
                return;
            }
            var tempStory = new StoryAutoSave
            {
                Content = inputModel.StoryContent,
                Title = inputModel.Title,
                DraftDate = DateTime.Now,
                Synopsis = inputModel.Synopsis,
                UserId = CurrentUser.Id
            };
            _storyService.SaveAutoSave(tempStory);
        }

    }
}
