using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bank.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNullOrEmpty<T>(this T value)
        {
            if (typeof(T) == typeof(string))
                return string.IsNullOrEmpty(value as string);

            return value == null || value.Equals(default(T));
        }

        public static object GetProperty(this object obj, string name)
        {
            Type myType = obj.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name != name) continue;

                return prop.GetValue(obj, null);
            }

            return null;
        }
    }
}
