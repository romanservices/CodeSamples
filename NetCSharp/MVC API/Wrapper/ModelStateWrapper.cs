﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotFramework.Services.Validation;

namespace DotFramework.Web.Mvc.Api
{
    public class ModelStateWrapper : IValidationDictionary
    {
        private readonly System.Web.Http.ModelBinding.ModelStateDictionary _modelState;

        public ModelStateWrapper(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }

        #region IValidationDictionary Members

        public override void AddError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
        }

        public override bool IsValid
        {
            get { return _modelState.IsValid; }
        }

        public override IList<ValidationError> Errors
        {
            get 
            { 
                var errors = new List<ValidationError>();
                foreach(var key in _modelState.Keys)
                {
                    errors.AddRange(_modelState[key].Errors.Select(error => new ValidationError(key, error.ErrorMessage)));
                }
                return errors;
            }
        }

        public override void Remove(string propertyName)
        {
            var newErrorList = new List<ValidationError>();
            List<string> matchingErrors = _modelState.Keys.Where(errorKey => errorKey == propertyName).ToList();
            foreach (var errorKey in matchingErrors)
            {
                _modelState.Remove(errorKey);
            }
        }

        #endregion
    }

}