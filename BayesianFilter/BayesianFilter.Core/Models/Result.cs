using System;
using System.Collections.Generic;
using System.Text;

namespace BayesianFilter.Core.Models
{
    /// <summary>
    /// Базовый класс для ответа
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Код статуса
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Message { get; set; }
    }
}
