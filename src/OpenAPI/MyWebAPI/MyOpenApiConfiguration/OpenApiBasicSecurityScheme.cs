namespace MyWebAPI.MyOpenApiConfiguration
{
    using System.Buffers.Text;
    using Microsoft.OpenApi.Models;

    public class OpenApiBasicSecurityScheme : OpenApiSecurityScheme
    {
        public OpenApiBasicSecurityScheme() 
            : base()
        {
            Type = SecuritySchemeType.Http;
        Scheme = "basic";
        Reference = new OpenApiReference {
             Id = "BasicAuth", 
             Type = ReferenceType.SecurityScheme
             };
        }
    }
}