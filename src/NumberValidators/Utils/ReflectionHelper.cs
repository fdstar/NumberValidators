using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Utils
{
    internal static class ReflectionHelper
    {
        private static readonly ConcurrentDictionary<string, Type> concurrentDictionary
            = new ConcurrentDictionary<string, Type>();
        /// <summary>
        /// 根据接口类型以及类名来获取对应的实现
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        internal static Type FindByInterface(Type interfaceType, string className)
        {
            string key = string.Format("{0}:{1}", interfaceType.FullName, className);
            return concurrentDictionary.GetOrAdd(key, _ => AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.Name.Equals(className) && t.GetInterfaces().Contains(interfaceType)))
                .FirstOrDefault());
        }
    }
}
