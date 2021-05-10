using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyWebAPI
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this._provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
            
            // parameterise the server names here
            // these are the base url where an instance of the API is hosted
            options.AddServer(new OpenApiServer
            {
                Description = "Local Host",
                Url = "http://localhost:5000"
            });
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "My Sample Web API",
                Version = description.ApiVersion.ToString(),
                Description = "My sample Web API",
                Contact = new OpenApiContact()
                {
                    Name = "API Support",
                    Email = "apisupport@something.com",
                    Url = new Uri("https://www.something.kam")
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                },
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    {
                        "x-logo", new OpenApiObject
                        {
                            {
                                "url",
                                new OpenApiString(
                                    "https://static.wixstatic.com/media/2d8b87_238e5d2d26bb44dea93753c448f6c428~mv2_d_1394_1344_s_2.png/v1/fill/w_391,h_375,al_c,usm_0.66_1.00_0.01/2d8b87_238e5d2d26bb44dea93753c448f6c428~mv2_d_1394_1344_s_2.png")
                            },
                            {"altText", new OpenApiString("The Logo")}
                        }
                    },
                    {
                        "x-tagGroups", new OpenApiObject
                        {
                            {"name", new OpenApiString("Kam")},
                            {
                                "tags", new OpenApiArray
                                {
                                    new OpenApiString("UserName"),
                                    new OpenApiString("store")
                                }
                            }
                        }
                    }
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}