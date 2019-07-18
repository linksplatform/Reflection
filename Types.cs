using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Platform.Reflection
{
    public abstract class Types
    {
        private static readonly ConcurrentDictionary<Type, Type[]> Cache = new ConcurrentDictionary<Type, Type[]>();

        protected Type[] ToArray()
        {
            var array = GetType().GetGenericArguments();

            var list = new List<Type>();
            AppendTypes(list, array);
            return list.ToArray();
        }

        private void AppendTypes(List<Type> list, Type[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var element = array[i];

                if (element == typeof(Types))
                    continue;

                if (element.IsSubclassOf(typeof(Types)))
                    AppendTypes(list, element.GetFirstField().GetStaticValue<Type[]>());
                else
                    list.Add(element);
            }
        }

        public static Type[] Get<T>()
        {
            var type = typeof(T);

            return Cache.GetOrAdd(type, t =>
            {
                if (type == typeof(Types))
                    return new Type[0];
                if (type.IsSubclassOf(typeof(Types)))
                    return type.GetFirstField().GetStaticValue<Type[]>();
                return new[] { type };
            });
        }
    }
}
