namespace DotNetRu.Clients.Portable.Model
{
    using DotNetRu.DataStore.Audit.Services;
    using DotNetRu.Utils.Helpers;

    public static class Communities
    {
        public static CommunityModel SaintPetersburg =>
            new CommunityModel
                {
                    Name = "SaintPetersburg",
                    VKLink = AboutThisApp.SpbLink,
                    ImageSource = LogoService.SpbDotNetLogo
                };

        public static CommunityModel Moscow =>
            new CommunityModel
                {
                    Name = "Moscow",
                    VKLink = AboutThisApp.MoscowLink,
                    ImageSource = LogoService.MskDotNetLogo
                };

        public static CommunityModel Kazan =>
            new CommunityModel
                {
                    Name = "Kazan",
                    VKLink = AboutThisApp.KazanLink,
                    ImageSource = LogoService.KznDotNetLogo
                };

        public static CommunityModel Krasnoyarksk =>
            new CommunityModel
                {
                    Name = "Krasnoyarksk",
                    VKLink = AboutThisApp.KrasnoyarskLink,
                    ImageSource = LogoService.KryDotNetLogo
                };

        public static CommunityModel Saratov =>
            new CommunityModel
                {
                    Name = "Saratov",
                    VKLink = AboutThisApp.SaratovLink,
                    ImageSource = LogoService.SarDotNetLogo
                };
    }
}
