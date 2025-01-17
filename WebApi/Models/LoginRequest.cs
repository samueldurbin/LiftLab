namespace WebApi.Models
{
    public class LoginRequest  // class specifically made for the login request
    {
        public string Username { get; set; }  // mandatory fields for login

        public string Password { get; set; }

    }
}
