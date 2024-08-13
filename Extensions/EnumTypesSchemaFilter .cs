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

                // 描述加入EnumMember
                var chineseDescriptions = enumValues.Cast<object>()
                    .Select(enumValue =>
                    {
                        var memberInfo = enumType.GetMember(enumValue.ToString()).FirstOrDefault();
                        var enumMemberAttribute = memberInfo?.GetCustomAttribute<EnumMemberAttribute>();
                        var description = enumMemberAttribute?.Value ?? enumValue.ToString();
                        return $"{Convert.ToInt32(enumValue)} = {description}";
                    });
                schema.Description = string.Join(", ", chineseDescriptions);

                // 枚舉列表加入參數名稱
                schema.Enum = enumValues.Cast<object>()
                    .Select(enumValue =>
                    {
                        var enumValueInt = Convert.ToInt32(enumValue);
                        return new OpenApiString($"{enumValueInt} = {enumValue}");
                    })
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
