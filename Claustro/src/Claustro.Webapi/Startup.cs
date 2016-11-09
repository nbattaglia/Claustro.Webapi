using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using System.Text;
using Claustro.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Claustro.MongoRepository;

namespace Claustro.Webapi
{
    public class Startup
    {
        ICreds creds;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("vcap-local.json", optional: true, reloadOnChange: true)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();
            Configuration = builder.Build();


            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            creds = new Creds();
            string vcapServices = System.Environment.GetEnvironmentVariable("VCAP_SERVICES");
            if (vcapServices != null)
            {
                dynamic json = JsonConvert.DeserializeObject(vcapServices);
                foreach (dynamic obj in json.Children())
                {



                    if (((string)obj.Name).ToLowerInvariant().Contains("compose-for-mongodb"))
                    {
                        dynamic credentials = (((JProperty)obj).Value[0] as dynamic).credentials;
                        if (credentials != null)
                        {
                            string uri = credentials.uri;
                            creds.GetDataFromUri(uri);
                            creds.cert = credentials.ca_certificate_base64;
                            //break;
                        }
                    }
                }



            }
            else
            {
                string uri = Configuration["compose-for-mongodb:0:credentials:uri"];
                string ca_base64 = Configuration["compose-for-mongodb:0:credentials:ca_certificate_base64"];
                creds.GetDataFromUri(uri);
                creds.cert = ca_base64;
                

            }

            
            Console.WriteLine(creds.cs);


        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            //ioc
            //services.AddSingleton<ICreds>(creds);
            services.AddSingleton<IMongoSession>(new MongoSession(creds));
            services.Add(new ServiceDescriptor(typeof(IMongoRepository<>), typeof(MongoRepository<>), ServiceLifetime.Transient));
            // Add framework services.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    // This should be much more intelligent - at the moment only expired 
                    // security tokens are caught - might be worth checking other possible 
                    // exceptions such as an invalid signature.
                    if (error != null && (error.Error is SecurityTokenExpiredException || error.Error is SecurityTokenInvalidSignatureException))
                    {
                        context.Response.StatusCode = 401;
                        // What you choose to return here is up to you, in this case a simple 
                        // bit of JSON to say you're no longer authenticated.
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject(
                                new { authenticated = false, tokenExpired = true }));
                    }
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;

                        if (error.Error.Source == "Microsoft.AspNet.Authentication.JwtBearer")
                            context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        // TODO: Shouldn't pass the exception message straight out, change this.
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject
                            (new { success = false, error = error.Error.Message }));
                    }
                    // We're not trying to handle anything else so just let the default 
                    // handler handle.
                    else await next();
                });
            });



            var key = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("my super secret key goes here")));

            var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {

                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidAudience = TokenHandler.JWT_TOKEN_AUDIENCE,
                ValidIssuer = TokenHandler.JWT_TOKEN_ISSUER,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(0) //TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {

                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseStaticFiles();


      

            app.UseCors(opt =>
            opt.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );
            app.UseMvc();
        }
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var host = new WebHostBuilder()
                        .UseKestrel()
                         .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseConfiguration(config)
                        .UseStartup<Startup>()
                        .Build();

            host.Run();
        }
    }

   

}
