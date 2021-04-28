namespace Danske.BankingChallenge.Core.Configuration
{
    public class LoanTermsConfiguration
    {
        public decimal InterestRate { get; set; } = 0.05m;
        public decimal AdministrationFeeRate { get; set; } = 0.01m;
        public decimal AdministrationFeeAmount { get; set; } = 10000m;

        public int PeymentsPerYear { get; set; } = 12;
    }
}
