using Holidays.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Holidays.Storage
{
    public class StorageContext : DbContext
    {
        public DbSet<CountryEntity> Countries { get; set; }
        
        public DbSet<CountryDateEntity> CountryDates { get; set; }
        
        public DbSet<CountryHolidayEntity> CountryHolidays { get; set; }

        private readonly IConfiguration _configuration;

        public StorageContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["ConnectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CountryEntity>()
                .ToTable("Countries")
                .HasKey(x => x.Code);
            
            modelBuilder.Entity<CountryEntity>()
                .HasMany(x => x.Days)
                .WithOne(x => x.Country)
                .HasForeignKey(x => x.CountryCode);

            modelBuilder.Entity<CountryDateEntity>()
                .ToTable("CountryDates")
                .HasKey(d => d.Id);

            modelBuilder.Entity<CountryDateEntity>()
                .Property(x => x.Date)
                .HasColumnType("Date");

            modelBuilder.Entity<CountryDateEntity>()
                .HasIndex(x => new {x.CountryCode, x.Date})
                .IsUnique();

            modelBuilder.Entity<CountryDateEntity>()
                .HasOne(x => x.Holiday)
                .WithOne(x => x.CountryDate)
                .HasForeignKey<CountryHolidayEntity>(x => x.CountryDateId);

            modelBuilder.Entity<CountryHolidayEntity>()
                .ToTable("CountryHolidays")
                .HasKey(x => x.Id);
        }
    }
}