using BeltExam.Models;

using Microsoft.EntityFrameworkCore;
 
namespace BeltExam.Models
{
    public class contextContext : DbContext
    {
        
        public contextContext(DbContextOptions<contextContext> options) : base(options) { }
        public DbSet<Like> likes { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Post> posts { get; set; }
        
    }
}
