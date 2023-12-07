using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration["MySqlConnection:MySqlConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(
    connection,
    new MySqlServerVersion(new Version(8, 1)))
);

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MySQLContext>()
    .AddDefaultTokenProviders();

// Add Identity Server and options
var builderServices = builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.EmitStaticAudienceClaim = true;
    }).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
      .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
      .AddInMemoryClients(IdentityConfiguration.Clients)
      .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builderServices.AddDeveloperSigningCredential();

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

var initializer = app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

// Redireciona para HTTPS
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Uso do IdentityServer (precisa estar entre UseRouting e UseAuthorization)
app.UseIdentityServer();

app.UseAuthorization();

initializer.Initialize();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
