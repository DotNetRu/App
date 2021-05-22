using System;
using System.Collections.Generic;
using System.Linq;
using DotNetRu.DataStore.Audit.RealmModels;
using MoreLinq;
using Realms;
using Realms.Utils;

namespace DotNetRu.RealmUpdateLibrary
{
    public static class DotNetRuRealmHelper
    {
        // TODO support deletion
        public static void UpdateRealm(Realm realm, AuditXmlUpdate auditXmlUpdate)
        {
            var auditData = GetAuditUpdate(realm, auditXmlUpdate);

            ReplaceRealmObjects(realm, new[] { auditData.AuditVersion }, x => x.CommitHash);

            using var transaction = realm.BeginWrite();

            UpdateRealmObjects(realm, auditData.Communities);
            UpdateRealmObjects(realm, auditData.Friends);
            UpdateRealmObjects(realm, auditData.Speakers);
            UpdateRealmObjects(realm, auditData.Talks);
            UpdateRealmObjects(realm, auditData.Venues);
            UpdateRealmObjects(realm, auditData.Meetups);

            UpdateRealmObjects(realm, auditData.Meetups.SelectMany(m => m.Sessions));

            transaction.Commit();
        }

        public static void ReplaceRealm(Realm realm, AuditXmlUpdate auditXmlUpdate)
        {
            var auditData = GetAuditUpdate(realm, auditXmlUpdate);

            ReplaceRealmObjects(realm, new[] { auditData.AuditVersion }, x => x.CommitHash);
            ReplaceRealmObjects(realm, auditData.Communities, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Friends, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Speakers, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Talks, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Venues, x => x.Id);
            ReplaceRealmObjects(realm, auditData.Meetups, x => x.Id);

            ReplaceRealmObjects(realm, auditData.Meetups.SelectMany(m => m.Sessions), x => x.Id);
        }

        private static AuditUpdate GetAuditUpdate(Realm realm, AuditXmlUpdate auditXmlUpdate)
        {
            var mapper = MapperManager.GetAutoMapper();

            var realmSpeakers = auditXmlUpdate.Speakers.Select(mapper.Map<Speaker>);

            var realmFriends = auditXmlUpdate.Friends.Select(mapper.Map<Friend>);

            var realmVenues = auditXmlUpdate.Venues.Select(mapper.Map<Venue>);

            var realmCommunities = auditXmlUpdate.Communities.Select(mapper.Map<Community>);

            var talkMapper = MapperManager.GetTalkMapper(realm);
            var realmTalks = auditXmlUpdate.Talks.Select(talkMapper.Map<Talk>);

            var meetupMapper = MapperManager.GetMeetupMapper(realm);
            var realmMeetups = auditXmlUpdate.Meetups.Select(meetupMapper.Map<Meetup>);

            var auditVersion = new AuditVersion
            {
                CommitHash = auditXmlUpdate.ToCommitSha
            };

            return new AuditUpdate
            {
                AuditVersion = auditVersion,
                Venues = realmVenues,
                Communities = realmCommunities,
                Friends = realmFriends,
                Meetups = realmMeetups,
                Speakers = realmSpeakers,
                Talks = realmTalks
            };
        }

        private static void UpdateRealmObjects<T>(Realm realm, IEnumerable<T> newObjects) where T : RealmObject
        {
            foreach (var @object in newObjects)
            {
                realm.Add(@object.Clone(), update: true);
            }
        }

        private static void ReplaceRealmObjects<T, TKey>(Realm realm, IEnumerable<T> newObjects, Func<T, TKey> keySelector) where T : RealmObject
        {
            // TODO use primary key
            var oldObjects = realm.All<T>().ToList();

            var objectsToRemove = oldObjects.ExceptBy(newObjects, keySelector).ToList();

            foreach (var @object in objectsToRemove)
            {
                realm.Write(() => realm.Remove(@object));
            }

            foreach (var @object in newObjects)
            {
                realm.Write(() => realm.Add(@object.Clone(), update: true));
            }
        }
    }
}
