using Microsoft.EntityFrameworkCore;
using PalaCrypto.Model;
using System.Xml;

namespace PalaCrypto.DbContextPalacryto
{
    public class DbContextPalacrypto : DbContext
    {

        public DbContextPalacrypto(DbContextOptions<DbContextPalacrypto> options) : base(options)
        {
        }

        public DbSet<LogAdmin> LogAdmins { get; set; }
        public DbSet<LogDifferenceAdmin> LogDifferenceAdmins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared; foreign keys=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
