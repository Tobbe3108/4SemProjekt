using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Auth.Application.Common.Interfaces;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private const string xmlKey =
            "<RSAKeyValue>" +
            "<Modulus>p9ZX2CSot2aHOiIRJJz0lngezY51Z+stl/sMYGFD1rxcYZbuHDs/cZgUURDhxdlkGoLGv5VSVSyecJ15LIDsjkaKeZ5HJOT5TXVXQOtvtq8Wm/gPsOZso0qoxNIswKwEAsHclfaNOQ7zi3yvVv04Wq3AnhC6y2u/I7YhZUIZtW9oy1BWKnP+HS0PUlP+EhCSmcCro76kWNTQn0Y9lv9ouJqrlOuGmjBEobCyGXISQYfitCTMFZXTcFv9k5F8Y3Kq7FIjAakAjX90rUzl5JxY81Q+8xeOT7zzXn+CrqGuFvlQ0+QrIJLylUOf/x6OguBHlfco682RIqReVFGRwPU+db77OUlj7Yazq1s5X2aRUFn+dRIo/x7+iEin+b1OeA8JycjCrk6bqkttGpy4rKYGuZfoheRwUoJdI8KnuWwWg7D5VbxCh0TX8l9aSczQCryHNN0YZtVDbxRhU/HdOgHSzTAzKsQ8O/fJwgGcaEZs/JH3AS9BGmfurYXZbpiMnkoBEvZpe1pd64GeRenaaCnL2UYFu96Bbb/IUW62foh78+T/leuY1buTLlsiYHAu2fmZw7FBiaPa+RSJ6WXO/sPG/aFPk3AgZx6xX/9tY7Zo1UJ4BWyNw3tpxM+NTu49y9rdiaJ1hdZscPfFACpt/VFFKolgMcVauqV+OvVBZO3ZrsE=</Modulus>" +
            "<Exponent>AQAB</Exponent>" +
            "<P>81fIBUT8ssqQPhLUaOy/qgyC5EIGH7rEAHEa+LnmWETJLhFLFWYSdD8kpNiMoov+iecWKKT22+87r/DukE9M7Zg7vtxWCiZN8KmXz7mhXb53diT6Bl/XUyPWCSx3uFV4ipp7wuKw1bMBkopPpHwetQS2KqSJXa2qZMeU5MANVIKZJ9Y1BVfmiAG804OP0cdHSscPtXOmfWngYAvvDGZ784HFXbeslErBxNH0jBC5k1jMJf7ZDbyOVZnaTrOgxxoAM/IwBadbAKvLkrd7LKwdm+MG7GKzwO33yddo99ybOVWK+tUe/azDvLj9ZwpOBp4KP/CmcTb7QqKqDmtgzYB2eQ==</P><Q>sJErxkliJN7K8XVph/H0BhukUf+tlvyS6WBrNyhHa1Hx0naKaeTQLOu/ZkLRI164o3O81oFo9H5OJJGw3YoxFISdyNacgOm91tvl8jqFMBaoWQspn72a0MigCkMCKg7pQ6niaT5c67fHD2lERe1V4pYUTVawSqb+n6JH2ItQEcgweq/4MopcbLs/B+MgiFK2CWT9naOlX5BBhPJhDkLoyzN9TC/gOjYx6CMTv4XDwc+4hzcwKEQt8W6+jKZF91dnIWyGe2ZcyEkaQGcLYwI9LXMw0+7jCsTXmx0K5Loh9XUwNQg7HEk6utqhBQpWScrh+TGHfGuK3qPtATQqW/qIiQ==</Q>" +
            "<DP>AwaxbxdXiSWpu4viZlejXor/SbVkfBqHe3XsnvE44xof4lnGtEJrslRisUYcIZ1aEf70sJa0lzGXbW2ymcrSqxW8kHgO/dtSFs8VGzk0v4Sx2Z9GGLQyak9ExyJtbrZpZFfdeCP3jQDH4TmlGeeNPjULyuX261pQhyZQPLQCtm1VWEB6slFQhZp37a1yKWqwAUcOc/Q3OtIJP0iGtYHEwtd3S94P4Fw5oqf1wZQnqqRtDX/o1RnmLzxVkar8Md2p6Pt3C2r0X8LDKlTrzDdruteM632ivKB492KdZI+ywnjE3K+vzfnLCVrHk/N12vE2MWtdtoS3hTGeqcU+E/AoCQ==</DP>" +
            "<DQ>g9w2c8Oz7tMPfllv8V2JHtKf2bj2u45ubKlcCHolNAmjbcinP8Poj9OTQdxecWq4uyChAxTr+8Mjun2vNxOP6cHMUAOuJzvoUUAxrr9zXeFd4fnLIQepSYi6tE8sm5o9LBIACxbOsB60I1c67Hn9LNVl3ggCag3ik/bObvJDXrRBOC6YTa40reL8hHhjgEs8tTFdkc93njGOLKoSP2NKlQ4j4bTTYMW+aZ1gjyRneCMXrKtTZI7ePLK2zTUThU8ZrsVsfVWJru2hFD2gLFWsuHeRHIUHu7AbUDTVQNGcsMHOuZwnAM3TdV326Fu66yr/rWkGxK1PrUuAoOiDReOI+Q==</DQ>" +
            "<InverseQ>l8EMEOliuTdWYESOhjVYWm8TdKT4uh6jAIgIVZbRl5xOO/zX/wBYHN8cnyIIArMLB8N/UP/95HuLrqIQe3c8Mg0EfpknAxFDM6iPpM91GelurfBf9F0lIMA1ACgIvjH5cMycCmSJuqGHMw7U34dwK8qzmFU08vF2nBR20VEvxsSIjKr9hWdGuJM+zLnjk6OjFUZ1NkMS+EpMbf/dpsifF9hMehHNU28GnJszc4D4/B7E7yxdrMuD3Kf5ItjYzNraB+lHVB3KAbkcExgQOa6jTRVK662KEJNZ86cKudHVabGJMmY2M/Jf5F3DB/pZy6hgZ91EWHQ+VIxoCIAXk7fcLQ==</InverseQ>" +
            "<D>PCbV1OFOlmxoiq0q97RI9KSYZZNpAJWJAa84AFLbvVpEwbUWIYgadNvMbEFgqUgAWIAGOyOJHn2fSz0k8I5EObmOZ4gSd2aY8unbG6lFoHOf5sZA2WW8Ccn4MpIg9+yug053gEec1ZZGMve837BhQh6DqiLUz5MOLf5YebH3BM+o/zULrKtlp48+roLrzA2kfPZQjZ8Fx9B99Jd3+2YOYhsM45KF6zzhXBG3wubfJB1PYsDNY+8OmHZfq0O5EOtH1uqZIT8ws4W6i2HEAusF9i97YCRRHMLbdkK801meyx0PmuFrk4EW7+gMr6KD8OPwxA6Q2/IQ0hmWtSGG22EJwRLyuXUf1MNnLtR550QHf55e8AigHKaC3W7f7QoChZ4E3odkj8+9Y9HhaS1G873eS/9kSXljx1d+R+zR3z7/H8pLuzgbPXJX8oz39yjtJhqWOHWDiJCdTMnhGKNfMjwRlurCaxEYrLdP0pA2MeXIXjOcWP/WS3Zf/L1m/x9Vg6wY6K7oCFjTeNUE6UXEs1v8opdLLQNKRlrDCsaQ/BLgI+3VNYGVyE0u+1jxZ2W2NX26GJuag6zbSm9cqUD2NmWzBuqwqH4GdFCF/ENlAF2g98PyGYRhDKLtL4fEwL7S4K9LDjKD1yj0SpUnHaDa5MsqJCseDsKBglDgoBzspN98EAE=</D>" +
            "</RSAKeyValue>";

        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }
        
        public string GenerateJsonWebToken(User user)
        {
            SecurityKey key = KeyService.BuildRsaSigningKey(xmlKey);

            var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            List<string> roles = new List<string>();

            foreach (var userRole in user.UserRoles)
            {
                roles.Add(userRole.Role.RoleName);
            }

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
        }
    }
}