namespace CaseTrack.Data.Entities;

/// <summary>
/// Parent class for all entities in the CaseTrack database.
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Unique identifier for an Entity.
    /// </summary>
    /// <remarks>This is the default primary key value for all entities. See <see cref="CaseTrackContext.OnModelCreating"/></remarks>
    public long Id { get; set; }
    
    /// <summary>
    /// Timestamp recording when the Entity was created.
    /// </summary>
    /// <remarks>This is auto-populated when added to tracking. See <see cref="CaseTrackContext.OnModelCreating"/></remarks>
    public DateTimeOffset Created { get; set; }
}