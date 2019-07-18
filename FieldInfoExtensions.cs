﻿using System.Reflection;
using System.Runtime.CompilerServices;

namespace Platform.Reflection
{
    public static class FieldInfoExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetStaticValue<T>(this FieldInfo fieldInfo) => (T)fieldInfo.GetValue(null);
    }
}
