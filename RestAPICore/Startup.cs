using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using RestAPICore.Models;
using RestAPICore.Services;
using System;
using System.IO;
using System.Reflection;

namespace RestAPICore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setting configuration for protected web api
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration);

            // Creating policies that wraps the authorization requirements
            services.AddAuthorization();

            // Adding in Blog Service with Connection Strings
            services.Configure<BlogDBSettingsModel>(
                Configuration.GetSection(nameof(BlogDBSettingsModel)));

            services.AddSingleton<IBlogDBSettingsModel>(sp =>
                sp.GetRequiredService<IOptions<BlogDBSettingsModel>>().Value);

            services.AddSingleton<BlogService>();

            // Adding in Contact Me Service with Connection Strings
            services.Configure<ContactDBSettingsModel>(
                Configuration.GetSection(nameof(ContactDBSettingsModel)));

            services.AddSingleton<IContactDBSettingsModel>(sp =>
                sp.GetRequiredService<IOptions<ContactDBSettingsModel>>().Value);

            services.AddSingleton<ContactService>();

            // Adding in Blog Subscription Service with Connection Strings
            services.Configure<SubscriptionDBSettingsModel>(
                Configuration.GetSection(nameof(SubscriptionDBSettingsModel)));

            services.AddSingleton<ISubscriptionDBSettingsModel>(sp =>
                sp.GetRequiredService<IOptions<SubscriptionDBSettingsModel>>().Value);

            services.AddSingleton<SubscriptionsService>();

            // Adding in Twitter Service with Connection Strings
            services.Configure<TwitterConnectionSettings>(
                Configuration.GetSection(nameof(TwitterConnectionSettings)));

            services.AddSingleton<ITwitterConnectionSettings>(sp =>
                sp.GetRequiredService<IOptions<TwitterConnectionSettings>>().Value);

            services.AddSingleton<TwitterService>();
            services.AddSingleton<Twitter2Service>();

            // Adding in Google API Service with Connection Strings
            services.Configure<GoogleConnectionsSettings>(
                Configuration.GetSection(nameof(GoogleConnectionsSettings)));

            services.AddSingleton<IGoogleConnectionsSettings>(sp =>
                sp.GetRequiredService<IOptions<GoogleConnectionsSettings>>().Value);

            services.AddSingleton<GooglePhotosService>();

            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());

            // Allowing CORS for all domains and methods for the purpose of the sample
            // In production, modify this with the actual domains you want to allow
            services.AddCors(o => o.AddPolicy("default", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RestAPIcore for JesseRules",
                    Version = "v1",
                    Description = "New .Net5.0 API for JesseRules",
                    Contact = new OpenApiContact
                    {
                        Name = "JesseRules",
                        Email = string.Empty,
                        Url = new Uri("https://jesserules.azurewebsites.net/#/home"),
                    },
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Since IdentityModel version 5.2.1 (or since Microsoft.AspNetCore.Authentication.JwtBearer version 2.2.0),
                // Personal Identifiable Information is not written to the logs by default, to be compliant with GDPR.
                // For debugging/development purposes, one can enable additional detail in exceptions by setting IdentityModelEventSource.ShowPII to true.
                // Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //Load Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestAPIcore v1"));

            //Use Default Pages ie index.html
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors("default");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}