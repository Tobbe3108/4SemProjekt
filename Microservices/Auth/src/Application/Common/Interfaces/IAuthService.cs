namespace Auth.Application.Common.Interfaces
{
    public interface IAuthService
    {
        string GenerateJsonWebToken(Domain.Entities.User user);
    }
}