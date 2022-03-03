using BayesianFilter.Core.Models.Entity;
using System.Collections.Generic;

namespace BayesianFilter.Core.Models
{
    public class PageModel
    {
        public List<ExceptionsModel> Exceptions { get; set; }

        public int Page { get; set; }

        public int TotalPageCount { get; set; }

        public int TotalExceptionsCount { get; set; }
    }
}
