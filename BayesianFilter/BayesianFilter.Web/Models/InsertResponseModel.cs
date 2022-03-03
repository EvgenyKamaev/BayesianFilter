namespace BayesianFilter.Web.Models
{
    /// <summary>
    /// Модель ответа добавления "черного/белого" списка
    /// </summary>
    public class InsertResponseModel
    {
        /// <summary>
        /// Кол-во добавленных записей
        /// </summary>
        public int? Inserted { get; set; }
    }
}
