namespace XamarinEvolve.Utils.Helpers
{
    using System;
    
    [AttributeUsage(AttributeTargets.Field)]
    public class LanguageCodeAttribute : Attribute
    {
        public string LanguageCode { get; set; }
    }
}
