using Microsoft.EntityFrameworkCore;
using MeterReadingApi.Models;
using MeterReadingUploads.Models;
namespace MeterReadingApi.Repository
{
    public class MeterReadingContext : DbContext
    {
        public DbSet<MeterReading> MeterReadings { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public MeterReadingContext(DbContextOptions<MeterReadingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
