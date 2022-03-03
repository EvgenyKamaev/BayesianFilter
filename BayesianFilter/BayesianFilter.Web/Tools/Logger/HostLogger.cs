using BayesianFilter.Core.Logger;
using Microsoft.Extensions.Logging;

namespace BayesianFilter.Web.Tools.Logger
{
    public class HostLogger : IHostLogger, ICoreLogger
    {
        private readonly ILogger<HostLogger> logger;

        public HostLogger(ILogger<HostLogger> logger)
        {
            this.logger = logger;
        }

        public void Error(string message)
        {
            this.logger.LogError(message);
        }

        public void Info(string message)
        {
            this.logger.LogInformation(message);
        }

        public void Warn(string message)
        {
            this.logger.LogWarning(message);
        }
    }
}
