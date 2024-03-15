using Microsoft.EntityFrameworkCore;
using TextStream.DataAccess.Models;
using TextStream.DataAccess.TablesConst;

namespace TextStream.DataAccess;

internal class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<BroadcastEntity> Broadcasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BroadcastEntity>().ToTable("broadcasts", schema: NameOfSchema.SchemaName);
        modelBuilder.Entity<BroadcastEntity>().HasKey(c => c.Id);
        modelBuilder.Entity<BroadcastEntity>().Property(c => c.DateStart)
            .HasColumnType(DataBaseColumnTypes.TimeStampWithoutTimeZone);
    }
}