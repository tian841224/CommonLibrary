using CommonLibrary.DTOs;

namespace CommonLibrary.Extensions
{
    public static class PagedOperationExtensions
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, PagedOperationDto dto)
        {
            var skip = (dto.PageNumber - 1) * dto.PageSize;
            var items = query.Skip(skip).Take(dto.PageSize).ToList();

            var result = new PagedResult<T>
            {
                CurrentPage = dto.PageNumber,
                PageSize = dto.PageSize,
                CurrentPageCount = items.Count,
                Items = items
            };

            result.TotalCount = items.Count;
            result.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)dto.PageSize);
            return result;
        }

        public static PagedResult<T> GetPaged<T>(this List<T> list, PagedOperationDto dto)
        {
            var skip = (dto.PageNumber - 1) * dto.PageSize;
            var items = list.Skip(skip).Take(dto.PageSize).ToList();

            var result = new PagedResult<T>
            {
                CurrentPage = dto.PageNumber,
                PageSize = dto.PageSize,
                CurrentPageCount = items.Count,
                Items = items
            };
            result.TotalCount = list.Count;
            result.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)dto.PageSize);
            return result;
        }
    }
}
