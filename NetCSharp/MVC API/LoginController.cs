using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotFramework.Domain;
using DotFramework.Services.UserService;
using DotFramework.Web.Mvc.Api.Models;
using DotFramework.Web.Mvc.Controllers.API;
using DotFramework.Web.Mvc.Providers;
using DotFramework.Web.Mvc.Resources;

namespace DotFramework.Web.Mvc.Api
{
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuth _authentication;
        public LoginController(IAuth authentication, IUserService userService) : base(userService)
        {
            _authentication = authentication;
            _userService = userService;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [HttpPost]
        public LoginResponseModel Login(LoginInputModel inputModel)
        {
            var vm = new LoginResponseModel();
            var user = _userService.Authenticate(inputModel.Email, inputModel.Password);
            if (user != null)
            {
                _authentication.DoAuth(inputModel.Email, false);
                vm.Success = true;
                vm.Role = user.Role.ToString();
                vm.AuthToken = user.AuthToken;
                vm.AccountBalance = user.AccountBalance;
                vm.UserBadges = user.UserBadges;
                vm.PurchasedTitleCount = user.PurchasedStories.Count;
            }
            else
            {
                vm.Message = AppResources.InvalidCredentials;
                vm.Success = false;
            }
           

            return vm;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [OptionalAuthorizeAttribute(true)]
        public BaseViewModel AmILoggedIn()
         {
             return new BaseViewModel{Success = true};
         }


        
    }
}