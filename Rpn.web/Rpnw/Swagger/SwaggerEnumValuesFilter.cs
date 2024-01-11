using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Rpnw.Presentation.Rest.Services.Attributs;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection;

namespace Rpnw.Swagger
{
    public class SwaggerEnumValuesFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
          
                foreach (var propertyInfo in context.Type.GetProperties())
                 {
                

                var enumAttribute = propertyInfo.GetCustomAttribute<SwaggerEnumValuesAttribute>();
                if (enumAttribute != null)
                {
                    // Convertissez le nom de la propriété en camelCase.
                    string jsonPropertyName = ConvertToCamelCase(propertyInfo.Name);
                    if (schema.Properties.ContainsKey(jsonPropertyName))
                    {
                        var property = schema.Properties[jsonPropertyName];
                        property.Enum = Enum.GetNames(enumAttribute.EnumType)
                                            .Select(name => new OpenApiString(name))
                                            .ToList<IOpenApiAny>();
                        property.Nullable = true; // Set nullable to true since ValueType can be null
                    }
                }
            }
        }

        private static string ConvertToCamelCase(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !char.IsUpper(propertyName[0]))
            {
                return propertyName;
            }

            char[] chars = propertyName.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    break;
                }

                chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
            }

            return new string(chars);
        }
    }
}
