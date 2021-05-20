namespace MyWebAPI.MyOpenApiConfiguration
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class GroupedTagsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Extensions["x-tagGroups"] = new OpenApiArray
            {
                new OpenApiObject
                {
                    {
                        "name", new OpenApiString("All")
                    },
                    {
                        "tags", new OpenApiArray {new OpenApiString("UserName"), new OpenApiString("WeatherForecast")}
                    }
                },
                new OpenApiObject
                {
                    {
                        "name", new OpenApiString("Weather Forecast Only")
                    },
                    {
                        "tags", new OpenApiArray {new OpenApiString("WeatherForecast")}
                    }
                },
                new OpenApiObject
                {
                    {
                        "name", new OpenApiString("UserName Only")
                    },
                    {
                        "tags", new OpenApiArray {new OpenApiString("UserName")}
                    }
                }
            };
        }
    }
}