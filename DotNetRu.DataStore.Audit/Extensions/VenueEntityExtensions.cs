namespace DotNetRu.DataStore.Audit.Extensions
{
    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Models;

    public static class VenueEntityExtensions
    {
        public static VenueModel ToModel(this VenueEntity talkEntity)
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
