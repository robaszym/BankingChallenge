using System.ComponentModel.DataAnnotations;

namespace Danske.BankingChallenge.Core.Model
{
    public class Loan
    {
        [Required]
        [Range(0, int.MaxValue)]
        public decimal Amount { get; set; }
        
        [Required]
        [Range(1, 600)]     //50 years * 12 = 600 months
        public int Duration { get; set; }   //in months
    }
}
