using System.Reflection;
using CaseTrack;
using CaseTrack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCaseTrackServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.UseInlineDefinitionsForEnums();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Case Track API", Version = "v1" });
    options.IncludeXmlComments(Assembly.GetExecutingAssembly(), true);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("v1/swagger.json", "Case Track API V1"));
    
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

// Expose this magic implicit Program class for tests.
public partial class Program { }