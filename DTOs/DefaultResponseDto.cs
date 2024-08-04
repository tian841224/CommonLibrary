using CommonLibrary.Extensions;
using System.Text.Json.Serialization;

namespace CommonLibrary.DTOs
{
    public class DefaultResponseDto
    {
        public int Id { get; protected set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]

        public DateTime CreateTime { get; protected set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime UpdateTime { get; protected set; } = DateTime.UtcNow;
    }
}
