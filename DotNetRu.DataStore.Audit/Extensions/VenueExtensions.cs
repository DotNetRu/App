namespace DotNetRu.DataStore.Audit.Extensions
{
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class VenueExtensions
    {
        public static VenueModel ToModel(this Venue venueEntity)
        {
            return new VenueModel
            {
                Id = venueEntity.Id,
                Name = venueEntity.Name,
                Address = venueEntity.Address,
                MapUrl = venueEntity.MapUrl
            };
        }
    }
}
