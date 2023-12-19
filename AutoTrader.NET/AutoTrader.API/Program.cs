using AutoTrader.API.ServicesExtensions;
using AutoTrader.Application;
using AutoTrader.Domain.Entities;
using AutoTrader.Domain.Exceptions;
using AutoTrader.Domain.Helpers;
using AutoTrader.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.ConfigureSwagger(builder.Configuration);

var app = builder.Build();

#endregion

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var rolesManager = services.GetRequiredService<RoleManager<Role>>();
        await AppDbContextSeed.SeedDefaultUsersAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        Console.WriteLine($"Exception: {exception}");

        switch (exception)
        {
            case AutoTraderException:
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(exception.Message);
                break;
            case StrangeException:
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(exception.Message);
                break;
            default:
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ErrorHelper.ExceptionWasThrown);
                break;
        }
    });
});

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
} else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseRouting();
app.UseCors("corsapp");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

app.Run();
