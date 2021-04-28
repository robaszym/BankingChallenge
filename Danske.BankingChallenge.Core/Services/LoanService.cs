using Danske.BankingChallenge.Core.Configuration;
using Danske.BankingChallenge.Core.Model;
using System;

namespace Danske.BankingChallenge.Core.Services
{
    public class LoanService : ILoanService
    {
        private readonly IConfigurationProvider _configurationProvider;

        public LoanService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public PaymentOverview GetPaymentOverview(Loan loan)
        {
            ValidateLoan(loan);

            return new PaymentOverview
            {
                EffectiveAPR = CalculateAPR(loan),
                MonthlyCost = RoundValue(CalculateMonthlyCost(loan)),
                InterestRateTotal = RoundValue(CalculateInterestRateTotal(loan)),
                AdministrativeFeesTotal = RoundValue(CalculateAdministrativeFeesTotal(loan))
            };
        }

        private void ValidateLoan(Loan loan)
        {
            if (loan == null)
            {
                throw new ArgumentNullException("Loan cannot be null!");
            }

            if (loan.Amount <= 0)
            {
                throw new ArgumentOutOfRangeException("Loan amount must be greater than 0!");
            }

            if (loan.Duration <= 0)
            {
                throw new ArgumentOutOfRangeException("Loan duration must be greater than 0");
            }
        }

        private decimal CalculateAPR(Loan loan)
        {
            return 0;
        }

        private decimal CalculateMonthlyCost(Loan loan)
        {
            //formula after transformations taken from https://www.thebalance.com/loan-payment-calculations-315564#how-do-you-calculate-loan-payments
            decimal periodicRate = _configurationProvider.GetLoanTermsConfiguration().InterestRate / _configurationProvider.GetLoanTermsConfiguration().PeymentsPerYear;
            decimal q = 1 + periodicRate;
            decimal qN = (decimal)Math.Pow((double)q, loan.Duration);

            return loan.Amount * qN * (q - 1) / (qN - 1); 
        }

        private decimal CalculateInterestRateTotal(Loan loan)
        {
            return CalculateMonthlyCost(loan) * loan.Duration - loan.Amount;
        }

        private decimal CalculateAdministrativeFeesTotal(Loan loan)
        {
            return Math.Min(loan.Amount * _configurationProvider.GetLoanTermsConfiguration().AdministrationFeeRate, _configurationProvider.GetLoanTermsConfiguration().AdministrationFeeAmount);
        }

        private decimal RoundValue(decimal value) => Math.Round(value, 2);
    }
}
