using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web_CSV_Json_XML_reader.Data.DB.Entities;

namespace Web_CSV_Json_XML_reader.Data.DB
{
    public class EFContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Entities.File> Files { get; set; }

        public DbSet<СryptographyKey> сryptographyKeys { get; set; }

        public EFContext(DbContextOptions<EFContext> options)
        : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlite(Configuration.GetConnectionString("EFContext"));
        //    optionsBuilder.UseSqlite("Data Source=mydatabase.db;");
        //}
    }
}
