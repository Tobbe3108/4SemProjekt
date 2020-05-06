using System;
using System.Security.Cryptography;
using Auth.Application.Common.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Services
{
    public class HashService : IHashService
    {
        public PasswordVerificationResult Compare(string password, string dbHash, string salt)
        {
            string hashedPassword = GenerateHash(password, salt);

            if (string.Equals(hashedPassword, dbHash))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }

        public string GenerateHash(string password, string salt)
        {
            string hashedPassword =
                Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Convert.FromBase64String(salt),
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 512 / 8));

            return hashedPassword;
        }

        public string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}