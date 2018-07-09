namespace TardisBank.Dto
{
    public class LoginRequest : IRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}