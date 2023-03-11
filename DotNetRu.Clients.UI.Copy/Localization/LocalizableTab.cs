using System.Threading.Tasks;
using Microsoft.Maui;

namespace DotNetRu.Clients.UI.Localization
{
    public class LocalizableTab : Tab
    {
        private string resourceName;

        public string ResourceName
        {
            get => this.resourceName;
            set
            {
                this.resourceName = value;
                this.Update();
            }
        }

        public void Update()
        {
            this.Title = AppResources.ResourceManager.GetString(this.ResourceName, AppResources.Culture);
        }

        protected override Task<Page> OnPopAsync(bool animated)
        {
            // temporary workaround while https://github.com/xamarin/Xamarin.Forms/issues/8581 not fixed
            return base.OnPopAsync(animated: false);
        }
    }
}
