namespace MyWebAPI
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using MyWeather.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class SampleModelSchemaFilter : ISchemaFilter
    {
        // ref: https://stackoverflow.com/questions/58681260/how-to-write-ischemafilter-for-problemdetails-in-asp-net-core-3-swashbuckle-5

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(SampleModel))
            {
                // TODO: Set the default and example based on the status code.
                schema.Default = new OpenApiObject
                {
                    ["ID"] = new OpenApiInteger(2),
                    ["Description"] = new OpenApiString("test"),
                    ["Random"] = new OpenApiString("random thing")
                };
                schema.Example = new OpenApiObject
                {
                    ["ID"] = new OpenApiInteger(9901),
                    ["Description"] = new OpenApiString("sample value"),
                    ["Random"] = new OpenApiString("random thing example")
                };
            }
        }
    }
}