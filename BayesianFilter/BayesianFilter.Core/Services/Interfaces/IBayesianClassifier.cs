namespace BayesianFilter.Core.Services.Interfaces
{
    public interface IBayesianClassifier
    {
        decimal IsSubjectSpam(string[] subject);
    }
}
