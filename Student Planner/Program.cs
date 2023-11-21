using Microsoft.EntityFrameworkCore;
using Student_Planner.Databases;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Repositories.Implementations;
using Student_Planner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EventsDBContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), 
        x => x.UseDateOnlyTimeOnly()));

builder.Services.AddScoped<IDayRepository, DayRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<EventServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(
    );

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "calendar",
    pattern: "{controller=Calendar}/{action=Calendar}/{id?}");
app.MapControllerRoute(
    name: "event",
    pattern: "{controller=Event}/{action=DayEvent}/{id?}");

app.Run();
