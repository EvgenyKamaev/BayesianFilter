using BayesianFilter.Core.Models;
using BayesianFilter.Core.Models.Entity;
using System.Collections.Generic;

namespace BayesianFilter.Core.Services.Interfaces
{
    public interface IBayesianService
    {
        CheckResponse Check(string subject);

        InsertResult AddSpams(List<string> spams);

        InsertResult AddHams(List<string> hams);

        ExceptionsModel AddException(string subject);

        Result DeleteException(int id);

        PageModel GetExceptionsPage(int page, int pageSize);
    }
}
