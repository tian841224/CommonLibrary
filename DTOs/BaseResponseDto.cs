namespace CommonLibrary.DTOs
{
    using System.Text.Json.Serialization;
    using CommonLibrary.Extensions;

    public class BaseResponseDto
    {
        public int Id { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]

        public virtual DateTime CreateTime { get;  set; } 

        /// <summary>
        /// 更新時間
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public virtual DateTime UpdateTime { get;  set; } 
    }
}
