using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 自动匹配
    /// </summary>
    public static class AutoMapper
    {
        /// <summary>
        /// 自动转换方法实现
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        [Description("自动转换方法实现")]
        private static TDestination MapWork<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source is null)
                return Activator.CreateInstance<TDestination>();

            Type destinationType = typeof(TDestination);
            var properties = destinationType.GetProperties();

            Type sourceType = source.GetType();
            foreach (var property in properties)
            {
                if (sourceType.GetProperty(property.Name) != null && property.SetMethod != null)
                {
                    var props = sourceType.GetProperties();
                    var sourcePropertity = sourceType.GetProperties().FirstOrDefault(x => x.Name.ToUpper().Equals(property.Name.ToUpper()));
                    if (sourcePropertity != null && sourcePropertity.GetValue(source) != null)
                    {
                        var value = sourcePropertity.GetValue(source);
                        if (value != null && sourcePropertity.PropertyType.Name.EndsWith("[]") && property.PropertyType.Name.ToLower().Equals("string"))
                        {
                            byte[] bytes = (byte[])value;
                            value = string.Join(",", bytes);
                        }
                        property.SetValue(destination, value);
                    }
                }
            }
            return destination;
        }

        /// <summary>
        /// 自动转换
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [Description("自动转换")]
        public async static Task<TDestination> MapToAsync<TDestination>(this Object source)
        {
            return await Task.Run(() => MapWork(source, Activator.CreateInstance<TDestination>()));
        }

        /// <summary>
        /// 自动转换
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [Description("自动转换")]
        public static TDestination MapTo<TDestination>(this Object source)
        {
            return MapWork(source, Activator.CreateInstance<TDestination>());
        }

        /// <summary>
        /// 自动转换
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        [Description("自动转换")]
        public static TDestination MapTo<TDestination>(this Object source, ref TDestination destination)
        {
            if (destination is null)
                destination = Activator.CreateInstance<TDestination>();

            return MapWork(source, destination);
        }
    }
}
