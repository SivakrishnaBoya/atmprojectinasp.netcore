using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ATMPROCESS.Models
{
    public class RegisterDBContext:DbContext
    {
        public RegisterDBContext(DbContextOptions<RegisterDBContext> options) : base(options)
        {
            
        }
        
        public DbSet<MiniStatement> miniStatements { get; set; }
        public DbSet<Register> UserRegister { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Register>(entry =>
            {
                entry.ToTable("UserRegister", tb => tb.HasTrigger("tr_Ministate"));
            });

        }
    }
}
