namespace TardisBank.Dto
{
    public class AccountResponseCollection : ResponseModelBase
    {
        public AccountResponse[] Accounts { get; set; }
    }
}