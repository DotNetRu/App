using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using DotNetRu.DataStore.Audit.Extensions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.AppUtils;
using Microsoft.AppCenter.Crashes;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DotNetRu.DataStore.Audit.Services
{
    public class RealmService
    {
        private static readonly Lazy<RealmService> lazy =
            new Lazy<RealmService>(() => new RealmService());

        public static RealmService Instance { get { return lazy.Value; } }

        private RealmService()
        {
        }

        private const string RealmOfflineResourceName = "DotNetRuOffline.realm";

        public static Realm OfflineRealm { get; set; }

        public static void InitializeOfflineDatabase()
        {
            var shouldCopyEmbeddedRealm = VersionTracking.IsFirstLaunchForCurrentBuild;
#if DEBUG
            shouldCopyEmbeddedRealm = true;
#endif
            if (shouldCopyEmbeddedRealm)
            {
                CopyEmbeddedRealm();
            }

            OfflineRealm = Realm.GetInstance(GetOfflineRealmPath());
            OfflineRealm.Error += (s, e) =>
            {
                Crashes.TrackError(e.Exception, new Dictionary<string, string>
                    {
                        {"Realm fatal error", "unknown" },
                        {"Error description", $"Some error reported by offline realm. See {e.Exception?.Message}" }
                    });
            };

            OfflineRealm.RealmChanged += (s, e) => MessagingCenter.Send(Instance, MessageKeys.RealmUpdated);
        }

        private static void CopyEmbeddedRealm()
        {
            File.WriteAllBytes(GetOfflineRealmPath(), ResourceHelper.ExtractResource(RealmOfflineResourceName));
        }

        private static string GetOfflineRealmPath()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(documentsPath, "ConferenceOffline.realm");
        }

        private static MapperConfiguration MapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Speaker, SpeakerModel>().ConvertUsing(x => x.ToModel());
            cfg.CreateMap<Venue, VenueModel>().ConvertUsing(x => x.ToModel());
            cfg.CreateMap<Friend, FriendModel>().ConvertUsing(x => x.ToModel());
            cfg.CreateMap<Talk, TalkModel>().ConvertUsing(x => x.ToModel());
            cfg.CreateMap<RealmModels.Session, SessionModel>().ConvertUsing(x => x.ToModel());
            cfg.CreateMap<Meetup, MeetupModel>().ConvertUsing(x => x.ToModel());
            cfg.CreateMap<Community, CommunityModel>().ConvertUsing(x => x.ToModel());
        });

        public static IEnumerable<TAppModel> Get<TAppModel>()
        {
            var realmType = MapperConfiguration.GetAllTypeMaps().Single(x => x.DestinationType == typeof(TAppModel)).SourceType;

            var mapper = MapperConfiguration.CreateMapper();

            return OfflineRealm.All(realmType.Name).ToList().Select(mapper.Map<TAppModel>);
        }

        public static IEnumerable<TAppModel> Get<TAppModel>(Realm realm)
        {
            var realmType = MapperConfiguration.GetAllTypeMaps().Single(x => x.DestinationType == typeof(TAppModel)).SourceType;

            var mapper = MapperConfiguration.CreateMapper();

            return realm?.All(realmType.Name).ToList().Select(mapper.Map<TAppModel>);
        }
    }
}
