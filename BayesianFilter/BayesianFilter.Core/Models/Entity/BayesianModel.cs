using System;

namespace BayesianFilter.Core.Models.Entity
{
    /// <summary>
    /// Модель таблицы dbo.Bayesian
    /// </summary>
    public class BayesianModel
    {
        /// <summary>
        /// Id Записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Строка (spam or ham)
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Признак спама
        /// </summary>
        public bool IsSpam { get; set; }
    }
}
