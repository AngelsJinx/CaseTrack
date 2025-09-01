using CaseTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseTrack.Data.Repositories;

public class TaskRepository(CaseTrackContext context) : BaseRepository<CaseTrackTask>(context)
{
    protected override DbSet<CaseTrackTask> GetDbSet()
    {
        return context.Tasks;
    }
}