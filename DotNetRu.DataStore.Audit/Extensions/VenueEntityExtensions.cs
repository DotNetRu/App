namespace DotNetRu.DataStore.Audit.Extensions
{
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class VenueEntityExtensions
    {
        public static VenueModel ToModel(this Venue talkEntity)
        {
            return new VenueModel
                       {
                           Id = talkEntity.Id,
                           Name = talkEntity.Name,
                           Address = talkEntity.Address,
                           MapUrl = talkEntity.MapUrl
                       };
        }
    }
}
