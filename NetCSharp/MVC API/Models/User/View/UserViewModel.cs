using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Domain;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class UserViewModel:BaseViewModel
    {
        private User _user;
        public UserViewModel(User user)
        {
            _user = user;
        }
        public string UserName { get { return _user.FullName; } }
        public int PurchasedStoryCount { get { return _user.PurchasedStories != null ? _user.PurchasedStories.Count : 0; } }
        public string Email { get { return _user.Email; } }
        public int CreatedStoryCount { get { return _user.Stories != null ? _user.Stories.Count : 0; } }
        public string Bio { get { return _user.Bio; } }
        public int  Age {get
        {
            if(_user.BirthDate.HasValue)
            {
                var now = DateTime.Today;
                var age = now.Year - _user.BirthDate.Value.Year;
                return _user.BirthDate.Value > now.AddYears(-age) ? age-- : age;
            }
            return 0;
        }}
        public string CoverArtUrl
        {
            get
            {
                if (_user.Image != null)
                {
                    return "/Image/ShowImage?imageid=" + _user.Id;
                }
                return "/Image/ShowImage?imageid=0";
            }
        }
        public int SoldStoryCount {get
        {
            return _user.Stories.Aggregate(0, (current, story) => current + story.Purchasers.Count);
        }
        }
       
    }
}