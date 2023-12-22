using Microsoft.EntityFrameworkCore;
using Words.Shared;

namespace Words.Server.Data
{
    public class DataContext : DbContext
    {
        // For connecting to the database that is installed in local machine
        // Options are for getting the connection string 
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        // Name of the table (Users)
        // Table Will be created during imigrations
        //public DbSet<ApplicationUser> Users { get; set; }

        //public DbSet<UserWord> Words { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserWord> UserWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, indexes, etc.
            modelBuilder.Entity<UserWord>()
                .HasOne(uw => uw.User)
                .WithMany(u => u.UserWords)
                .HasForeignKey(uw => uw.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust cascade delete behavior as needed

            // Add other configurations as necessary
        }
    }
}
