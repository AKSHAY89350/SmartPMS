using Microsoft.EntityFrameworkCore;
using SmartPMS.PerformancePlanService.Data;
using SmartPMS.PerformancePlanService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PerformancePlanDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPerformancePlanService, PerformancePlanService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SmartPMSCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseCors("SmartPMSCors");

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PerformancePlanDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();