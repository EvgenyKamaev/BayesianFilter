using BayesianFilter.Core.Models.Entity;
using System.Collections.Generic;

namespace BayesianFilter.Web.Models
{
    /// <summary>
    /// Модель ответа получения страницы исключений
    /// </summary>
    public class PageResponseModel
    {
        /// <summary>
        /// Список исключений
        /// </summary>
        public List<ExceptionsModel> Exceptions { get; set; }

        /// <summary>
        /// Текущая страница
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Всего страниц
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// Всего исключений
        /// </summary>
        public int TotalExceptionsCount { get; set; }
    }
}
