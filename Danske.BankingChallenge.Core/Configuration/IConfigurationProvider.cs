namespace Danske.BankingChallenge.Core.Configuration
{
    public interface IConfigurationProvider
    {
        LoanTermsConfiguration GetLoanTermsConfiguration();
    }
}
