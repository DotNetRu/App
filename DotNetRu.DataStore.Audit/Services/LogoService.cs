namespace DotNetRu.DataStore.Audit.Services
{
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    [Preserve]
    public class LogoService
    {
        private static string logoPath = "DotNetRu.DataStore.Audit.Images.logos.";

        public static ImageSource EkbDotNetLogo => ImageSource.FromResource(logoPath + "EkbDotNet.png");        

        public static ImageSource KryDotNetLogo => ImageSource.FromResource(logoPath + "KryDotNet.png");

        public static ImageSource KznDotNetLogo => ImageSource.FromResource(logoPath + "KznDotNet.png");

        public static ImageSource MskDotNetLogo => ImageSource.FromResource(logoPath + "MskDotNet.png");

        public static ImageSource NskDotNetLogo => ImageSource.FromResource(logoPath + "NskDotNet.png");

        public static ImageSource OmsDotNetLogo => ImageSource.FromResource(logoPath + "OmsDotNet.png");

        public static ImageSource SarDotNetLogo => ImageSource.FromResource(logoPath + "SarDotNet.png");

        public static ImageSource SpbDotNetLogo => ImageSource.FromResource(logoPath + "SpbDotNet.png");                
    }
}
