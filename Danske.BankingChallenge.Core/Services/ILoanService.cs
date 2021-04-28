using Danske.BankingChallenge.Core.Model;

namespace Danske.BankingChallenge.Core.Services
{
    public interface ILoanService
    {
        PaymentOverview GetPaymentOverview(Loan loan);
    }
}
