using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;

using DotNetRu.DataStore.Audit.Extensions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.Utils.Helpers;
using Realms;
using Xamarin.Essentials;

namespace DotNetRu.DataStore.Audit.Services
{
    public class RealmService
    {
        private const string RealmResourceName = "DotNetRu.DataStore.Audit.DotNetRuOffline.realm";
        private static Realm auditRealm;

        public static Realm AuditRealm => auditRealm ?? (auditRealm = Realm.GetInstance("Audit.realm"));

        public static void Initialize()
        {
            InitializeRealm();
            InitializeAutoMapper();
        }

        public static IEnumerable<TAppModel> Get<TAppModel>()
        {
            var realmType = Mapper.Configuration.GetAllTypeMaps().Single(x => x.DestinationType == typeof(TAppModel)).SourceType;

            return AuditRealm.All(realmType.Name).ToList().Select(Mapper.Map<TAppModel>);
        }

        private static void InitializeAutoMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<Speaker, SpeakerModel>().ConvertUsing(x => x.ToModel());
                    cfg.CreateMap<Venue, VenueModel>().ConvertUsing(x => x.ToModel());
                    cfg.CreateMap<Friend, FriendModel>().ConvertUsing(x => x.ToModel());
                    cfg.CreateMap<Talk, TalkModel>().ConvertUsing(x => x.ToModel());
                    cfg.CreateMap<Session, SessionModel>().ConvertUsing(x => x.ToModel());
                    cfg.CreateMap<Meetup, MeetupModel>().ConvertUsing(x => x.ToModel());
                });
        }

        private static void InitializeRealm()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var realmPath = Path.Combine(documentsPath, "Audit.realm");

            if (VersionTracking.IsFirstLaunchForCurrentBuild)
            {
                File.WriteAllBytes(realmPath, ResourceHelper.ExtractResource(RealmResourceName));
            }
        }
    }
}
