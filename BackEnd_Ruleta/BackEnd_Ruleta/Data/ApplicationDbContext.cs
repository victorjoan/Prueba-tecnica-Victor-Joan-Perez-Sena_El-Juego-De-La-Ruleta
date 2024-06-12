using BackEnd_Ruleta.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd_Ruleta.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<usuario> usuarios { get; set; } 

    }
}
