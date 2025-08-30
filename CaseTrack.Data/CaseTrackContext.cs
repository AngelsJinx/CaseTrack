using CaseTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseTrack.Data;

public class CaseTrackContext(DbContextOptions<CaseTrackContext> options) : DbContext(options)
{
    public DbSet<CaseTrackTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        

        foreach (var entityType in modelBuilder.Model.FindLeastDerivedEntityTypes(typeof(BaseEntity)))
        {
            modelBuilder.Entity(entityType.ClrType)
                .HasKey(nameof(CaseTrackTask.Id));

            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(CaseTrackTask.Created))
                .ValueGeneratedOnAdd();
        }
    }
}