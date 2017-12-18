using System.Globalization;

namespace XamarinEvolve.Clients.Portable.Interfaces
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();

        void SetLocale(CultureInfo cultureInfo);
    }
}
