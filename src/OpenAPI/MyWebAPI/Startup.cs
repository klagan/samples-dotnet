namespace MyWebAPI
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Microsoft.OpenApi.Models;

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
                    
                    // remove text/plain as a default to avoid response errors for unhandled mediatype
                    setupAction.OutputFormatters.RemoveType<StringOutputFormatter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            
            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                } );
            
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                } );
            
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

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    foreach (var documentationFile in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
                    {
                        options.IncludeXmlComments(documentationFile);
                    }
                    
                    // this is just an example and does not work as the API is not protected by secuirty layer
                    // and the token values below are not valid
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows()
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri(
                                    $"https://login.microsoftonline.com/putATenantIdHere/oauth2/v2.0/token"), 
                                AuthorizationUrl = new Uri(
                                    $"https://login.microsoftonline.com/putATenantIdHere/oauth2/v2.0/authorize"),  
                                Scopes =
                                {
                                    { "my_scope_1", "Scope 1"},
                                    { "my_scope_2", "Scope 2"},
                                    { "my_scope_3", "Scope 3"}
                                },
                            }
                        }
                    });
                } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseSwagger();

            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }

                    options.RoutePrefix = string.Empty;
                    options.HeadContent = @"<p style=""color:red""><b><center>this may be a good place for identifying environments, perhaps?</center></b></p>";
                    options.DocumentTitle = "my sample web api";
                    options.OAuthClientId("my_client_id");
                    options.OAuthClientSecret("my_client_secret");
                    
                    // scopes are defined in the OpenApiOAuthFlows of the SecurityDefinition object
                    // default scopes to select are set here
                    options.OAuthScopes("my_scope_1", "my_scope_3");
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}