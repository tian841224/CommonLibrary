using Microsoft.EntityFrameworkCore;

namespace CommonLibrary.DTOs
{
    public class BasicSortResponseDto : BasicResponseDto
    {
        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; } = 0;
    }
}
