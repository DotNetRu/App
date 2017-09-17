using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
    using DotNetRu.DataStore.Audit.DataObjects;

    public partial class SpeakerDetailsPage : BasePage
	{
		public override AppPage PageType => AppPage.Speaker;
		private IPlatformSpecificExtension<Speaker> _extension;

		SpeakerDetailsViewModel ViewModel => vm ?? (vm = BindingContext as SpeakerDetailsViewModel);
		SpeakerDetailsViewModel vm;
		string sessionId;

		public SpeakerDetailsPage(Speaker speaker) : this((string)null)
		{
			Speaker = speaker;
		}

		public SpeakerDetailsPage(string sessionId)
		{
			this.sessionId = sessionId;
			InitializeComponent();
			MainScroll.ParallaxView = HeaderView;
			_extension = DependencyService.Get<IPlatformSpecificExtension<Speaker>>();

			ListViewSessions.ItemSelected += async (sender, e) =>
				{
					var session = ListViewSessions.SelectedItem as Session;
					if (session == null)
						return;

					var sessionDetails = new SessionDetailsPage(session);

					await NavigationService.PushAsync(Navigation, sessionDetails);

					ListViewSessions.SelectedItem = null;
				};

			if (Device.Idiom != TargetIdiom.Phone)
				Row1Header.Height = Row1Content.Height = 350;
		}

		public Speaker Speaker
		{
			get { return ViewModel.Speaker; }
			set
			{
				BindingContext = new SpeakerDetailsViewModel(value, sessionId);
				ItemId = value?.FullName;
			}
		}

		void MainScroll_Scrolled(object sender, ScrolledEventArgs e)
		{
			if (e.ScrollY > (MainStack.Height - SpeakerTitle.Height))
				Title = Speaker.FirstName;
			else
				Title = "Speaker Info";
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			//MainStack.HeightRequest = HeaderView.Height;
			MainScroll.Parallax();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			vm = null;

			var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.FollowItems.Count + 2;
			ListViewFollow.HeightRequest = (ViewModel.FollowItems.Count * ListViewFollow.RowHeight) - adjust;
			ListViewSessions.HeightRequest = 0;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			MainScroll.Scrolled += MainScroll_Scrolled;
			ListViewFollow.ItemTapped += ListViewTapped;
			ListViewSessions.ItemTapped += ListViewTapped;

			MainScroll.Parallax();

			if (ViewModel.Sessions?.Count > 0)
				return;

			await ViewModel.ExecuteLoadSessionsCommandAsync();
			var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.Sessions.Count + 1;
			ListViewSessions.HeightRequest = (ViewModel.Sessions.Count * ListViewSessions.RowHeight) - adjust;

			if (_extension != null)
			{
				await _extension.Execute(ViewModel.Speaker);
			}
		}

		void ListViewTapped(object sender, ItemTappedEventArgs e)
		{
			var list = sender as ListView;
			if (list == null)
				return;
			list.SelectedItem = null;
		}

		protected override async void OnDisappearing()
		{
			base.OnDisappearing();
			ListViewFollow.ItemTapped -= ListViewTapped;
			ListViewSessions.ItemTapped -= ListViewTapped;
			MainScroll.Scrolled -= MainScroll_Scrolled;

			if (_extension != null)
			{
				await _extension.Finish();
			}
		}
	}
}

