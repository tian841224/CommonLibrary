namespace CommonLibrary.DTOs
{
    public class BasicSortDto : BasicDto
    {
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int? Sort { get; set; } = 0;
    }
}
