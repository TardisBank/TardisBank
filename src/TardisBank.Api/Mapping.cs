using E247.Fun;
using TardisBank.Dto;

namespace TardisBank.Api
{
    public static class Mapping
    {
        public static Login ToModel(
            this RegisterRequest registerRequest)
            => new Login
            {
                Email = registerRequest.Email,
                PasswordHash = Password.HashPassword(registerRequest.Password)
            };

        public static RegisterResponse ToDto(
            this Login login)
            => new RegisterResponse();
    }
}