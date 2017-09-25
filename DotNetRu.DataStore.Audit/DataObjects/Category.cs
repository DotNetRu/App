namespace XamarinEvolve.DataObjects
{
    using System.Collections.Generic;

    using DotNetRu.DataStore.Audit.DataObjects;

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

        public virtual ICollection<Session> Sessions { get; set; }

        bool filtered;
        [JsonIgnore]
        public bool IsFiltered
        {
            get { return filtered; }
            set { SetProperty(ref filtered, value); }
        }

        bool enabled;
        [JsonIgnore]
        public bool IsEnabled
        {
            get { return enabled; }
            set { SetProperty(ref enabled, value); }
        }
        [JsonIgnore]
        public string BadgeName => string.IsNullOrWhiteSpace(ShortName) ? Name : ShortName; 
    }
}