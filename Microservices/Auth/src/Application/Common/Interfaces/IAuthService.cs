namespace Auth.Application.Common.Interfaces
{
    public interface IAuthService
    {
        string GenerateJsonWebToken(Domain.Entities.AuthUser authUser);
    }
}