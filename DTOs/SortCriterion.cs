using CommonLibrary.Enum;

namespace CommonLibrary.DTOs
{
    public class SortCriterion
    {
        /// <summary>
        /// 排序的欄位名稱
        /// </summary>
        public string Field { get; set; } = null!;

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortTypeEnum SortType { get; set; } = SortTypeEnum.Desc;
    }
}
