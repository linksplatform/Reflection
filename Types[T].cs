using System;

namespace Platform.Reflection
{
    public class Types<T> : Types
    {
        public static readonly Type[] Array = new Types<T>().ToArray();

        private Types() { }
    }
}
