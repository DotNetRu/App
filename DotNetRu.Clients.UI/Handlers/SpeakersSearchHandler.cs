using System;
using System.Linq;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Pages.Speakers;
using DotNetRu.Clients.UI.Speakers;
using DotNetRu.DataStore.Audit.Models;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Handlers
{
    public class SpeakersSearchHandler : SearchHandler
    {
        private readonly SpeakersViewModel _viewModel;

        private readonly SpeakersPage _page;

        public SpeakersSearchHandler(SpeakersViewModel viewModel, SpeakersPage page)
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
                ItemsSource = this._viewModel.Speakers.SelectMany(speaker => speaker.Items).Where(x =>
                    x.FullName.Contains(newValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (item is SpeakerModel speaker)
            {
                App.Logger.TrackPage(AppPage.Speaker.ToString(), speaker.FullName);

                await this._page.Navigation.PushAsync(new SpeakerDetailsPage { SpeakerModel = speaker });
            }
        }
    }
}
