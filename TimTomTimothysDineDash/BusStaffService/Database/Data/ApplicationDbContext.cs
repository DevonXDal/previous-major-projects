using BusStaffService.Database.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BusStaffService.Database.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Table> Tables { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) 
    {
        
    }

    /// <summary>
    /// Creates the tables for the database and sets some constraints on the models.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this _context. Databases (and other extensions) typically
    /// define extension methods on this object that allow you to configure aspects of the model that are specific
    /// to a given database.
    /// </param>
    /// <remarks>
    /// This forces the use of the numeric data type using a specific precision and scale so that money paid is handled correctly.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Table>().ToTable(nameof(Table));
    }
}
