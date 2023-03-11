using System.Globalization;

namespace DotNetRu.Clients.Portable.Helpers
{
    public class CultureChangedMessage
    {
        public CultureChangedMessage(string lngName)
            : this(new CultureInfo(lngName))
        {

        }

        public CultureChangedMessage(CultureInfo newCultureInfo)
        {
            this.NewCultureInfo = newCultureInfo;
        }

        public CultureInfo NewCultureInfo { get; }
    }
}
