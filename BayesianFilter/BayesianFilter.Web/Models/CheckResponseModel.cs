namespace BayesianFilter.Web.Models
{
    /// <summary>
    /// Модель ответа проверки строки
    /// </summary>
    public class CheckResponseModel
    {
        /// <summary>
        /// Значение проверки
        /// </summary>
        public decimal Result { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Message { get; set; }
    }
}
