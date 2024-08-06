using CommonLibrary.DTOs;

namespace CommonLibrary.Extensions
{
    public static class PagedOperationExtensions
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, PagedOperationDto dto)
        {
            var skip = (dto.PageNumber - 1) * dto.PageSize;
            var items = query.Skip(skip).Take(dto.PageSize).OrderBy(x => dto.OrderBy).ToList();

            var result = new PagedResult<T>
            {
                Items = items,
                Page = new PageResponseDto
                {
                    CurrentPage = dto.PageNumber,
                    PageSize = dto.PageSize,
                    CurrentPageCount = items.Count,
                    OrderBy = dto.OrderBy,
                }
            };

            result.Page.TotalCount = items.Count;
            result.Page.TotalPages = (int)Math.Ceiling(result.Page.TotalCount / (double)dto.PageSize);
            return result;
        }

        public static PagedResult<T> GetPaged<T>(this List<T> list, PagedOperationDto dto)
        {
            var skip = (dto.PageNumber - 1) * dto.PageSize;
            var items = list.Skip(skip).Take(dto.PageSize).OrderBy(x => dto.OrderBy).ToList();

            var result = new PagedResult<T>
            {
                Items = items,
                Page = new PageResponseDto
                {
                    CurrentPage = dto.PageNumber,
                    PageSize = dto.PageSize,
                    CurrentPageCount = items.Count,
                    OrderBy = dto.OrderBy,
                }
            };
            result.Page.TotalCount = list.Count;
            result.Page.TotalPages = (int)Math.Ceiling(result.Page.TotalCount / (double)dto.PageSize);
            return result;
        }
    }
}
