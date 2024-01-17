using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notes.Models;

namespace Notes.Data
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;    

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration; 
        }

        public DbSet<User> Users 
        {
            get;
            set;
        }

        public DbSet<Note> Notes
        {
            get;
            set;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
