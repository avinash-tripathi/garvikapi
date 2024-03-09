using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GARVIKService.Middleware
{
    public class JwtTokenGenerator
    {
        public static string GenerateToken()
        {
            // Set the secret key used to sign the token
            var secretKey = "JLeQo6xDglrCNJdZk8AvheDgsGwl5Vm8";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Create token credentials
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define the claims for the token
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "Avinash Tripathi"),
            new Claim(ClaimTypes.Email, "avinash.tripathi@garvik.com"),
            // Add more claims as needed
        };

            // Define token's expiration date
            var expiration = DateTime.UtcNow.AddHours(1);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: "garvik.azurewebsites.net",
                audience: "QSCEGNTJ",
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            // Serialize the token to a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
