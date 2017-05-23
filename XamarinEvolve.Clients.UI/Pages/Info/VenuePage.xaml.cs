using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.UI
{
	public partial class VenuePage : BasePage
	{
		public override AppPage PageType => AppPage.ConferenceInfo;

		VenueViewModel vm;
		public VenuePage()
		{
			InitializeComponent();
			BindingContext = vm = new VenueViewModel();

			if (Device.OS == TargetPlatform.Android)
			{
				ToolbarItems.Add(new ToolbarItem
				{
					Order = ToolbarItemOrder.Secondary,
					Text = "Get Directions",
					Command = vm.NavigateCommand

				});

				if (vm.CanMakePhoneCall)
				{

					ToolbarItems.Add(new ToolbarItem
					{
						Order = ToolbarItemOrder.Secondary,
						Text = $"Call {EventInfo.VenueName}",
						Command = vm.CallCommand
					});
				}
			}
			else if (Device.OS == TargetPlatform.iOS)
			{
				ToolbarItems.Add(new ToolbarItem
				{
					Text = "More",
					Icon = "toolbar_overflow.png",
					Command = new Command(async () =>
						{
							string[] items = null;
							if (!vm.CanMakePhoneCall)
							{
								items = new[] { "Get Directions" };
							}
							else
							{
								items = new[] { "Get Directions", $"Call {EventInfo.VenuePhoneNumber}" };
							}
							var action = await DisplayActionSheet($"{EventInfo.VenueName}", "Cancel", null, items);
							if (action == items[0])
								vm.NavigateCommand.Execute(null);
							else if (items.Length > 1 && action == items[1] && vm.CanMakePhoneCall)
								vm.CallCommand.Execute(null);

						})
				});
			}
			else
			{
				ToolbarItems.Add(new ToolbarItem
				{
					Text = "Directions",
					Command = vm.NavigateCommand,
					Icon = "toolbar_navigate.png"
				});

				if (vm.CanMakePhoneCall)
				{

					ToolbarItems.Add(new ToolbarItem
					{
						Text = "Call",
						Command = vm.CallCommand,
						Icon = "toolbar_call.png"
					});
				}
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (EventMap.Pins.Count > 0)
				return;

			var position = new Position(vm.Latitude, vm.Longitude);
			EventMap.MoveToRegion(new MapSpan(position, 0.02, 0.02));
			EventMap.Pins.Add(new Pin
			{
				Type = PinType.Place,
				Address = vm.LocationTitle,
				Label = vm.EventTitle,
				Position = position
			});
		}


	}
}

