using static CommonLibrary.DTOs.PagedOperationDto;

namespace CommonLibrary.DTOs
{
    public class PageResponseDto
    {
        /// <summary>
        /// 當前頁數
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 當前頁面筆數
        /// </summary>
        public int CurrentPageCount { get; set; }

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }

        ///// <summary>
        ///// 顯示筆數
        ///// </summary>
        public int PageSize { get; set; }

        public List<SortOption> SortOptions { get; set; }
    }

}
