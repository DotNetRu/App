using System;

namespace DotNetRu.Utils.Helpers
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LanguageCodeAttribute : Attribute
    {
        public string LanguageCode { get; set; }
    }
}
