using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;

namespace CommonLibrary.Extensions
{
    public class EnumTypesSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                var enumType = context.Type;
                var enumValues = System.Enum.GetValues(enumType);

                foreach (var enumValue in enumValues)
                {
                    var memberInfo = enumType.GetMember(enumValue.ToString()).FirstOrDefault();
                    var enumMemberAttribute = memberInfo?.GetCustomAttribute<EnumMemberAttribute>();
                    var description = enumMemberAttribute?.Value ?? enumValue.ToString();

                    var enumValueInt = Convert.ToInt32(enumValue);
                    schema.Enum.Add(new OpenApiString($"{enumValueInt} = {description}"));
                }

                //修改註解
                //schema.Description = string.Join(", ", schema.Enum.Select(x => x.ToString()));
            }

        }
    }
}
