using System;
using Platform.Reflection;

Console.WriteLine($"int? CanBeNumeric: {NumericType<int?>.CanBeNumeric}");
Console.WriteLine($"int? IsNumeric: {NumericType<int?>.IsNumeric}");
Console.WriteLine($"int? IsNullable: {NumericType<int?>.IsNullable}");
Console.WriteLine($"int? UnderlyingType: {NumericType<int?>.UnderlyingType}");

Console.WriteLine($"double? CanBeNumeric: {NumericType<double?>.CanBeNumeric}");
Console.WriteLine($"double? IsNumeric: {NumericType<double?>.IsNumeric}");

Console.WriteLine($"bool? CanBeNumeric: {NumericType<bool?>.CanBeNumeric}");
Console.WriteLine($"bool? IsNumeric: {NumericType<bool?>.IsNumeric}");

Console.WriteLine($"string? CanBeNumeric: {NumericType<string?>.CanBeNumeric}");
Console.WriteLine($"string? IsNumeric: {NumericType<string?>.IsNumeric}");
