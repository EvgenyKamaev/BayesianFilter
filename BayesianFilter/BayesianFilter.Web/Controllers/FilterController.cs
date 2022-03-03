using BayesianFilter.Core.Services.Interfaces;
using BayesianFilter.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BayesianFilter.Web.Controllers
{
    /// <summary>
    /// Контроллер фильтра
    /// </summary>
    [Route("api/filter")]
    [ApiController]
    public class FilterController : Controller
    {
        private readonly IBayesianService _bayesianService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="bayesianService"></param>
        public FilterController(IBayesianService bayesianService)
        {
            _bayesianService = bayesianService;
        }

        /// <summary>
        /// Проверка строки по алгоритму Байеса
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        [HttpGet("check")]
        public CheckResponseModel Check(string targetString)
        {
            var result = _bayesianService.Check(targetString);

            return new CheckResponseModel()
            {
                Message = result.Message,
                Result = result.Result,
            };
        }

        /// <summary>
        /// Добавить в "черный" список
        /// </summary>
        /// <param name="spams"></param>
        /// <returns></returns>
        [HttpPost("addSpams")]
        public InsertResponseModel AddSpams(AddSpamsModel spams)
        {
            var result = _bayesianService.AddSpams(spams?.Spams);

            return new InsertResponseModel()
            {
                Inserted = result?.Inserted
            };
        }

        /// <summary>
        /// Добавить в "белый" список
        /// </summary>
        /// <param name="hams"></param>
        /// <returns></returns>
        [HttpPost("addHams")]
        public InsertResponseModel AddHams(AddHamsModel hams)
        {
            var result = _bayesianService.AddHams(hams.Hams);

            return new InsertResponseModel()
            {
                Inserted = result?.Inserted
            };
        }

        /// <summary>
        /// Добавить в список исключений
        /// </summary>
        /// <returns></returns>
        [HttpGet("addException")]
        public ExceptionResponseModel AddException(string subject)
        {
            var result = _bayesianService.AddException(subject);

            return new ExceptionResponseModel()
            {
                Id = result?.Id,
                CreatedDate = result?.CreatedDate,
                Subject = subject,
            };
        }

        /// <summary>
        /// Удалить из списка исключений
        /// </summary>
        /// <returns></returns>
        [HttpGet("deleteException")]
        public ResponseModel DeleteException(int id)
        {
            var result = _bayesianService.DeleteException(id);

            return new ResponseModel()
            {
                StatusCode = result.StatusCode,
                Message = result.Message,
            };
        }

        /// <summary>
        /// Постраничный вывод исключений
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        [HttpGet("getExceptionsPage")]
        public PageResponseModel GetExceptionsPage(int page, int pageSize)
        {
            var result = _bayesianService.GetExceptionsPage(page, pageSize);

            return new PageResponseModel()
            {
                Exceptions = result.Exceptions,
                Page = result.Page,
                TotalExceptionsCount = result.TotalExceptionsCount,
                TotalPageCount = result.TotalPageCount
            };
        }
    }
}
