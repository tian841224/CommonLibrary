using CommonLibrary.Enum;

namespace CommonLibrary.DTOs
{
    public class PagedOperationDto
    {
        /// <summary>
        /// 頁數
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// 單頁筆數
        /// </summary>
        public int PageSize { get; set; } = int.MaxValue;

        public List<SortOption> SortOptions { get; set; } = new List<SortOption> { new SortOption { Field = "Id", Desc = SortTypeEnum.Asc } };

        public class SortOption
        {
            public string Field { get; set; }
            public SortTypeEnum Desc { get; set; }
        }
    }
}
