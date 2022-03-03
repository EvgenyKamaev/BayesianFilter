namespace BayesianFilter.Web.Tools.Logger
{
    public interface IHostLogger
    {
        void Info(string message);

        void Warn(string message);

        void Error(string message);
    }
}
