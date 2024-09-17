namespace EmailService.API.Models
{
    public class EmailOptions
    {
        public string Service { get; set; } = null!;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
