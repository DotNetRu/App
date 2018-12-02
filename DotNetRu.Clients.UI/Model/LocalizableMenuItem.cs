using DotNetRu.Clients.UI.ApplicationResources;

namespace DotNetRu.Clients.Portable.Model
{
    public class LocalizableMenuItem : MenuItem
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
            this.Name = AppResources.ResourceManager.GetString(this.ResourceName, AppResources.Culture);
        }
    }
}
