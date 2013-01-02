using System.Collections.Generic;
using DotFramework.Domain;
using DotFramework.Services.Validation;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            Errors = new List<ValidationError>();
        }
        public int CurrentUserId { get; set; }
        public string Publication { get; set; }
        public bool CanEdit { get; set; }
        public IList<ValidationError> Errors { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public double AccountBalance { get; set; }
        public double StoryPrice { get; set; }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }


    }
}