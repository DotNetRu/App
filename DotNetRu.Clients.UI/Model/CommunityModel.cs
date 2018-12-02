namespace DotNetRu.Clients.Portable.Model
{
    using DotNetRu.Clients.UI.ApplicationResources;
    using Xamarin.Forms;

    public class CommunityModel
    {
        public string VKLink { get; set; }

        public string Name { get; set; }

        public string LocalizedName => AppResources.ResourceManager.GetString(this.Name, AppResources.Culture);

        public ImageSource ImageSource { get; set; }
    }
}
