﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform.Collections.Lists;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Reflection
{
    public class Types<T1, T2, T3, T4, T5, T6, T7> : Types
    {
        public static ReadOnlyCollection<Type> Collection = new Types<T1, T2, T3, T4, T5, T6, T7>().ToReadOnlyCollection();
        public static Type[] Array => ((IList<Type>)Collection).ToArray();
        private Types() { }
    }
}
