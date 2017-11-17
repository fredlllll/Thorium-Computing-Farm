using System;
using System.Collections.Generic;
using System.Data;

namespace Thorium_Shared.Data
{
    public static class Util
    {
        public static Dictionary<Type, DbType> TypeMap { get; } = new Dictionary<Type, DbType>();
        public static Dictionary<Type, string> TypeAffinityMap { get; } = new Dictionary<Type, string>();

        static Util()
        {
            TypeMap[typeof(byte)] = DbType.Byte;
            TypeMap[typeof(sbyte)] = DbType.SByte;
            TypeMap[typeof(short)] = DbType.Int16;
            TypeMap[typeof(ushort)] = DbType.UInt16;
            TypeMap[typeof(int)] = DbType.Int32;
            TypeMap[typeof(uint)] = DbType.UInt32;
            TypeMap[typeof(long)] = DbType.Int64;
            TypeMap[typeof(ulong)] = DbType.UInt64;
            TypeMap[typeof(float)] = DbType.Single;
            TypeMap[typeof(double)] = DbType.Double;
            TypeMap[typeof(decimal)] = DbType.Decimal;
            TypeMap[typeof(bool)] = DbType.Boolean;
            TypeMap[typeof(string)] = DbType.String;
            TypeMap[typeof(char)] = DbType.StringFixedLength;
            TypeMap[typeof(Guid)] = DbType.Guid;
            TypeMap[typeof(DateTime)] = DbType.DateTime;
            TypeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            TypeMap[typeof(byte[])] = DbType.Binary;

            string integer = "Integer";
            string text = "Text";
            string blob = "Blob";
            string real = "Real";
            string numeric = "Numeric";
            TypeAffinityMap[typeof(byte)] = integer;
            TypeAffinityMap[typeof(sbyte)] = integer;
            TypeAffinityMap[typeof(short)] = integer;
            TypeAffinityMap[typeof(ushort)] = integer;
            TypeAffinityMap[typeof(int)] = integer;
            TypeAffinityMap[typeof(uint)] = integer;
            TypeAffinityMap[typeof(long)] = integer;
            TypeAffinityMap[typeof(ulong)] = integer;
            TypeAffinityMap[typeof(float)] = real;
            TypeAffinityMap[typeof(double)] = real;
            TypeAffinityMap[typeof(decimal)] = numeric;
            TypeAffinityMap[typeof(bool)] = numeric;
            TypeAffinityMap[typeof(string)] = text;
            TypeAffinityMap[typeof(char)] = text;
            TypeAffinityMap[typeof(DateTime)] = numeric;
            TypeAffinityMap[typeof(DateTimeOffset)] = numeric;
            TypeAffinityMap[typeof(byte[])] = blob;
        }
    }
}
