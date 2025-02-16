using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class PublicHolidaysDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Holiday> Holidays { get; set; }
    public DbSet<HolidayEffectivePeriod> HolidayEffectivePeriods { get; set; }
    public DbSet<HolidayOccurrence> HolidayOccurrences { get; set; }
    public DbSet<LocalizedTextEntry> LocalizedTextEntries { get; set; }

    public PublicHolidaysDbContext(DbContextOptions<PublicHolidaysDbContext> options) 
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    } 
}