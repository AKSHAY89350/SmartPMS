using Microsoft.EntityFrameworkCore;
using SmartPMS.SelfAppraisalService.Data;
using SmartPMS.SelfAppraisalService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<SelfAppraisalDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ISelfAppraisalService, SelfAppraisalService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SmartPMSCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("SmartPMSCors");
app.UseAuthorization();
app.MapControllers();

app.Run();
