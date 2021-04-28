using Danske.BankingChallenge.Core.Configuration;
using Danske.BankingChallenge.Core.Model;
using Danske.BankingChallenge.Core.Services;
using Moq;
using NUnit.Framework;
using System;

namespace Danske.BankingChallenge.Tests
{
    public class Tests
    {
        private ILoanService _loanService;

        [SetUp]
        public void Setup()
        {
            var configurationProviderMock = new Mock<IConfigurationProvider>();
            configurationProviderMock.Setup(m => m.GetLoanTermsConfiguration()).Returns(new LoanTermsConfiguration());

            _loanService = new LoanService(configurationProviderMock.Object);
        }

        [Test]
        public void Should_Throw_When_LoanIsNull()
        {
            var loan = new Loan();

            Assert.Throws<ArgumentNullException>(() => _loanService.GetPaymentOverview(null));
        }

        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-100)]
        public void Should_Throw_When_AmountLessThanZero(decimal amount)
        {
            var loan = new Loan()
            {
                Amount = amount,
                Duration = 10
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => _loanService.GetPaymentOverview(loan));
        }

        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-100)]
        public void Should_Throw_When_DurationLessThanZero(int duration)
        {
            var loan = new Loan()
            {
                Amount = 10000,
                Duration = duration
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => _loanService.GetPaymentOverview(loan));
        }

        [Test]
        public void Should_ReturnLimitedAdministrationFee()
        {
            var configurationProviderMock = new Mock<IConfigurationProvider>();
            configurationProviderMock.Setup(m => m.GetLoanTermsConfiguration()).Returns(new LoanTermsConfiguration()
            {
                AdministrationFeeAmount = 2000,
                AdministrationFeeRate = 0.1m
            });

            var loanService = new LoanService(configurationProviderMock.Object);
            var loan = new Loan()
            {
                Amount = 50000,
                Duration = 10
            };
                       

            //AdministrationFeeRate * loan.Amount = 0.1 * 50000 = 5000
            //AdministrationFeeAmount = 2000
            Assert.AreEqual(2000, loanService.GetPaymentOverview(loan).AdministrativeFeesTotal);
        }

        [Test]
        public void Should_CalculateCorrectEAPR()
        {
            var loan = new Loan()
            {
                Amount = 500000,
                Duration = 120
            };

            Assert.AreEqual(2000, _loanService.GetPaymentOverview(loan).EffectiveAPR);
        }

        [Test]
        public void Should_CalculateCorrectMonthlyCost()
        {
            var loan = new Loan()
            {
                Amount = 500000,
                Duration = 120
            };

            Assert.AreEqual(5303.28m, _loanService.GetPaymentOverview(loan).MonthlyCost);
        }

        [Test]
        public void Should_CalculateCorrectInterestRateTotal()
        {
            var loan = new Loan()
            {
                Amount = 500000,
                Duration = 120
            };

            Assert.AreEqual(136393.09m, _loanService.GetPaymentOverview(loan).InterestRateTotal);
        }
    }
}