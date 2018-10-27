using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.DataObjects;

    public partial class SpeakersPage : BasePage
	{
		public override AppPage PageType => AppPage.Speakers;

		SpeakersViewModel vm;
		SpeakersViewModel ViewModel => vm ?? (vm = BindingContext as SpeakersViewModel);

		public SpeakersPage()
		{
			InitializeComponent();
			BindingContext = new SpeakersViewModel(Navigation);

			if (Device.OS == TargetPlatform.Android)
				ListViewSpeakers.Effects.Add(Effect.Resolve("Xpirit.ListViewSelectionOnTopEffect"));

			if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
			{
				ToolbarItems.Add(new ToolbarItem
				{
					Text = "Refresh",
					Icon = "toolbar_refresh.png",
					Command = ViewModel.ForceRefreshCommand
				});
			}
			ListViewSpeakers.ItemSelected += async (sender, e) =>
			{
				var speaker = ListViewSpeakers.SelectedItem as Speaker;
				if (speaker == null)
					return;

                ContentPage destination;

                if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                {
					destination = new SpeakerDetailsPageUWP(speaker);
                }
                else
                {
					destination = new SpeakerDetailsPage(speaker);
                }

				await NavigationService.PushAsync(Navigation, destination);
				ListViewSpeakers.SelectedItem = null;
			};
		}

		void ListViewTapped(object sender, ItemTappedEventArgs e)
		{
			var list = sender as ListView;
			if (list == null)
				return;
			list.SelectedItem = null;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ListViewSpeakers.ItemTapped += ListViewTapped;
			if (ViewModel.Speakers?.Count == 0)
				ViewModel.LoadSpeakersCommand.Execute(true);

		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ListViewSpeakers.ItemTapped -= ListViewTapped;
		}
	}
}
