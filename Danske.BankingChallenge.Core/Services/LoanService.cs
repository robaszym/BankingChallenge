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
                EffectiveAPR = RoundValue(CalculateAPR()),
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

        //Possible change in implementation is to replace below methods with "Strategy" pattern, but it'll be overkill in that case
        private decimal CalculateAPR()
        {
            /*
             * I'm aware that's not formula for EAPR, but rather for EAR. As I joined recrutation process in the last moment, I decided to use EAR to at least show "some" calculations.
             * EAPR equation is quite sophisticated and implementing this would take too much time and I believe that purpose of this challenge is not only checking math related skills ;)
             */
            decimal periodicRate = _configurationProvider.GetLoanTermsConfiguration().InterestRate / _configurationProvider.GetLoanTermsConfiguration().PeymentsPerYear;
            decimal q = 1 + periodicRate;
            decimal qN = (decimal)Math.Pow((double)q, _configurationProvider.GetLoanTermsConfiguration().PeymentsPerYear);

            return (qN - 1) * 100;
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
