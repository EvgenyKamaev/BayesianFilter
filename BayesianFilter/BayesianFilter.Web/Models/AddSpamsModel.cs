using System.Collections.Generic;

namespace BayesianFilter.Web.Models
{
    /// <summary>
    /// Модель на добавление "черного" списка
    /// </summary>
    public class AddSpamsModel
    {
        /// <summary>
        /// Черный список
        /// </summary>
        public List<string> Spams { get; set; }
    }
}
