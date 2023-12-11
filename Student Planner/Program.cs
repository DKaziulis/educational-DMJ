using Microsoft.EntityFrameworkCore;
using Student_Planner.Databases;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Repositories.Implementations;
using Serilog;
using Student_Planner.Services.Interfaces;
using Student_Planner.Services.Implementations;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EventsDBContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), 
        x => x.UseDateOnlyTimeOnly()));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calendar API", Version = "v1" });
});

builder.Services.AddScoped<IDayRepository, DayRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventServices, EventServices>();
builder.Services.AddScoped<IDayOperator, DayOperator>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calendar API");
    });
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(
    );

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "event",
    pattern: "{controller=Event}/{action=DayEvent}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
