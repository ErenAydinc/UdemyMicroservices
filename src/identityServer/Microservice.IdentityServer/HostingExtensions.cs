using Duende.IdentityServer;
using Microservice.IdentityServer.Data;
using Microservice.IdentityServer.Models;
using Microservice.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Duende.IdentityServer.Validation;


namespace Microservice.IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "My API",
                Version = "v1",
                Description = "ASP.NET 8 Web API with Swagger",
            });

            // JWT desteði eklemek için aþaðýdaki kodu açabilirsiniz
            // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            // {
            //     Name = "Authorization",
            //     Type = SecuritySchemeType.Http,
            //     Scheme = "Bearer",
            //     BearerFormat = "JWT",
            //     In = ParameterLocation.Header,
            //     Description = "Enter 'Bearer' [space] and then your token in the text input below."
            // });
        });

        builder.Services.AddLocalApiAuthentication();
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>();

        //builder.Services.AddIdentityServerBuilder().AddDeveloperSigningCredential();

        builder.Services.AddIdentityServerBuilder()
            .AddDeveloperSigningCredential()
            .AddResourceOwnerValidator<IdentityResourceOwnerPasswordValidator>();
        
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            });
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.RoutePrefix = ""; // Swagger'ý ana dizinde çalýþtýrmak için
            });
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}