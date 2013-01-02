using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DotFramework.Domain;
using DotFramework.Infrastructure;
using DotFramework.Services.EmailService;
using DotFramework.Services.UserService;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;


namespace DotFramework.Web.Mvc.Controllers.API
{
    [OptionalAuthorize]
    public class BaseController : ApiController
    { 
        private static IUserService _userService;
        public static User CurrentUser { get; set; }
        public  BaseController(IUserService userService)
        {

            _userService = userService;
        }

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            var user = (HttpContextWrapper)controllerContext.Request.Properties["MS_HttpContext"];
           if(user.User.Identity.IsAuthenticated)
           {
               var username = user.User.Identity.Name;
               CurrentUser = _userService.GetUserByEmail(username);
           }

        }


       


        public class OptionalAuthorizeAttribute : AuthorizeAttribute
        {
            private readonly bool _authorize;

            public OptionalAuthorizeAttribute()
            {
                _authorize = true;
            }

            public OptionalAuthorizeAttribute(bool authorize)
            {
                _authorize = authorize;
               
            }

            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                if (!_authorize)
                    return true;
               
                return base.AuthorizeCore(httpContext);
            }
        }
    }
}


