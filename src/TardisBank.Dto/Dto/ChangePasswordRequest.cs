namespace TardisBank.Dto
{
    public class ChangePasswordRequest : IRequestModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}