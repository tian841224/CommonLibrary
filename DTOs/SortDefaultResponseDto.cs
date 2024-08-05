using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CommonLibrary.Extensions;

namespace CommonLibrary.DTOs
{
    public class SortDefaultResponseDto
    {
        public int Id { get;  set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]

        public DateTime CreateTime { get;  set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime UpdateTime { get;  set; } = DateTime.UtcNow;

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; } = 0;
    }
}
