using System;
using System.Linq;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Pages.Home;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Handlers
{
    public class NewsSearchHandler : SearchHandler
    {
        private readonly NewsViewModel _viewModel;

        private readonly NewsPage _page;

        public NewsSearchHandler(NewsViewModel viewModel, NewsPage page)
        {
            _viewModel = viewModel;
            _page = page;
        }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = this._viewModel.Tweets.Where(x =>
                        x.Text.Contains(newValue, StringComparison.InvariantCultureIgnoreCase) ||
                        x.Name.Contains(newValue, StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(x => x.CreatedDate)
                    .ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (item is Tweet tweet && !string.IsNullOrWhiteSpace(tweet.Url))
            {
                App.Logger.TrackPage(AppPage.Tweet.ToString(), tweet.Url);

                await Launcher.OpenAsync(new Uri(tweet.Url));
            }
        }
    }
}
