namespace DotNetRu.DataStore.Audit.Services
{
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    [Preserve]
    public class LogoService
    {
        private static string logoPath = "DotNetRu.DataStore.Audit.Storage.logos.";

        public static ImageSource DotNetRuLogo => ImageSource.FromResource(logoPath + "DotNetRu.png");

        public static ImageSource SpbDotNetLogo => ImageSource.FromResource(logoPath + "SpbDotNet.png");

        public static ImageSource EkbDotNetLogo => ImageSource.FromResource(logoPath + "EkbDotNet.png");

        public static ImageSource KryDotNetLogo => ImageSource.FromResource(logoPath + "KryDotNet.png");

        public static ImageSource MskDotNetLogo => ImageSource.FromResource(logoPath + "MskDotNet.png");

        public static ImageSource SarDotNetLogo => ImageSource.FromResource(logoPath + "SarDotNet.png");

        public static ImageSource KznDotNetLogo => ImageSource.FromResource(logoPath + "KznDotNet.png");
    }
}
