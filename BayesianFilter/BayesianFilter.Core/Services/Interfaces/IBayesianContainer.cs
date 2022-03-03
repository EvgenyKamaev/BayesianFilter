using BayesianFilter.Core.Models;

namespace BayesianFilter.Core.Services.Interfaces
{
    public interface IBayesianContainer
    {
        void LoadData();

        BayesianContainerModel BayesianData { get; set; }
    }
}
