using Xamarin.Forms;

namespace DotNetRu.Clients.UI.About
{
    public class AboutTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CommunityTemplate { get; set; }

        public DataTemplate MenuItemTemplate { get; set; }

        public DataTemplate SettingsTemplate { get; set; }

        public DataTemplate FriendsTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var aboutPageItem = item as AboutPageItem;

            switch (aboutPageItem.AboutItemType)
            {
                case AboutItemType.Community:
                    return CommunityTemplate;
                case AboutItemType.MenuItem:
                    return MenuItemTemplate;
                case AboutItemType.Settings:
                    return SettingsTemplate;
                case AboutItemType.Friends:
                    return FriendsTemplate;
                default:
                    return CommunityTemplate;
            }
        }
    }
}
