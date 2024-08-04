using CommonLibrary.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CommonLibrary.Extensions
{
    public static class PagedOperationExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, PagedOperationDto dto)
        {
            var result = new PagedResult<T>
            {
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                TotalCount = await query.CountAsync()
            };

            var skip = (dto.PageNumber - 1) * dto.PageSize;
            result.Items = await query.Skip(skip).Take(dto.PageSize).ToListAsync();

            return result;
        }

        public static PagedResult<T> GetPaged<T>(this List<T> list, PagedOperationDto dto)
        {
            var result = new PagedResult<T>
            {
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                TotalCount = list.Count
            };

            var skip = (dto.PageNumber - 1) * dto.PageSize;
            result.Items = list.Skip(skip).Take(dto.PageSize).ToList();

            return result;
        }
    }
}
