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

        public string OrderBy { get; set; } = "Id";

        //public int Desc { get; set; }
    }
}
