namespace BayesianFilter.Core.Models
{
    /// <summary>
    /// Модель ответа проверки
    /// </summary>
    public class CheckResponse
    {
        /// <summary>
        /// Результат
        /// </summary>
        public decimal Result { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}
