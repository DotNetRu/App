using System;
using System.Collections.Generic;
using System.Linq;
using DotNetRu.Clients.Portable.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Handlers
{
    public class NewsSearchHandler : SearchHandler
    {
        private IEnumerable<Tweet> _tweets;

        private bool _isFirstSearch = true;

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (ItemsSource is IEnumerable<Tweet> tweets)
            {
                if (_isFirstSearch)
                {
                    this._tweets = tweets;
                    this._isFirstSearch = false;
                }

                if (string.IsNullOrWhiteSpace(newValue))
                {
                    ItemsSource = this._tweets;
                }
                else
                {
                    ItemsSource = this._tweets.Where(x =>
                            x.Text.Contains(newValue, StringComparison.InvariantCultureIgnoreCase) ||
                            x.Name.Contains(newValue, StringComparison.InvariantCultureIgnoreCase))
                        .OrderBy(x => x.CreatedDate)
                        .ToList();
                }
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
