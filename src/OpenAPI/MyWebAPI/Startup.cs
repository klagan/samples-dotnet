namespace MyWebAPI
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
            services.AddControllers(setupAction =>
                {
                    // add "common" openapi response documentation filters to all API methods
                    setupAction.Filters.Add(
                        new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                    setupAction.Filters.Add(
                        new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                    setupAction.Filters.Add(
                        new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

                    // TODO: causing response errors when accept header is test/plain
                    setupAction.Filters.Add(
                        new ProducesDefaultResponseTypeAttribute());

                    setupAction.ReturnHttpNotAcceptable = true;
                    setupAction.RespectBrowserAcceptHeader = true;

                    // add an XML formatter to the output types openapi documentation
                    setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                    // find the JSON outputter if it exists
                    var jsonOutputFormatter = setupAction.OutputFormatters
                        .OfType<SystemTextJsonOutputFormatter>()
                        .FirstOrDefault();

                    if (jsonOutputFormatter == null) return;

                    // remove text/json as it isn't the approved media type for working with JSON at API level
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                    
                    // remove text/plain as a default
                    setupAction.OutputFormatters.RemoveType<StringOutputFormatter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // if a mal-formed input model is created then return 406 to indicate this
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
                
                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }
                    
                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });
            
            services.AddTransient(typeof(MyWeather.Bll.Compute));
            services.AddTransient(typeof(MyUserName.Bll.Compute));
            
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "MyWeatherAPI",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "My Weather API",
                        Version = "1",
                        Description = "My Weather API Sample",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "github@lagan.me",
                            Name = "Kam Lagan",
                            Url = new Uri("https://www.something.kam")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });
                
                setupAction.SwaggerDoc(
                    "MyUserNameAPI",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "My UserName API",
                        Version = "1",
                        Description = "My UserName API Sample",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "github@lagan.me",
                            Name = "Kam Lagan",
                            Url = new Uri("https://www.something.kam")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });
                
                foreach (var documentationFile in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
                {
                    setupAction.IncludeXmlComments(documentationFile);
                }
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/MyWeatherAPI/swagger.json",
                    "My Weather API");
                
                setupAction.SwaggerEndpoint(
                    "/swagger/MyUserNameAPI/swagger.json",
                    "My UserName API");
                
                setupAction.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}