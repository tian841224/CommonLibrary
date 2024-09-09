namespace CommonLibrary.DTOs
{
    public class BaseSortDto : BaseDto
    {
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int? Sort { get; set; } = 0;
    }
}
