namespace MyWebAPI.MyOpenApiConfiguration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Interfaces;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    ///     Configures the Swagger generation options.
    /// </summary>
    /// <remarks>
    ///     This allows API versioning to define a Swagger document per API version after the
    ///     <see cref="IApiVersionDescriptionProvider" /> service has been resolved from the service container.
    /// </remarks>
    public class MyOpenApiGenerationOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MyOpenApiGenerationOptions" /> class.
        /// </summary>
        /// <param name="provider">
        ///     The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger
        ///     documents.
        /// </param>
        public MyOpenApiGenerationOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
            
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            
            // parameterise the server names here
            // these are the base url where an instance of the API is hosted
            options.AddServer(new OpenApiServer
            {
                Description = "Local host",
                Url = string.Empty
            });
            
            options.AddServer(new OpenApiServer
            {
                Description = "Some host",
                Url = "http://localhost:5000"
            });
            
            options.AddServer(new OpenApiServer
            {
                Description = "Another Host (fake)",
                Url = "http://remotehost:5000"
            });
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "My Sample Web API",
                Version = description.ApiVersion.ToString(),
                Description = @"
# Introduction
This API is documented in **OpenAPI format** and originally provided by [swagger.io](http://swagger.io) team. It was **extended** to illustrate features of [generator-openapi-repo](https://github.com/Rebilly/generator-openapi-repo) tool and [ReDoc](https://github.com/Redocly/redoc) documentation. In addition to standard OpenAPI syntax we use a few [vendor extensions](https://github.com/Redocly/redoc/blob/master/docs/redoc-vendor-extensions.md).
## OpenAPI Specification
This API is documented in **OpenAPI format** and originally provided by [swagger.io](http://swagger.io) team. It was **extended** to illustrate features of [generator-openapi-repo](https://github.com/Rebilly/generator-openapi-repo) tool and [ReDoc](https://github.com/Redocly/redoc) documentation. In addition to standard OpenAPI syntax we use a few [vendor extensions](https://github.com/Redocly/redoc/blob/master/docs/redoc-vendor-extensions.md).
## Cross-Origin Resource Sharing
This API features Cross-Origin Resource Sharing (CORS) implemented in compliance with  [W3C spec](https://www.w3.org/TR/cors/).  And that allows cross-domain communication from the browser. All responses have a wildcard same-origin which makes them completely public and accessible to everyone, including any code on any site.
## Another section
This is a section to illustrate a child section
### Child section
This is a child section
# Authentication
(not true! - this is just sample documentation)
**My Sample Web API** offers two forms of authentication:
- API Key
- OAuth2
OAuth2 - an open protocol to allow secure authorization in a simple and standard method from web, mobile and desktop applications.",
                Contact = new OpenApiContact
                {
                    Name = "API Support",
                    Email = "apisupport@sample.laganlabs.it",
                    Url = new Uri("https://www.laganlabs.it")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                },
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    {
                        // should probably use static files at this point to remove the dependency on a third party
                        "x-logo", new OpenApiObject
                        {
                            {
                                "url",
                                new OpenApiString(
                                    "http://images5.fanpop.com/image/photos/29000000/Little-Monkey-monkeys-29039877-449-438.png")
                            },
                            {"altText", new OpenApiString("The Logo")}
                        }
                    }
                }
            };

            if (description.IsDeprecated) info.Description += " This API version has been deprecated.";

            return info;
        }
    }
}