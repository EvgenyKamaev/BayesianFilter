using BayesianFilter.Core.Services.Interfaces;
using System;

namespace BayesianFilter.Core.Services
{
    public class BayesianClassifier : IBayesianClassifier
    {
        private readonly IBayesianContainer _container;
        private const int MaxKeywordsToEvaluate = 40;

        public BayesianClassifier(IBayesianContainer container)
        {
            _container = container;
        }

        public decimal IsSubjectSpam(string[] subject)
        {
            return GetSpamProbability(subject);
        }

        private decimal GetSpamProbability(string[] words)
        {
            var bayesianData = _container.BayesianData;

            double pSpam = 0.5d;

            KeywordComparison.TrainingSetHamToSpamRatio = (double)bayesianData.HamTrainingMessages / bayesianData.SpamTrainingMessages;

            KeywordComparison[] keywords = new KeywordComparison[words.Length];
            for (int n = 0; n < words.Length; n++)
            {
                long hcount = 0, scount = 0;
                if (bayesianData.Hams.ContainsKey(words[n])) hcount = bayesianData.Hams[words[n]];
                if (bayesianData.Spams.ContainsKey(words[n])) scount = bayesianData.Spams[words[n]];
                keywords[n] = new KeywordComparison(words[n], hcount, scount);
            }
            Array.Sort(keywords);
            Array.Reverse(keywords);

            var x = Math.Log(1.0d - pSpam) - Math.Log(pSpam);
            for (int n = 0; n < keywords.Length && n < MaxKeywordsToEvaluate; n++)
            {
                x += Math.Log(1.0d - keywords[n].SpamProbability) - Math.Log(keywords[n].SpamProbability);
            }

            var pow = (decimal)Math.Pow(Math.E, x);

            var result = (1m / (1m + pow));
            return result;
        }

        private class KeywordComparison : IComparable
        {
            public static double TrainingSetHamToSpamRatio { get; set; } = 1.0d;
            public static double s = 1.0d;
            public KeywordComparison(string word, long hammy, long spammy)
            {
                Word = word;
                Hammy = hammy;
                Spammy = spammy;
                SpamProbability = (spammy + s) / (spammy + s + (hammy / TrainingSetHamToSpamRatio + s));
                Usefulness = Math.Abs(0.5d - SpamProbability) * 2;
            }
            public string Word { get; private set; }
            public long Hammy { get; private set; }
            public long Spammy { get; private set; }
            public double SpamProbability { get; private set; }
            private double Usefulness { get; set; } = 0.0d;

            public int CompareTo(object obj)
            {
                return Usefulness.CompareTo((obj as KeywordComparison).Usefulness);
            }
        }
    }
}
