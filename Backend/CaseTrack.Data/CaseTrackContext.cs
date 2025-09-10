using CaseTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

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
                .ValueGeneratedOnAdd()
                .HasValueGenerator(typeof(DateTimeOffsetValueGenerator));
        }
    }
}

internal class DateTimeOffsetValueGenerator : ValueGenerator<DateTimeOffset>
{
    public override bool GeneratesTemporaryValues => false;

    public override DateTimeOffset Next(EntityEntry entry)
    {
        if (entry is null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        return DateTimeOffset.UtcNow;
    }
}