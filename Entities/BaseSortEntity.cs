using Microsoft.EntityFrameworkCore;

namespace CommonLibrary.Entities
{
    public class SortDefaultEntity : BaseEntity
    {
        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; } = 0;
    }
}
