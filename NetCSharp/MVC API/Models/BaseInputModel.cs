using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotFramework.Domain.Enumerable;

namespace DotFramework.Web.Mvc.Api.Models
{
    public class BaseInputModel
    {
        public int? Id { get; set; }
        public int? Count { get; set; }
        public int? Page { get; set; }
        public Genre? Genre { get; set; }
    }
}