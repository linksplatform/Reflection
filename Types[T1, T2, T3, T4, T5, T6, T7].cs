﻿using System;
using System.Collections.Generic;

namespace Platform.Reflection
{
    public class Types<T1, T2, T3, T4, T5, T6, T7> : Types
    {
        public static readonly IList<Type> List = new Types<T1, T2, T3, T4, T5, T6, T7>().ToReadOnlyList();
        private Types() { }
    }
}