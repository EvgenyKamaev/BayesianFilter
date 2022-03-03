using BayesianFilter.Core.Logger;
using BayesianFilter.Core.Models;
using BayesianFilter.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BayesianFilter.Core.Services
{
    public class BayesianContainer : IBayesianContainer
    {
        private readonly IBayesianRepository _bayesianRepository;
        private readonly ICoreLogger _logger;
        public BayesianContainerModel BayesianData { get; set; }

        public BayesianContainer(IBayesianRepository bayesianRepository, ICoreLogger logger)
        {
            _bayesianRepository = bayesianRepository;
            _logger = logger;
        }

        public void LoadData()
        {
            _logger.Info("Start loading bayesian data");

            var data = _bayesianRepository.GetAll();

            if (data == null)
                return;

            var spams = data.Where(d => d.IsSpam).Select(c => c.Subject).ToList();
            var hams = data.Where(d => !d.IsSpam).Select(c => c.Subject).ToList();

            long spamTrainingMessages = 0;
            long hamTrainingMessages = 0;

            Dictionary<string, long> subjectSpamKeywords = new Dictionary<string, long>();
            Dictionary<string, long> subjectHamKeywords = new Dictionary<string, long>();

            foreach (var spam in spams)
            {
                try
                {
                    var parts = SplitString(spam).ToArray();

                    TrainList(subjectSpamKeywords, parts[0].Split(','));
                    spamTrainingMessages++;
                }
                catch (Exception ex)
                {
                    _logger.Error($"BayesianContainer. Error get spams: {ex}");
                }
            }

            foreach (var ham in hams)
            {
                try
                {
                    var parts = SplitString(ham).ToArray();

                    TrainList(subjectHamKeywords, parts[0].Split(','));
                    hamTrainingMessages++;
                }
                catch (Exception ex)
                {
                    _logger.Error($"BayesianContainer. Error get hams: {ex}");
                }
            }

            _logger.Info("Finish loading bayesian data");

            this.BayesianData = new BayesianContainerModel()
            {
                Hams = subjectHamKeywords,
                Spams = subjectSpamKeywords,
                HamTrainingMessages = hamTrainingMessages,
                SpamTrainingMessages = spamTrainingMessages
            };
        }

        private List<string> SplitString(string str)
        {
            List<string> list = new List<string>();
            int i = 0;
            while (i < str.Length - 1)
            {
                list.Add(str.Substring(i, 2));
                i += 2;
            }
            return list;
        }

        private void TrainList(Dictionary<string, long> list, string[] words)
        {
            if (words == null) return;
            foreach (string s in words)
            {
                var str = s.ToLower();

                if (!list.ContainsKey(str)) list.Add(str, 1);
                else list[str] = list[str] + 1;
            }
        }
    }
}
