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
                var enumType = context.Type;
                var enumValues = System.Enum.GetValues(enumType);

                // 描述增加完整說明
                var fullDescriptions = enumValues.Cast<object>()
                    .Select(enumValue =>
                    {
                        var memberInfo = enumType.GetMember(enumValue.ToString()).FirstOrDefault();
                        var enumMemberAttribute = memberInfo?.GetCustomAttribute<EnumMemberAttribute>();
                        var description = enumMemberAttribute?.Value ?? enumValue.ToString();
                        return $"{Convert.ToInt32(enumValue)} = {description} ({enumValue})";
                    });
                schema.Description = string.Join(", ", fullDescriptions);

                // 枚舉值只包含數字
                schema.Enum = enumValues.Cast<object>()
                    .Select(enumValue => new OpenApiInteger(Convert.ToInt32(enumValue)))
                    .Cast<IOpenApiAny>()
                    .ToList();

                // 設定 Example 為純數字
                if (schema.Enum.Any())
                {
                    schema.Example = new OpenApiInteger(Convert.ToInt32(enumValues.GetValue(0)));
                }
            }
        }
    }
}
