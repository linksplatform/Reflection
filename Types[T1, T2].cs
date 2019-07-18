using System;

namespace Platform.Reflection
{
    public class Types<T1, T2> : Types
    {
        public static readonly Type[] Array = new Types<T1, T2>().ToArray();

        private Types() { }
    }
}
