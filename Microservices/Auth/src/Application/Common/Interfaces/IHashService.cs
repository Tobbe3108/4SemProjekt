using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Common.Interfaces
{
    public interface IHashService
    {
        PasswordVerificationResult Compare(string password, string dbHash, string salt);
        string GenerateHash(string password, string salt);
        string GenerateSalt();
    }
}