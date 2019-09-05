using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetRu.Clients.UI;
using DotNetRu.DataStore.Audit.Extensions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.Utils.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoreLinq;
using Realms;
using Realms.Sync;
using Realms.Sync.Exceptions;
using Xamarin.Essentials;
using static DotNetRu.DataStore.Audit.Services.RealmHelpers;

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

        private static readonly string RealmServerURL = "dotnet.de1a.cloud.realm.io";

        private static readonly Uri RealmServerUri = new Uri($"https://{RealmServerURL}");

        private static Uri RealmUri;

        private const string RealmOfflineResourceName = "DotNetRu.DataStore.Audit.DotNetRuOffline.realm";

        private static Realm OfflineRealm { get; set; }

        private static Realm OnlineRealm { get; set; }

        private static string SyncError = nameof(SyncError);
        private static string IsOnlineRealmCreated = nameof(IsOnlineRealmCreated);

        private static bool Initialized = false;

        public static void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            var config = AppConfig.GetConfig();

            RealmUri = new Uri($"realms://{RealmServerURL}/{config.RealmDatabaseKey}");

            if (VersionTracking.IsFirstLaunchForCurrentBuild)
            {
                CopyEmbeddedRealm();
                DeleteOnlineRealm();
            }

            OfflineRealm = Realm.GetInstance(GetOfflineRealmPath());
            OfflineRealm.Error += (s, e) =>
            {
                Crashes.TrackError(e.Exception);
            };

            // OfflineRealm.RealmChanged += (s, e) => MessagingCenter.Send(Instance, MessageKeys.RealmUpdated);

            Realms.Sync.Session.Error += HandlerRealmSyncErrors();
            SyncConfigurationBase.UserAgent = $"{AppInfo.Name} ({AppInfo.PackageName} {AppInfo.VersionString})";

            ResumeCloudSync();

            Initialized = true;
        }

        public static async void ResumeCloudSync()
        {
            try
            {
                if (OnlineRealm != null)
                {
                    return;
                }

                // get or create Realm user
                var user = User.AllLoggedIn.Any()
                    ? User.AllLoggedIn.First()
                    : await CreateRealmUser(RealmServerUri);

                var syncConfiguration = new FullSyncConfiguration(RealmUri, user);

                var cloudRealm = Preferences.Get(IsOnlineRealmCreated, false)
                    ? OpenRealmSync(syncConfiguration)
                    : await OpenRealmAsync(syncConfiguration);

                if (cloudRealm != null)
                {
                    cloudRealm.Error += (s, e) =>
                    {
                        Crashes.TrackError(e.Exception);
                    };

                    cloudRealm.RealmChanged += (s, e) => UpdateRealm(OfflineRealm, cloudRealm);
                }

                OnlineRealm = cloudRealm;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private static EventHandler<Realms.ErrorEventArgs> HandlerRealmSyncErrors()
        {
            return async (s, errorArgs) =>
            {
                Preferences.Set(SyncError, true);

                var sessionException = (SessionException)errorArgs.Exception;

                if (sessionException.ErrorCode.IsClientResetError())
                {
                    Analytics.TrackEvent("Client reset error");

                    DeleteOnlineRealm();
                    await RemoveCachedUsers();

                    return;
                }

                switch (sessionException.ErrorCode)
                {
                    case ErrorCode.AccessTokenExpired:
                    case ErrorCode.BadUserAuthentication:
                        await RemoveCachedUsers();
                        break;
                    case ErrorCode.PermissionDenied:
                        DeleteOnlineRealm();
                        await RemoveCachedUsers();
                        break;
                }

                Crashes.TrackError(sessionException, new Dictionary<string, string>() {
                    { "ErrorType", sessionException.ErrorCode.ToString() }
                });
            };
        }

        private static async Task<Realm> OpenRealmAsync(FullSyncConfiguration syncConfiguration)
        {
            try
            {
                Preferences.Set(SyncError, false);

                var realm = await Realm.GetInstanceAsync(syncConfiguration);

                // TODO report as github issue
                if (Preferences.Get(SyncError, defaultValue: false))
                {
                    Crashes.TrackError(new Exception("Failed to initiate first time cloud realm connection"));
                    DeleteOnlineRealm();

                    // stick with offline realm
                    return null;
                }
                else
                {
                    Preferences.Set(IsOnlineRealmCreated, true);
                    UpdateRealm(OfflineRealm, realm);

                    return realm;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        private static Realm OpenRealmSync(FullSyncConfiguration syncConfiguration)
        {
            return Realm.GetInstance(syncConfiguration);
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

        private static void DeleteOnlineRealm()
        {
            try
            {
                CloseRealmSafely(OnlineRealm);

                var users = User.AllLoggedIn;
                if (users.Any())
                {
                    var syncConfiguration = new FullSyncConfiguration(RealmUri, users.First());
                    Realm.DeleteRealm(syncConfiguration);
                }

                Analytics.TrackEvent("Online realm removed");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                OnlineRealm = null;
                Preferences.Set(IsOnlineRealmCreated, false);
            }
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

        private static void UpdateRealm(Realm offline, Realm online)
        {
            try
            {
                using (var transaction = offline.BeginWrite())
                {
                    MoveRealmObjects<AuditVersion, string>(online, offline, x => x.CommitHash);
                    MoveRealmObjects<Community, string>(online, offline, x => x.Id);
                    MoveRealmObjects<Friend, string>(online, offline, x => x.Id);
                    MoveRealmObjects<Meetup, string>(online, offline, x => x.Id);
                    MoveRealmObjects<RealmModels.Session, string>(online, offline, x => x.Id);
                    MoveRealmObjects<Speaker, string>(online, offline, x => x.Id);
                    MoveRealmObjects<Talk, string>(online, offline, x => x.Id);
                    MoveRealmObjects<Venue, string>(online, offline, x => x.Id);

                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}
