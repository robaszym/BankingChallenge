using Microsoft.Extensions.Options;

namespace Danske.BankingChallenge.Core.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IOptions<LoanTermsConfiguration> _options;

        public ConfigurationProvider(IOptions<LoanTermsConfiguration> options)
        {
            _options = options;
        }

        public LoanTermsConfiguration GetLoanTermsConfiguration()
        {
            LoanTermsConfiguration defaultTerms = new LoanTermsConfiguration();

            LoanTermsConfiguration configTerms = _options.Value;
            if (configTerms == null)
            {
                return defaultTerms;
            }

            if (configTerms.AdministrationFeeAmount < 0)
            {
                configTerms.AdministrationFeeAmount = 0;
            }

            if (configTerms.AdministrationFeeRate < 0)
            {
                configTerms.AdministrationFeeRate = 0;
            }

            if (configTerms.InterestRate < 0)
            {
                configTerms.InterestRate = defaultTerms.InterestRate;
            }

            if (configTerms.PeymentsPerYear < 1)
            {
                configTerms.PeymentsPerYear = defaultTerms.PeymentsPerYear;
            }
            

            return configTerms;
        }
    }
}
