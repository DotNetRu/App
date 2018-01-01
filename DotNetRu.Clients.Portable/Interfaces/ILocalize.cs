using System.Globalization;

namespace DotNetRu.Clients.Portable.Interfaces
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();

        void SetLocale(CultureInfo cultureInfo);
    }
}
