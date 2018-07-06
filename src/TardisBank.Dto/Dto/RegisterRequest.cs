namespace TardisBank.Dto
{
    public class RegisterRequest : IRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}