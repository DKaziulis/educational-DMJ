using Microsoft.EntityFrameworkCore;
using Student_Planner.Databases;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Repositories.Implementations;
using Serilog;
using Student_Planner.Services.Interfaces;
using Student_Planner.Services.Implementations;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EventsDBContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), 
        x => x.UseDateOnlyTimeOnly()));

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 5;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<EventsDBContext>();

builder.Services.AddTransient<LoggingInterceptor>();

builder.Services.AddHttpClient("LoggingInterceptorClient")
            .AddHttpMessageHandler<LoggingInterceptor>();


builder.Services.AddScoped<IDayRepository, DayRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventServices, EventServices>();
builder.Services.AddScoped<IDayOperator, DayOperator>();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    // Cookie settings
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

//    options.LoginPath = "/Users/Login";
//    options.AccessDeniedPath = "/Home";
//    options.SlidingExpiration = true;
//});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();


app.UseMiddleware<AuthenticationMiddleware>();


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting(
    );

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "event",
    pattern: "{controller=Event}/{action=DayEvent}/{id?}");

app.Run();
