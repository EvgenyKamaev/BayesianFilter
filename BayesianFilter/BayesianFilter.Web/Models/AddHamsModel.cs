using System.Collections.Generic;

namespace BayesianFilter.Web.Models
{
    /// <summary>
    /// Модель на добавление "белого" списка
    /// </summary>
    public class AddHamsModel
    {
        /// <summary>
        /// Белый список
        /// </summary>
        public List<string> Hams { get; set; }
    }
}
