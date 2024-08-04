using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs
{
    public class SortBasicDto
    {
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; } = 0;
    }
}
