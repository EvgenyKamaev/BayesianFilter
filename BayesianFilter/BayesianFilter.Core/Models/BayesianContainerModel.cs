using System.Collections.Generic;

namespace BayesianFilter.Core.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class BayesianContainerModel
    {
        public long HamTrainingMessages { get; set; }

        public long SpamTrainingMessages { get; set; }

        public Dictionary<string, long> Spams { get; set; }

        public Dictionary<string, long> Hams { get; set; }
    }
}
