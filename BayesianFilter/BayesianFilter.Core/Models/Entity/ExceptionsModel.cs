using System;

namespace BayesianFilter.Core.Models.Entity
{
    /// <summary>
    /// Модель таблицы dbo.Exceptions
    /// </summary>
    public class ExceptionsModel
    {
        /// <summary>
        /// Id Записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Строка
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
