using System.Configuration;
using System.Web.Http;
using DotFramework.Infrastructure;
using DotFramework.Services;
using DotFramework.Services.ContentService;
using DotFramework.Services.EmailService;
using DotFramework.Services.Image_Service;
using DotFramework.Services.StoryBookService;
using DotFramework.Services.UserService;
using DotFramework.Web.Mvc.Api;
using DotFramework.Web.Mvc.Providers;
using Microsoft.Practices.Unity;

namespace DotFramework.Web.Mvc.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var unity = new UnityContainer();
            //Controllers
            unity.RegisterType<LoginController>();
            unity.RegisterType<StoryController>();
            unity.RegisterType<GenreController>();
            unity.RegisterType<UserController>();
            unity.RegisterType<ReviewController>();

            //Services
            //
            //StoryService
            unity.RegisterType<IStoryService, StoryService>(
                new HierarchicalLifetimeManager());
            //UserService
            unity.RegisterType<IUserService, UserService>(
               new HierarchicalLifetimeManager());
            //IAuth
            unity.RegisterType<IAuth, FormsAuthenticationWrapper>(
               new HierarchicalLifetimeManager());
            //EmailService
            unity.RegisterType<IEmailService, EmailService>("", new InjectionConstructor(ConfigurationManager.AppSettings["EmailTemplateRoot"]));
            

            //ReviewService
            unity.RegisterType<IReviewService, ReviewService>(
               new HierarchicalLifetimeManager());
            //ImageAudion
            unity.RegisterType<IImageAudioService, IImageAudioService>(
               new HierarchicalLifetimeManager());
   
            //
            //Repository
            //
            //Story
            unity.RegisterType<IStoryRepository, StoryRepository>(
                new HierarchicalLifetimeManager());
            //StoryAutoSave
            unity.RegisterType<IStoryAutoSaveRepository, StoryAutoSaveRepository>(
                new HierarchicalLifetimeManager());
            //Purchased
            unity.RegisterType<IPurchasedStoryRepository, PurchasedStoryRepository>(
                new HierarchicalLifetimeManager());
            //Content
            unity.RegisterType<IStoryContentRepository, StoryContentRepository>(
                new HierarchicalLifetimeManager());
            //User
            unity.RegisterType<IUserRepository, UserRepository>(
                new HierarchicalLifetimeManager());
            //Review
            unity.RegisterType<IReviewRepository, ReviewRepository>(
              new HierarchicalLifetimeManager());
            //ChildReview
            unity.RegisterType<IReviewChildCommentRepository, ReviewChildCommentRepository>
                (new HierarchicalLifetimeManager());
            //StoryVote
            unity.RegisterType<IStoryVoteRepository, StoryVoteRepository>
                (new HierarchicalLifetimeManager());
            //ReviewVote
            unity.RegisterType<IReviewVoteRepository, ReviewVoteRepository>
                (new HierarchicalLifetimeManager());
            //ChildReviewVote
            unity.RegisterType<IReviewChildCommentVoteRepository, ReviewChildCommentVoteRepository>
                (new HierarchicalLifetimeManager());
            //ChildReview
            unity.RegisterType<IReviewChildCommentRepository, ReviewChildCommentRepository>
                (new HierarchicalLifetimeManager());
            //CoverArt
            unity.RegisterType<ICoverArtRepository, CoverArtRepository>
                (new HierarchicalLifetimeManager());
            //AudioBook
            unity.RegisterType<IAudioBookRepository, AudioBookRepository>
                (new HierarchicalLifetimeManager());


            configuration.DependencyResolver = new IoCContainer(unity);
            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "Api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}