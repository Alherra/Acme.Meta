using Meta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// Linq扩展方法
    /// </summary>
    [Description("Linq扩展方法")]
    public static class LinqExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源</param>
        /// <param name="SkipCount">页码</param>
        /// <param name="PageSize">页条数</param>
        /// <returns></returns>
        [Description("分页扩展")]
        public static PagedResult<T> ToPageList<T>(this IEnumerable<T> source, int SkipCount, int PageSize)
        {
            if (SkipCount <= 0) SkipCount = 1;
            if (PageSize <= 0) PageSize = 10;

            var result = source
                .Skip((SkipCount - 1) * PageSize)
                .Take(PageSize);
            return new PagedResult<T>(source.Count(), result.ToList());
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源</param>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [Description("分页扩展")]
        public static PagedResult<T> ToPageList<T>(this IEnumerable<T> source, PagedInput input)
        {
            if (input.SkipCount <= 0) input.SkipCount = 1;
            if (input.PageSize <= 0) input.PageSize = 10;

            var result = source
                .Skip((input.SkipCount - 1) * input.PageSize)
                .Take(input.PageSize);
            return new PagedResult<T>(source.Count(), result.ToList());
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源</param>
        /// <param name="SkipCount">页码</param>
        /// <param name="PageSize">页条数</param>
        /// <returns></returns>
        [Description("分页扩展")]
        public static PagedResult<T> ToPageList<T>(this IEnumerable<object> source, int SkipCount, int PageSize)
        {
            if (SkipCount <= 0) SkipCount = 1;
            if (PageSize <= 0) PageSize = 10;

            var result = source
                .Skip((SkipCount - 1) * PageSize)
                .Take(PageSize);
            return new PagedResult<T>(source.Count(), result.Select(x => x.MapTo<T>()).ToList());
        }
    }
}
