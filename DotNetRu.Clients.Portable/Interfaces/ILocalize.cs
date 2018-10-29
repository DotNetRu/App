namespace XamarinEvolve.Clients.Portable.Interfaces
{
    using System.Globalization;

    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();

        void SetLocale(CultureInfo cultureInfo);
    }
}
