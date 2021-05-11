namespace MyWebAPI.MyOpenApiConfiguration
{
    using System;
    using Microsoft.OpenApi.Models;

    public class OpenApiAADSecurityScheme : OpenApiSecurityScheme
    {
        public OpenApiAADSecurityScheme()
            : base()
        {
            Description =
                "JWT authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\" User interactively authenticates with Azure Active Directory";
            Name = "Authorization";
            Type = SecuritySchemeType.OAuth2;
            Flows = new OpenApiOAuthFlows
            {
                Implicit = new OpenApiOAuthFlow
                {
                    TokenUrl = new Uri(
                        "https://login.microsoftonline.com/putATenantIdHere/oauth2/v2.0/token"),
                    AuthorizationUrl = new Uri(
                        "https://login.microsoftonline.com/putATenantIdHere/oauth2/v2.0/authorize"),
                    Scopes =
                    {
                        {"my_scope_1", "Scope 1"},
                        {"my_scope_2", "Scope 2"},
                        {"my_scope_3", "Scope 3"}
                    }
                }
            };
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "AAD"
            };
        }
    }
}