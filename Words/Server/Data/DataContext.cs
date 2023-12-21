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
        public DbSet<User> Users { get; set; }

        //public DbSet<Word> Weath { get; set; }
    }
}
