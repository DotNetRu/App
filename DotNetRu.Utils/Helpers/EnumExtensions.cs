using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetRu.Utils.Helpers
{
    public static class EnumExtension
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            var info = enumValue.GetType().GetRuntimeField(enumValue.ToString());
            return info?.GetCustomAttribute<TAttribute>();
        }

        public static string GetLanguageCode(this Enum enumValue)
        {
            // creturn "ru"; // for LivePlayer
            return enumValue.GetAttribute<LanguageCodeAttribute>().LanguageCode;
        }

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}