using System;
using System.Collections.Generic;
using System.Linq;
using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.Portable.ViewModel;
using DotNetRu.Clients.UI.Speakers;
using DotNetRu.DataStore.Audit.Models;
using MvvmHelpers;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Handlers
{
    public class SpeakersSearchHandler : SearchHandler
    {
        private IEnumerable<Grouping<char, SpeakerModel>> _speakerGroups;

        private bool _isFirstSearch = true;

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (ItemsSource is IEnumerable<Grouping<char, SpeakerModel>> speakerGroups)
            {
                if (_isFirstSearch)
                {
                    this._speakerGroups = speakerGroups;
                    this._isFirstSearch = false;
                }

                if (string.IsNullOrWhiteSpace(newValue))
                {
                    ItemsSource = this._speakerGroups;
                }
                else
                {
                    ItemsSource = this._speakerGroups.SelectMany(speaker => speaker.Items).Where(x =>
                        x.FullName.Contains(newValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (item is SpeakerModel speaker)
            {
                App.Logger.TrackPage(AppPage.Speaker.ToString(), speaker.FullName);

                if (BindingContext is ViewModelBase viewModel)
                {
                    await viewModel.Navigation.PushAsync(new SpeakerDetailsPage { SpeakerModel = speaker });
                }
            }
        }
    }
}
