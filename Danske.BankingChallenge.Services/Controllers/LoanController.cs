using Danske.BankingChallenge.Core.Model;
using Danske.BankingChallenge.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Danske.BankingChallenge.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet("Calculate")]
        public ActionResult<PaymentOverview> Calculate([FromQuery] Loan loan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _loanService.GetPaymentOverview(loan);

            return result;
        }
    }
}
