using Authentication_Servie.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Servie.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } 

        public DbSet<User> Users { get; set; }
    }
}
