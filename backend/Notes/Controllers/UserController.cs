using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Notes.Models;
using Notes.Data;

namespace Notes.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase 
    {
        private ApplicationDbContext _context;

        private static string[] RevokedTokens = [];

        public UserController(ApplicationDbContext context) 
        {
            _context = context;
        }

        private string HashPassword(string Password)
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(Password)));
            }
        }

        private string GenerateToken(User User)
        {
            var claims = new List<Claim> {
                new Claim(
                    ClaimTypes.Name, 
                    User.Username
                )
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ERT897ERT79ERTER89TTR8E797TE9T87ERTTR0"));
    
            var creds = new SigningCredentials(
                key, 
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public static bool IsTokenRevoked(string token)
        {
            return RevokedTokens.Contains(token);
        }

        private void RevokeToken(string token)
        {
            RevokedTokens.Append(token);
        }

        [HttpPost("Signup")]
        public IActionResult Signup(User User)
        {
            if (_context.Users.Any(
                ExistingUser => ExistingUser.Username == User.Username
            )) return BadRequest("Username already exists");

            User.Password = HashPassword(User.Password);

            _context.Users.Add(User);
            _context.SaveChanges();

            var token = GenerateToken(User);

            return Ok(new { 
                token 
            });
        }

        [HttpPost("Login")]
        public IActionResult Login(User User)
        {
            var AuthenticatedUser = _context.Users.Single(
                ExistingUser => (
                    ExistingUser.Username == User.Username 
                    && ExistingUser.Password == HashPassword(User.Password)
                )
            );

            if (AuthenticatedUser == null) return Unauthorized("Invalid username or password");

            var token = GenerateToken(AuthenticatedUser);

            return Ok(new { 
                token 
            });
        }

        [Authorize("AuthenticatedUser")]
        [HttpPost("Logout")]
        public IActionResult Logout() 
        {
            string Token = HttpContext
                .Request
                .Headers["Authorization"]
                .ToString()
                .Replace(
                    "Bearer ", 
                    ""
                );

            if (IsTokenRevoked(Token)) return Unauthorized("Token is revoked");

            RevokeToken(Token);

            return NoContent();
        }
    }
}
