namespace BayesianFilter.Core.Models
{
    public class InsertModel
    {
        /// <summary>
        /// Строка
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Признак спама
        /// </summary>
        public bool IsSpam { get; set; }
    }
}
