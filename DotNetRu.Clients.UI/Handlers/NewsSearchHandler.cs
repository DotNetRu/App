using System;
using System.Collections.Generic;
using System.Linq;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Models.Social;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Handlers
{
    public class NewsSearchHandler : SearchHandler
    {
        private IEnumerable<ISocialPost> _socialPosts;

        private bool _isFirstSearch = true;

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (ItemsSource is IEnumerable<ISocialPost> socialPosts)
            {
                if (_isFirstSearch)
                {
                    this._socialPosts = socialPosts;
                    this._isFirstSearch = false;
                }

                if (string.IsNullOrWhiteSpace(newValue))
                {
                    ItemsSource = this._socialPosts;
                }
                else
                {
                    ItemsSource = this._socialPosts.Where(x =>
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
            if (item is ISocialPost socialPost && !string.IsNullOrWhiteSpace(socialPost.Url))
            {
                App.Logger.TrackPage(AppPage.SocialPost.ToString(), socialPost.Url);

                await Launcher.OpenAsync(new Uri(socialPost.Url));
            }
        }
    }
}
