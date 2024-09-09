using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Entities
{
    public class BaseEntity
    {

        [Key]
        public int Id { get; set; }

        [Comment("建立日期")]
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;

        [Comment("更新時間")]
        public virtual DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
