using System;

namespace BayesianFilter.Web.Models
{
    /// <summary>
    /// Модель ответа добавления исключения
    /// </summary>
    public class ExceptionResponseModel
    {
        /// <summary>
        /// Id записи
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Добавленное значение
        /// </summary>
        public string Subject { get; set; }
    }
}
