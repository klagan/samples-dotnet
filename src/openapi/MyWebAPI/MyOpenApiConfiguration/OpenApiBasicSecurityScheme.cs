namespace MyWebAPI.MyOpenApiConfiguration
{
    using Microsoft.OpenApi.Models;

    public class OpenApiBasicSecurityScheme : OpenApiSecurityScheme
    {
        public OpenApiBasicSecurityScheme()
        {
            Type = SecuritySchemeType.Http;
            Scheme = "basic";
            Reference = new OpenApiReference
            {
                Id = "BasicAuth",
                Type = ReferenceType.SecurityScheme
            };
        }
    }
}