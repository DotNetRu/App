namespace DotNetRu.DataStore.Audit.Extensions
{
    using System.Linq;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class SessionExtension
    {
        public static SessionModel ToModel(this Session session)
        {
            return new SessionModel
            {
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Talk = session.Talk.ToModel(),

                // Meetup = session.Meetup.Single().ToModel()
            };
        }
    }
}