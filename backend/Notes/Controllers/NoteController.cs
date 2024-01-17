using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Notes.Data;
using Notes.Models;

namespace Notes.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class NoteController : ControllerBase 
    {
        private ApplicationDbContext _context;

        public NoteController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [Authorize("AuthenticatedUser")]
        [HttpGet]
        public IActionResult GetAll()
        {
            string Token = HttpContext
                .Request
                .Headers["Authorization"]
                .ToString()
                .Replace(
                    "Bearer ",
                    ""
                );

            if (UserController.IsTokenRevoked(Token)) return Unauthorized("Token is revoked");

            string Username = User.Identity.Name;

            var AuthUser = _context.Users.First(
                User => User.Username == Username
            );

            return Ok(_context.Notes.Where(
                Note => Note.User.Id == AuthUser.Id  
            ).ToList());
        }

        [Authorize("AuthenticatedUser")]
        [HttpPost]
        public IActionResult Create(Note Note) 
        {
            string Token = HttpContext
                .Request
                .Headers["Authorization"]
                .ToString()
                .Replace(
                    "Bearer ",
                    ""
                );

            if (UserController.IsTokenRevoked(Token)) return Unauthorized("Token is revoked");

            string Username = User.Identity.Name;

            var AuthUser = _context.Users.First(
                User => User.Username == Username
            );

            Note.User = AuthUser;

            _context.Notes.Add(Note);
            _context.SaveChanges();

            return Ok(Note);
        }

        [Authorize("AuthenticatedUser")]
        [HttpGet("{Id}")]
        public IActionResult Get(int Id) 
        {
            string Token = HttpContext
                .Request
                .Headers["Authorization"]
                .ToString()
                .Replace(
                    "Bearer ",
                    ""
                );

            if (UserController.IsTokenRevoked(Token)) return Unauthorized("Token is revoked");

            string Username = User.Identity.Name;

            var AuthUser = _context.Users.First(
                User => User.Username == Username
            );

            var Note = _context.Notes.First(
                Note => (
                    Note.Id == Id
                    && Note.User.Id == AuthUser.Id
                )
            );

            if (Note == null) return NotFound();

            return Ok(Note);
        }

        [Authorize("AuthenticatedUser")]
        [HttpPut("{Id}")]
        public IActionResult Update(int Id, Note NewNote) 
        {
            string Token = HttpContext
                .Request
                .Headers["Authorization"]
                .ToString()
                .Replace(
                    "Bearer ",
                    ""
                );

            if (UserController.IsTokenRevoked(Token)) return Unauthorized("Token is revoked");

            string Username = User.Identity.Name;

            var AuthUser = _context.Users.First(
                User => User.Username == Username
            );

            var Note = _context.Notes.First(
                Note => (
                    Note.Id == Id
                    && Note.User.Id == AuthUser.Id
                )
            );

            if (Note == null) return NotFound();

            Note.Title = NewNote.Title;
            Note.Body = NewNote.Body;

            _context.SaveChanges();

            return NoContent();
        }

        [Authorize("AuthenticatedUser")]
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id) 
        {
            string Token = HttpContext
                .Request
                .Headers["Authorization"]
                .ToString()
                .Replace(
                    "Bearer ",
                    ""
                );

            if (UserController.IsTokenRevoked(Token)) return Unauthorized("Token is revoked");

            string Username = User.Identity.Name;

            var AuthUser = _context.Users.First(
                User => User.Username == Username
            );

            var Note = _context.Notes.First(
                Note => (
                    Note.Id == Id
                    && Note.User.Id == AuthUser.Id
                )
            );

            if (Note == null) return NotFound();

            _context.Notes.Remove(Note);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
