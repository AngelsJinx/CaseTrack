using CaseTrack;
using CaseTrack.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCaseTrackServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // For simplicity of the tech test I'm applying the migration at runtime (In development)
    // In the real world, the recommended approach is to generate SQL scripts from the migration and run them independently.
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<CaseTrackContext>();
        await db.Database.MigrateAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();