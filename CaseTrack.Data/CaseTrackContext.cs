using Microsoft.EntityFrameworkCore;

namespace CaseTrack.Data;

public class CaseTrackContext : DbContext
{
    public DbSet<Task> Tasks { get; set; }
    
}