using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinEvolve.Clients.UI
{
    public class CultureChangedMessage
    {
        public CultureInfo NewCultureInfo { get; private set; }

        public CultureChangedMessage(string lngName)
            : this(new CultureInfo(lngName))
        { }

        public CultureChangedMessage(CultureInfo newCultureInfo)
        {
            NewCultureInfo = newCultureInfo;
        }
    }
}
