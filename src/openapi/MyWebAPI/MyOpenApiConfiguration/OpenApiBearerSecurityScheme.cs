namespace MyWebAPI.MyOpenApiConfiguration
{
    using Microsoft.OpenApi.Models;

    public class OpenApiBearerSecurityScheme : OpenApiSecurityScheme
    {
        public OpenApiBearerSecurityScheme()
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"";
            Name = "Authorization";
            In = ParameterLocation.Header;
            Type = SecuritySchemeType.Http;
            Scheme = "bearer";
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            };
        }
    }
}