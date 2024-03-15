using Loans.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Loans.DataAccess;

internal class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<ClientEntity> Clients { get; set; }

    public DbSet<LoanEntity> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientEntity>().ToTable("clients", schema: TablesConst.NameOfSchema.SchemaName);
        modelBuilder.Entity<ClientEntity>().HasKey(c => c.Id);
        modelBuilder.Entity<ClientEntity>().Property(c => c.BirthDate)
            .HasColumnType(TablesConst.DataBaseColumnTypes.TimeStampWithoutTimeZone);
        modelBuilder.Entity<LoanEntity>().ToTable("loans", schema: TablesConst.NameOfSchema.SchemaName);
        modelBuilder.Entity<LoanEntity>().HasKey(c => c.Id);
        modelBuilder.Entity<LoanEntity>().Property(c => c.CreationDate).HasColumnType(TablesConst.DataBaseColumnTypes.TimeStampWithTimeZone);
    }
}