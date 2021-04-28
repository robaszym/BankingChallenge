namespace Danske.BankingChallenge.Core.Model
{
    public class PaymentOverview
    {
        public decimal EffectiveAPR { get; set; }

        public decimal MonthlyCost { get; set; }

        public decimal InterestRateTotal { get; set; }

        public decimal AdministrativeFeesTotal { get; set; }
    }
}
