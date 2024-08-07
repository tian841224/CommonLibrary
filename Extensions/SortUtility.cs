using CommonLibrary.DTOs;
using CommonLibrary.Enum;
using Google.Protobuf.WellKnownTypes;

namespace CommonLibrary.Extensions
{
    /// <summary>
    /// 排序工具類
    /// </summary>
    /// <typeparam name="T">要排序的類型</typeparam>
    public static class SortUtility
    {
        /// <summary>
        /// 依照排序條件動態排序
        /// </summary>
        /// <param name="list">來源資料</param>
        /// <param name="sortCriteria">排序條件</param>
        /// <returns></returns>
        public static List<T> OrderBy<T>(this List<T> list, List<SortCriterion> sortCriteria)
        {
            IOrderedEnumerable<T>? orderedQuery = null;

            foreach (var sortCriterion in sortCriteria)
            {
                //首字轉成大寫
                sortCriterion.Field = sortCriterion.Field.Substring(0, 1).ToUpper() + sortCriterion.Field.Substring(1);
                var propertyInfo = typeof(T).GetProperty(sortCriterion.Field);
                if (propertyInfo == null) continue;


                if (orderedQuery == null)
                {
                    orderedQuery = sortCriterion.SortType == SortTypeEnum.Asc
                        ? list.OrderBy(x => propertyInfo.GetValue(x, null))
                        : list.OrderByDescending(x => propertyInfo.GetValue(x, null));
                }
                else
                {
                    orderedQuery = sortCriterion.SortType == SortTypeEnum.Asc
                        ? orderedQuery.ThenBy(x => propertyInfo.GetValue(x, null))
                        : orderedQuery.ThenByDescending(x => propertyInfo.GetValue(x, null));
                }
            }

            return orderedQuery == null ? list : orderedQuery.ToList();
        }

        /// <summary>
        /// 依照排序條件動態排序
        /// </summary>
        /// <param name="list">來源資料</param>
        /// <param name="sortCriteria">排序條件</param>
        /// <returns></returns>
        public static List<T> OrderBy<T>(this List<T> list, string Field, SortTypeEnum SortType)
        {
            IOrderedEnumerable<T>? orderedQuery = null;
            //首字轉成大寫
            Field = Field.Substring(0, 1).ToUpper() + Field.Substring(1);
            var propertyInfo = typeof(T).GetProperty(Field);
            if (propertyInfo != null)
            {
                orderedQuery = SortType == SortTypeEnum.Asc
                                      ? list.OrderBy(x => propertyInfo.GetValue(x, null))
                                      : list.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            return orderedQuery == null ? list : orderedQuery.ToList();
        }
    }
}
