using CommonLibrary.DTOs;

namespace CommonLibrary.Extensions
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }

        public PageResponseDto Page { get; set; } = new PageResponseDto();

        public PagedResult()
        {
            Items = new List<T>();
        }

    }

}
