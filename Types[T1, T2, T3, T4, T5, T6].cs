using System;

namespace Platform.Reflection
{
    public class Types<T1, T2, T3, T4, T5, T6> : Types
    {
        public static readonly Type[] Array = new Types<T1, T2, T3, T4, T5, T6>().ToArray();

        private Types() { }
    }
}
