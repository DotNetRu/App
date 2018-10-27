namespace DotNetRu.DataStore.Audit.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Category : BaseDataObject
    {
        /// <summary>
        /// Gets or sets the name that is displayed during filtering
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the short name/code that is displayed on the sessions page.
        /// For instance the short name for Xamarin.Forms is X.Forms
        /// </summary>
        /// <value>The short name.</value>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the color in Hex form, for instance #FFFFFF
        /// </summary>
        /// <value>The color.</value>
        public string Color { get; set; }

        public virtual ICollection<TalkModel> Sessions { get; set; }

        bool filtered;
        [JsonIgnore]
        public bool IsFiltered
        {
            get => this.filtered;
            set => this.SetProperty(ref this.filtered, value);
        }

        bool enabled;
        [JsonIgnore]
        public bool IsEnabled
        {
            get => this.enabled;
            set => this.SetProperty(ref this.enabled, value);
        }
        [JsonIgnore]
        public string BadgeName => string.IsNullOrWhiteSpace(this.ShortName) ? this.Name : this.ShortName; 
    }
}