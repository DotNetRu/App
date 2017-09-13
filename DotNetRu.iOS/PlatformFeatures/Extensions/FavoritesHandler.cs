using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Newtonsoft.Json;
using Xamarin.Forms;
using XamarinEvolve.Utils;
using XamarinEvolve.iOS.PlatformFeatures.Extensions;
using NotificationCenter;

[assembly: Dependency(typeof(FavoritesHandler))]

namespace XamarinEvolve.iOS.PlatformFeatures.Extensions
{
    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils.Helpers;

	public class FavoritesHandler : IPlatformSpecificDataHandler<Session>
	{
		public Task UpdateMultipleEntities(IEnumerable<Session> data)
		{
			return Task.Run(() =>
            {
                var fileManager = new NSFileManager();
                var appGroupContainer = fileManager.GetContainerUrl($"group.{AboutThisApp.PackageName}");
                var appGroupContainerPath = appGroupContainer.Path;
                var sessionsFilePath = Path.Combine(appGroupContainerPath, "sessions.json");

                if (fileManager.FileExists(sessionsFilePath))
                {
                    NSError error;
                    fileManager.Remove(sessionsFilePath, out error);
                }

                var attributes = new NSFileAttributes
                {
                    Type = NSFileType.Regular
                };

                var json = JsonConvert.SerializeObject(data.Where(s => s.IsFavorite).ToList());
                var fileData = NSData.FromString(json);

                fileManager.CreateFile(sessionsFilePath, fileData, attributes);

                var url = NSUrl.FromFilename(sessionsFilePath);
                url.SetResource(NSUrl.IsExcludedFromBackupKey, new NSNumber(true));

                var settings = new NSUserDefaults($"group.{AboutThisApp.PackageName}", NSUserDefaultsType.SuiteName);
                settings.SetBool(true, "FavoritesInitialized");
                settings.Synchronize();

                UpdateWidget();
            });
		}

        private static void UpdateWidget()
        {
            var widgetController = NCWidgetController.GetWidgetController();
            widgetController?.SetHasContent(true, $"{AboutThisApp.PackageName}.upnext");
        }

        public async Task UpdateSingleEntity(Session entity)
		{
			List<Session> sessions = null;

			await Task.Run(() =>
			{
				var fileManager = new NSFileManager();
				var appGroupContainer = fileManager.GetContainerUrl($"group.{AboutThisApp.PackageName}");
				var appGroupContainerPath = appGroupContainer.Path;
				var sessionsFilePath = Path.Combine(appGroupContainerPath, "sessions.json");

				if (!fileManager.FileExists(sessionsFilePath))
				{
					return;
				}

				var data = File.ReadAllText(sessionsFilePath);
				sessions = (List<Session>)JsonConvert.DeserializeObject(data, typeof(List<Session>));

				var oldSession = sessions.SingleOrDefault(s => s.Id == entity.Id);
				if (oldSession != null)
				{
					sessions.Remove(oldSession);
				}

				if (entity.IsFavorite)
				{
					sessions.Add(entity);
				}

				foreach (var session in sessions)
				{
					session.IsFavorite = true; // this field is ignored by the serializer so we set it again here
				}

			}).ConfigureAwait(false);

			if (sessions != null)
			{
				await UpdateMultipleEntities(sessions).ConfigureAwait(false);
			}
		}
	}
}
