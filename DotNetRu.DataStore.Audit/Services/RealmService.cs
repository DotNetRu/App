namespace DotNetRu.DataStore.Audit.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AutoMapper;

    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    using Realms;

    public class RealmService
    {
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

            return AuditRealm.All(realmType.Name).AsEnumerable().Select(Mapper.Map<TAppModel>);
        }

        private static void InitializeRealm()
        {
            var realmDB = "Audit.realm";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            File.WriteAllBytes(Path.Combine(documentsPath, realmDB), ExtractRealmBytes());
        }

        private static void InitializeAutoMapper()
        {
            Mapper.Initialize(
                cfg =>
                    {
                        cfg.CreateMap<Speaker, SpeakerModel>().ConvertUsing(x => x.ToModel());
                        cfg.CreateMap<Venue, VenueModel>().ConvertUsing(x => x.ToModel());
                        cfg.CreateMap<Friend, FriendModel>().ConvertUsing(x => x.ToModel());
                        cfg.CreateMap<Talk, TalkModel>().ConvertUsing(x => x.ToModel());
                        cfg.CreateMap<Meetup, MeetupModel>().ConvertUsing(x => x.ToModel());
                    });
        }

        private static byte[] ExtractRealmBytes()
        {
            var assembly = typeof(RealmService).Assembly;
            using (Stream resFilestream = assembly.GetManifestResourceStream("DotNetRu.DataStore.Audit.Audit.realm"))
            {
                if (resFilestream == null)
                {
                    return null;
                }

                byte[] resultBytes = new byte[resFilestream.Length];
                resFilestream.Read(resultBytes, 0, resultBytes.Length);
                return resultBytes;
            }
        }
    }
}
