namespace BayesianFilter.Core.Logger
{
    public interface ICoreLogger
    {
        void Info(string message);

        void Warn(string message);

        void Error(string message);
    }
}
