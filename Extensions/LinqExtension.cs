using Meta;
using Newtonsoft.Json;
using System.ComponentModel;

namespace System.Linq
{
    /// <summary>
    /// Linq Extension Functions.
    /// </summary>
    [Description("Linq-Extension")]
    public static class LinqExtension
    {
        /// <summary>
        /// Returns an instance of the target class and make the same field-values.
        /// </summary>
        /// <typeparam name="Target">The target type.</typeparam>
        /// <param name="source">The source data.</param>
        /// <returns></returns>
        public static Target MapTo<Target>(this object source) => Activator.CreateInstance<Target>().MapFrom(source);

        /// <summary>
        /// Returns and set field-values from an object.
        /// </summary>
        /// <typeparam name="Target">The target type.</typeparam>
        /// <param name="target">The original object.</param>
        /// <param name="source">The source data.</param>
        /// <returns></returns>
        public static Target MapFrom<Target>(this Target target, object source) => Mapper(source, target);

        /// <summary>
        /// Return the field-values mapped object from one to another.
        /// </summary>
        /// <typeparam name="Source">The source type.</typeparam>
        /// <typeparam name="Target">The target type.</typeparam>
        /// <param name="source">The source data.</param>
        /// <param name="target">The original object.</param>
        /// <returns></returns>
        private static Target Mapper<Source, Target>(Source source, Target target)
        {
            if (typeof(Source) == typeof(Target))
                return JsonConvert.DeserializeObject<Target>(JsonConvert.SerializeObject(source))!;

            if (target is null)
                target = Activator.CreateInstance<Target>();

            if (source is null)
                return target;

            var sourceProperties = source.GetType().GetProperties().ToDictionary(p => p.Name, p => p);

            foreach (var property in target!
                .GetType()
                .GetProperties()
                .Where(p => p.CanWrite && sourceProperties.ContainsKey(p.Name)))
            {

                try
                {
                    var sourceProperty = sourceProperties[property.Name];
                    var value = sourceProperty.GetValue(source);

                    var propertyType = property.PropertyType;
                    if (propertyType == sourceProperty)
                    {
                        property.SetValue(target, value);
                        continue;
                    }

                    if (value is null)
                    {
                        property.SetValue(target, propertyType == typeof(Nullable<>) ? null : default);
                        continue;
                    }

                    if (!propertyType.IsValueType && propertyType == typeof(Nullable<>) && propertyType.GenericTypeArguments.Any())
                        propertyType = property.PropertyType.GenericTypeArguments[0];

                    property.SetValue(target, Convert.ChangeType(value, propertyType));
                }
                catch { continue; }
            }
            return target;
        }

        /// <summary>
        /// Returns a paged reuslt.
        /// </summary>
        /// <typeparam name="T">Any data type.</typeparam>
        /// <param name="source">The list to be paged by.</param>
        /// <param name="input">The paged by input data.</param>
        /// <returns></returns>
        [Description("Paged-Extension")]
        public static PagedResult<T> ToPageList<T>(this IEnumerable<T> source, PagedInput input)
        {
            if (input.PageNum <= 0) input.PageNum = 1;
            if (input.PageSize <= 0) input.PageSize = 10;

            var result = source
                .Skip((input.PageNum - 1) * input.PageSize)
                .Take(input.PageSize);
            return new PagedResult<T>(source.Count(), result.ToList());
        }

        /// <summary>
        /// Returns paged reuslt.
        /// </summary>
        /// <typeparam name="T">Any data type.</typeparam>
        /// <param name="source">The list to be paged by.</param>
        /// <param name="pageNum">The index num of the page.</param>
        /// <param name="pageSize">The size num of the page.</param>
        /// <returns></returns>
        [Description("Paged-Extension")]
        public static PagedResult<T> ToPageList<T>(this IEnumerable<T> source, int pageNum, int pageSize)
        {
            if (pageNum <= 0) pageNum = 1;
            if (pageSize <= 0) pageSize = 10;

            var result = source
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
            return new PagedResult<T>(source.Count(), result.ToList());
        }

        /// <summary>
        /// Returns paged reuslt.
        /// </summary>
        /// <typeparam name="T">Any data type.</typeparam>
        /// <param name="source">The list to be paged by.</param>
        /// <param name="pageNum">The index num of the page.</param>
        /// <param name="pageSize">The size num of the page.</param>
        /// <returns></returns>
        [Description("Paged-Extension")]
        public static PagedResult<T> ToPageList<T>(this IEnumerable<object> source, int pageNum, int pageSize)
        {
            if (pageNum <= 0) pageNum = 1;
            if (pageSize <= 0) pageSize = 10;

            var result = source
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);

            if (typeof(T) == source.GetType().GenericTypeArguments[0])
                return new PagedResult<T>(source.Count(), result.Select(x => (T)x).ToList());

            return new PagedResult<T>(source.Count(), result.Select(x => x.MapTo<T>()).ToList());
        }
    }
}
