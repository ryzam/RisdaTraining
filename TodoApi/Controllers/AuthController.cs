using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            // This is a simple example. In a real application, you should validate the user's credentials from a database
            if (user.Username == "test" && user.Password == "password")
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your_Secret_Key_12345678910111213"));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, "test token"),
                        new Claim(JwtRegisteredClaimNames.Name, "test"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
                var token = new JwtSecurityToken(
                issuer: "https://localhost:7289",
                audience: "https://localhost:7289",
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                 return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }
    }
}
