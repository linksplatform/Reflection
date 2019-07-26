using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Platform.Reflection
{
    public abstract class Types
    {
        private static readonly ConcurrentDictionary<Type, IList<Type>> Cache = new ConcurrentDictionary<Type, IList<Type>>();

        protected IList<Type> ToReadOnlyList()
        {
            var types = GetType().GetGenericArguments();
            var result = new List<Type>();
            AppendTypes(result, types);
            return new ReadOnlyCollection<Type>(result);
        }

        private void AppendTypes(List<Type> container, IList<Type> types)
        {
            for (var i = 0; i < types.Count; i++)
            {
                var element = types[i];
                if (element != typeof(Types))
                {
                    if (element.IsSubclassOf(typeof(Types)))
                    {
                        AppendTypes(container, element.GetFirstField().GetStaticValue<IList<Type>>());
                    }
                    else
                    {
                        container.Add(element);
                    }
                }
            }
        }

        public static IList<Type> Get<T>()
        {
            return Cache.GetOrAdd(typeof(T), type =>
            {
                if (type == typeof(Types))
                {
                    return Array.AsReadOnly(new Type[0]);
                }
                if (type.IsSubclassOf(typeof(Types)))
                {
                    return type.GetFirstField().GetStaticValue<IList<Type>>();
                }
                return Array.AsReadOnly(new[] { type });
            });
        }
    }
}
