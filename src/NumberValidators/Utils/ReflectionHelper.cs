using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Utils
{
    internal static class ReflectionHelper
    {
        private static readonly ConcurrentDictionary<string, object> concurrentDictionary
            = new ConcurrentDictionary<string, object>();
        /// <summary>
        /// 根据接口类型以及类名来获取对应的实现
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        internal static object FindByInterface(Type interfaceType, string className)
        {
            string key = string.Format("{0}:{1}", interfaceType.FullName, className);
            return concurrentDictionary.GetOrAdd(key, _ =>
            {
                var type = AppDomain.CurrentDomain.GetAssemblies()
                     .SelectMany(a => a.GetTypes().Where(t => t.Name.Equals(className) && t.GetInterface(interfaceType.Name) != null))
                     .FirstOrDefault();
                return type?.Assembly.CreateInstance(type.FullName);
            });
        }
        /// <summary>
        /// 获取指定类名的接口实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="className"></param>
        /// <returns></returns>
        internal static T FindByInterface<T>(string className)
        {
            return (T)FindByInterface(typeof(T), className);
        }
    }
}
