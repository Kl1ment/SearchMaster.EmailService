using System.ComponentModel.DataAnnotations;

namespace EmailService.API.Contracts
{
    public class Email
    {
        [Required]
        public string Sender { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Message { get; set;} = null!;
    }
}
