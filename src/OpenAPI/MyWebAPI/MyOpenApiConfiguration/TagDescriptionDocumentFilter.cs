using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyWebAPI.MyOpenApiConfiguration
{
    public class TagDescriptionDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Extensions["tags"] = new OpenApiArray
            {
                new OpenApiObject
                {
                    {
                        "name", new OpenApiString("UserName")
                    },
                    {
                        "description", new OpenApiString("This is where I would write a lot of blurb on what the `UserName` tag is.")
                    }
                }
            };
        }
    }
}