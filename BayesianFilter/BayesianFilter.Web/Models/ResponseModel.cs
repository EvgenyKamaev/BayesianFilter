namespace BayesianFilter.Web.Models
{
    /// <summary>
    /// Базовый класс для ответа
    /// </summary>
    public class ResponseModel
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
