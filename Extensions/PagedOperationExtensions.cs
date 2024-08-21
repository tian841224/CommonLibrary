using CommonLibrary.DTOs;
using static CommonLibrary.DTOs.PagedOperationDto;
using CommonLibrary.Enum;
using System.Linq.Dynamic.Core;

namespace CommonLibrary.Extensions
{
    public static class PagedOperationExtensions
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, PagedOperationDto dto)
        {
            // 應用排序
            query = ApplySorting(query, dto.SortOptions);

            var totalCount = query.Count();
            var skip = (dto.PageNumber - 1) * dto.PageSize;
            var items = query.Skip(skip).Take(dto.PageSize).ToList();

            return CreatePagedResult(items, dto, totalCount);
        }

        public static PagedResult<T> GetPaged<T>(this List<T> list, PagedOperationDto dto)
        {
            var query = list.AsQueryable();

            // 應用排序
            query = ApplySorting(query, dto.SortOptions);

            var totalCount = list.Count;
            var skip = (dto.PageNumber - 1) * dto.PageSize;
            var items = query.Skip(skip).Take(dto.PageSize).ToList();

            return CreatePagedResult(items, dto, totalCount);
        }

        private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, List<SortOption> sortOptions)
        {
            if (sortOptions == null || !sortOptions.Any())
                return query;

            var orderByString = string.Join(", ", sortOptions.Select(x =>$"{x.Field} {(x.Desc == SortTypeEnum.Desc ? "descending" : "ascending")}"));
            return query.OrderBy(orderByString);
        }

        private static PagedResult<T> CreatePagedResult<T>(List<T> items, PagedOperationDto dto, int totalCount)
        {
            var result = new PagedResult<T>
            {
                Items = items,
                Page = new PageResponseDto
                {
                    CurrentPage = dto.PageNumber,
                    PageSize = dto.PageSize,
                    CurrentPageCount = items.Count,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)dto.PageSize),
                    SortOptions = dto.SortOptions
                }
            };

            return result;
        }

        //public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, PagedOperationDto dto)
        //{
        //    var skip = (dto.PageNumber - 1) * dto.PageSize;
        //    var items = query.Skip(skip).Take(dto.PageSize).ToList().OrderBy(dto.OrderBy, dto.Desc);
        //    var result = new PagedResult<T>
        //    {
        //        Items = items,
        //        Page = new PageResponseDto
        //        {
        //            CurrentPage = dto.PageNumber,
        //            PageSize = dto.PageSize,
        //            CurrentPageCount = items.Count,
        //            OrderBy = dto.OrderBy,
        //        }
        //    };

        //    result.Page.TotalCount = items.Count;
        //    result.Page.TotalPages = (int)Math.Ceiling(result.Page.TotalCount / (double)dto.PageSize);

        //    return result;
        //}

        //public static PagedResult<T> GetPaged<T>(this List<T> list, PagedOperationDto dto)
        //{
        //    var skip = (dto.PageNumber - 1) * dto.PageSize;
        //    var items = list.Skip(skip).Take(dto.PageSize).ToList().OrderBy(dto.OrderBy, dto.Desc);

        //    var result = new PagedResult<T>
        //    {
        //        Items = items,
        //        Page = new PageResponseDto
        //        {
        //            CurrentPage = dto.PageNumber,
        //            PageSize = dto.PageSize,
        //            CurrentPageCount = items.Count,
        //            OrderBy = dto.OrderBy,
        //        }
        //    };
        //    result.Page.TotalCount = list.Count;
        //    result.Page.TotalPages = (int)Math.Ceiling(result.Page.TotalCount / (double)dto.PageSize);
        //    return result;
        //}
    }
}
