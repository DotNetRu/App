using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetRu.DataStore.Audit.Extensions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.RealmUpdateLibrary;
using DotNetRu.Utils.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Realms;
using Realms.Sync;
using Realms.Sync.Exceptions;
using Realms.Utils;
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

        private static Realm OnlineRealm { get; set; }

        private static string IsOnlineRealmCreated = nameof(IsOnlineRealmCreated);

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

        public static async Task InitializeCloudSync(string realmServer, string realmDatabase)
        {
            Console.WriteLine("Initialize cloud sync");

            var realmDatabaseUrl = new Uri($"realms://{realmServer}/{realmDatabase}");

            var shouldCleanOnlineRealm = VersionTracking.IsFirstLaunchForCurrentBuild;
#if DEBUG
            shouldCleanOnlineRealm = true;
#endif
            if (shouldCleanOnlineRealm)
            {
                DeleteOnlineRealm(realmDatabaseUrl);
                try
                {
                    await RealmHelpers.RemoveCachedUsers();
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e, new Dictionary<string, string>
                    {
                        {"Realm fatal error", "false" },
                        {"Error description", $"Failed to remove cached users. See {e.Message}" }
                    });
                }
            }

            Realms.Sync.Session.Error += HandlerRealmSyncErrors(realmDatabaseUrl);
            SyncConfigurationBase.UserAgent = $"{AppInfo.Name} ({AppInfo.PackageName} {AppInfo.VersionString})";

            var realmServerUri = new Uri($"https://{realmServer}");
            OnlineRealm = await GetCloudRealm(realmServerUri, realmDatabaseUrl);
        }

        public static async Task<Realm> GetCloudRealm(Uri realmServerUri, Uri realmDatabaseUri)
        {
            User user;
            try
            {
                user = await RealmHelpers.GetRealmUser(realmServerUri);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string>
                {
                    {"Realm fatal error", "true" },
                    {"Error description", $"Failed to create realm user. See {e.Message}" }
                });
                return null;
            }

            try
            {
                var syncConfiguration = new FullSyncConfiguration(realmDatabaseUri, user);

                var cloudRealm = await OpenCloudRealm(syncConfiguration);

                if (cloudRealm != null)
                {
                    RealmSyncManager.UpdateRealm(cloudRealm, OfflineRealm);

                    cloudRealm.Error += (s, e) =>
                    {
                        Crashes.TrackError(e.Exception, new Dictionary<string, string>
                            {
                                {"Realm fatal error", "unknown" },
                                {"Error description", $"Some error reported by online realm. See {e.Exception?.Message}" }
                            });
                    };

                    cloudRealm.RealmChanged += (s, e) =>
                    {
                        try
                        {
                            RealmSyncManager.UpdateRealm(cloudRealm, OfflineRealm);
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex, new Dictionary<string, string>
                                {
                                    {"Realm fatal error", "true" },
                                    {"Error description", $"Failed to update offline realm. See {ex.Message}" }
                                });
                        }
                    };
                }

                return cloudRealm;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string>
                    {
                        {"Realm fatal error", "unknown" },
                        {"Error description", $"Error on getting cloud realm. See {e.Message}" }
                    });
                return null;
            }
        }

        private static EventHandler<Realms.ErrorEventArgs> HandlerRealmSyncErrors(Uri realmDatabaseUri)
        {
            return async (s, errorArgs) =>
            {
                var sessionException = (SessionException)errorArgs.Exception;

                if (sessionException.ErrorCode.IsClientResetError())
                {
                    Analytics.TrackEvent("Realm client reset");

                    DeleteOnlineRealm(realmDatabaseUri);
                    await RealmHelpers.RemoveCachedUsers();

                    return;
                }

                switch (sessionException.ErrorCode)
                {
                    case ErrorCode.AccessTokenExpired:
                    case ErrorCode.BadUserAuthentication:
                    case ErrorCode.PermissionDenied:
                        await RealmHelpers.RemoveCachedUsers();
                        break;
                }

                Crashes.TrackError(sessionException, new Dictionary<string, string> {
                    { "ErrorType", sessionException.ErrorCode.ToString() },
                    {"Realm fatal error", "unknown" },
                    {"Error description", $"Realm sync error reported by realm. See {sessionException.Message}" }
                });
            };
        }

        private static async Task<Realm> OpenCloudRealm(FullSyncConfiguration syncConfiguration)
        {
            if (Settings.IsOnlineRealmCreated)
            {
                return Realm.GetInstance(syncConfiguration);
            }

            var realm = await Realm.GetInstanceAsync(syncConfiguration);

            Settings.IsOnlineRealmCreated = true;

            return realm;
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

        private static void DeleteOnlineRealm(Uri realmDatabaseUri)
        {
            try
            {
                Console.WriteLine("Try to remove cloud realm");

                var users = User.AllLoggedIn;
                if (users.Any())
                {
                    var syncConfiguration = new FullSyncConfiguration(realmDatabaseUri, users.First());
                    Realm.DeleteRealm(syncConfiguration);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string>
                    {
                        {"Realm fatal error", "true" },
                        {"Error description", $"Error on deleting cloud realm. See {e.Message}" }
                    });
            }
            finally
            {
                OnlineRealm = null;
                Settings.IsOnlineRealmCreated = false;
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
    }
}
