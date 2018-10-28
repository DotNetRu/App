namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Helpers;
    using DotNetRu.DataStore.Audit.Models;

    public static class VenueService
    {
        private static IEnumerable<VenueModel> venues;

        public static IEnumerable<VenueModel> Venues => venues ?? (venues = GetVenues());

        private static IEnumerable<VenueModel> GetVenues()
        {
            var venueEntities = ParseHelper.ParseXml<VenueEntity>("Venues");
            return venueEntities.Select(venueEntity => venueEntity.ToModel());
        }
    }
}
