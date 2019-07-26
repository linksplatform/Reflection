using System;
using System.Collections.Generic;

namespace Platform.Reflection
{
    public class Types<T> : Types
    {
        public static readonly IList<Type> Array = new Types<T>().ToReadOnlyList();
        private Types() { }
    }
}
