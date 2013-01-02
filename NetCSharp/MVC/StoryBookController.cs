using System;
using System.Web;
using System.Web.Mvc;
using DotFramework.Domain;
using DotFramework.Domain.Enumerable;
using DotFramework.Domain.Reviews;
using DotFramework.Domain.StoryBook;
using DotFramework.Infrastructure;
using DotFramework.Infrastructure.UserRepository;
using DotFramework.Services.EmailService;
using DotFramework.Services.StoryBookService;
using DotFramework.Services.UserService;
using DotFramework.Web.Mvc.Attributes;
using DotFramework.Web.Mvc.Models;
using DotFramework.Web.Mvc.Models.StoryBookCategory.Input;
using DotFramework.Web.Mvc.Models.StoryBookCategory.View;
using DotFramework.Web.Mvc.Models.StoryBookClips.Input;
using DotFramework.Web.Mvc.Models.StoryBookClips.View;
using DotFramework.Web.Mvc.Models.StoryBookInvite.Input;
using DotFramework.Web.Mvc.Models.StoryBookReview.Input;
using DotFramework.Web.Mvc.Models.StoryBookTitle.Input;
using DotFramework.Web.Mvc.Models.StoryBookTitle.Response;
using DotFramework.Web.Mvc.Models.StoryBookTitle.View;
using DotFramework.Web.Mvc.Wrappers;

namespace DotFramework.Web.Mvc.Controllers
{
    public class StoryBookController : BaseController
    {
        //
        // GET: /StoryBook/
        private readonly IStoryBookService _storyBookService;
        private readonly IReviewService _reviewService;
        private readonly EmailService _emailService;

        public StoryBookController(IStoryBookService storyBookService, IReviewService reviewService, IUserService userService)
            : base(userService)
        {
            _storyBookService = storyBookService;
            _reviewService = reviewService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [RequiresAuthentication]
        public ActionResult GetGenres()
        {
            var categories = _storyBookService.GetGenre(CurrentUser);

            if (categories.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new GenreListViewModel(categories);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult GetMyStories()
        {
            var categories = _storyBookService.GetMyStories(CurrentUser);

            if (categories.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new GenreListViewModel(categories);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult GetPubByGenre(PublicationByGenreInputModel inputModel)
        {

            var pubs = _storyBookService.GetPublishStatusByGenre(inputModel.Genre, CurrentUser);

            if (pubs.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new PublicationListViewModel(pubs);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult GetPubs()
        {

            var pubs = _storyBookService.GetPublishStatus(CurrentUser);

            if (pubs.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new PublicationListViewModel(pubs);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
     
        [RequiresAuthentication]
        public ActionResult GetMyPubByGenre(PublicationByGenreInputModel inputModel)
        {

            var pubs = _storyBookService.GetMyPublishStatusByGenre(inputModel.Genre, CurrentUser);

            if (pubs.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new PublicationListViewModel(pubs);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult GetTitles()
        {
            var titles = _storyBookService.GetTitles(CurrentUser);

            if (titles.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new TitleListViewModel(titles);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
         //[RequiresAuthentication]
        public ActionResult GetTop10Titles()
        {
            var titles = _storyBookService.GetTop10Titles(CurrentUser);

            if (titles.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new TitleListViewModel(titles);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult GetTitlesByGenre(TitlesByGenreInputModel inputModel)
        {
            var titles = _storyBookService.GetTitlesByGenre(inputModel.Genre, inputModel.PublishStatus, CurrentUser);

            if (titles.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new TitleListViewModel(titles);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult GetTitlesByPublication(TitlesByGenreInputModel inputModel)
        {
            var titles = _storyBookService.GetTitlesByPublication(inputModel.PublishStatus, CurrentUser);

            if (titles.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new TitleListViewModel(titles);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult GetMyTitlesByGenre(TitlesByGenreInputModel inputModel)
        {
            var titles = _storyBookService.GetMyTitlesByGenre(inputModel.Genre, inputModel.PublishStatus, CurrentUser);

            if (titles.Count < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var vm = new TitleListViewModel(titles);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult GetStory(GetClipsInputModel inputModel)
        {
            
            var title = _storyBookService.GetTitleByID(inputModel.StoryID);
           
            var validationState = new ModelStateWrapper(ModelState);
            
            if(title.IsPrivate)
            {
                if(!_storyBookService.ValidatePassword(title,inputModel.Password,validationState))
                {
                    var vm1 = new JsonResponseModel { Errors = validationState.Errors };
                    return Json(vm1, JsonRequestBehavior.AllowGet);
                }
            }
            var storyclips = _storyBookService.GetStoryClipsByID(inputModel.StoryID);
            if (storyclips.Count < 0)
            {
                throw new HttpException(404, "NotFound");

            }
            var vm = new StoryBookClipListViewModel(storyclips)
                         {CanEdit = title.User == CurrentUser, Publication = title.PublishStatus.ToString()};
            return Json(vm, JsonRequestBehavior.AllowGet);
          

            
        }
        [RequiresAuthentication]
        public ActionResult RequestContribute(int storyID, string deviceID)
        {
            var response = new RequestToContributeResponseModel();

            var title = _storyBookService.GetTitleByID(storyID);
            if(title == null)
            {
                return null;
            }
            if (title.DateLocked.HasValue)
            {
                var lockDate = title.DateLocked ?? null;

                var ts = new TimeSpan(0, 0, 10, 0);
               
                if (lockDate - DateTime.UtcNow > ts)
                {
                    title.StoryStatus = StoryStatus.Open;
                }
            }
            response.Status = title.StoryStatus.GetDescription();
            switch (title.StoryStatus)
            {
                    case StoryStatus.Ended:
                    response.IsLocked = true;
                    break;
                    case StoryStatus.Locked:
                    response.IsLocked = true;
                    break;
                    case StoryStatus.RequestToEnd:
                    response.IsLocked = true;
                    break;
                    case StoryStatus.Open:
                    response.IsLocked = false;
                    title.StoryStatus = StoryStatus.Locked;
                    title.DeviceID = deviceID;
                    title.DateLocked = DateTime.UtcNow;
                    _storyBookService.UpdateTitle(title);
                    break;   
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult SubmitClip(SubmitClipInputModel inputModel)
        {

            var title = _storyBookService.GetTitleByID(inputModel.StoryID);
            var decodedText = "  "+Server.UrlDecode(inputModel.Text);
       
            if (inputModel.IsParagraph)
                decodedText = "<p>" + decodedText + "</p>";
            var clip = new StoryClip
                           {
                               Clip = decodedText,
                               DateAdded = DateTime.UtcNow,
                               Included = true,
                               Title = title,
                               User = CurrentUser
                           };
            var validationState = new ModelStateWrapper(ModelState);
            var vm = new JsonResponseModel();
            // If both the view model and the user object are valid
            if (_storyBookService.ValidateCanAddClip(clip,inputModel.DeviceID, validationState))
            {
                clip.Title.StoryStatus = StoryStatus.Open;
                _storyBookService.AddStoryClip(clip);
                vm.Success = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            vm.Errors = validationState.Errors;
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult CancelClip(int storyID, string deviceID)
        {

            var title = _storyBookService.GetTitleByID(storyID);
            title.StoryStatus = StoryStatus.Open;
            title.DeviceID = "";
            _storyBookService.UpdateTitle(title);
            var validationState = new ModelStateWrapper(ModelState);
            var vm = new JsonResponseModel {Errors = validationState.Errors};
            vm.Success = true;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [RequiresAuthentication]
        public ActionResult SubmitTitle(TitleInputModel inputModel)
        {
            var isPrivate = inputModel.PublishStatus == PublishStatus.Group;
            var title = new Title
                          {
                              DateCreated = DateTime.UtcNow,
                              StoryTitle = inputModel.Title,
                              Synopsis = inputModel.Synopsis,
                              PublishStatus = inputModel.PublishStatus,
                              Password = inputModel.Password,
                              Genre = inputModel.Genre,
                              User = CurrentUser,
                              IsPrivate = isPrivate

                          }; 
          

          
            var validationState = new ModelStateWrapper(ModelState);
            var vm = new JsonResponseModel();
            // If both the view model and the user object are valid
            if (_storyBookService.ValidateTitle(title, validationState))
            {
                
                _storyBookService.CreateNewTitle(title);
                vm.Success = true;
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            vm.Errors = validationState.Errors;
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        

        [RequiresAuthentication]
        public ActionResult InviteUsers(StoryBookInviteUserInputModel inputModel)
        {
            var validationState = new ModelStateWrapper(ModelState);
         
           
           
            var vm = new JsonResponseModel();
            var emailParams = new EmailParams();
            emailParams.Add("FirstName",CurrentUser.FirstName);
            emailParams.Add("LastName",CurrentUser.LastName);
            _emailService.Send(inputModel.Email, "Welcome!", "Invited.html", emailParams);
            vm.Errors = validationState.Errors;
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

    }
}
