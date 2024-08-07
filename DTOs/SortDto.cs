using Microsoft.EntityFrameworkCore;

namespace CommonLibrary.DTOs
{
    public class SortDto
    {
        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; } = 0;
    }
}
