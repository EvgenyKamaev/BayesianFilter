using BayesianFilter.Core.Services.Interfaces;
using BayesianFilter.Web.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BayesianFilter.Web.Services
{
    public class HostConfig : IHostConfig, ICoreConfig
    {
        public string ConnectionString { get { return _configuration.GetConnectionString("Pos"); } }

        private readonly IConfiguration _configuration;

        public HostConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
