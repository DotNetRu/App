namespace DotNetRu.DataStore.Audit.Models
{
    using System;

    public class Room
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public Uri ImageUri 
        { 
            get 
            { 
                try
                {
                    return new Uri(ImageUrl);
                }
                catch
                {

                }
                return null;
            } 
        }
    }
}