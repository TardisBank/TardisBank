namespace TardisBank.Client
{
    public class RegisterReqeust : IRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}