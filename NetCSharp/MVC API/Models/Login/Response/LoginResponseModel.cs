using System.Collections.Generic;
using DotFramework.Domain;


namespace DotFramework.Web.Mvc.Api.Models
{
    public class LoginResponseModel :BaseViewModel
    {
        public string Role { get; set; }
        public string AuthToken { get; set; }
        public double AccountBalance { get; set; }
        public IList<UserBadge> UserBadges { get; set; }
        public int PurchasedTitleCount { get; set; }
        public LoginResponseModel()
        {
            UserBadges = new List<UserBadge>();
        }
    }
}