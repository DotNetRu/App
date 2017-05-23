﻿using Xamarin.Forms.Platform.Android;
using Android.Support.Design.Widget;
using Android.Runtime;
using Xamarin.Forms;
using XamarinEvolve.Droid;
using XamarinEvolve.Clients.Portable;
using Android.Widget;
using FormsToolkit;
using Android.Views;
using XamarinEvolve.Utils;


[assembly: ExportRenderer(typeof(XamarinEvolve.Clients.UI.NavigationView), typeof(NavigationViewRenderer))]
namespace XamarinEvolve.Droid
{
	public class NavigationViewRenderer : ViewRenderer<XamarinEvolve.Clients.UI.NavigationView, NavigationView>
    {
        NavigationView navView;
        ImageView profileImage;
        TextView profileName;

		public override void OnViewAdded(Android.Views.View child)
		{
			base.OnViewAdded(child);

			navView.Menu.FindItem(Resource.Id.nav_feed).SetTitle($"{EventInfo.EventName}");
			navView.Menu.FindItem(Resource.Id.nav_speakers).SetVisible(FeatureFlags.SpeakersEnabled);
			navView.Menu.FindItem(Resource.Id.nav_events).SetVisible(FeatureFlags.EventsEnabled);
			navView.Menu.FindItem(Resource.Id.nav_mini_hacks).SetVisible(FeatureFlags.MiniHacksEnabled);
			navView.Menu.FindItem(Resource.Id.nav_evals).SetVisible(FeatureFlags.EvalEnabled);
			navView.Menu.FindItem(Resource.Id.nav_floor_map).SetVisible(FeatureFlags.FloormapEnabled);
			navView.Menu.FindItem(Resource.Id.nav_conference_info).SetVisible(FeatureFlags.ConferenceInformationEnabled || FeatureFlags.CodeOfConductEnabled);
		}

        protected override void OnElementChanged(ElementChangedEventArgs<XamarinEvolve.Clients.UI.NavigationView> e)
        {
            
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;


            var view = Inflate(Forms.Context, Resource.Layout.nav_view, null);
            navView = view.JavaCast<NavigationView>();


            navView.NavigationItemSelected += NavView_NavigationItemSelected;

            Settings.Current.PropertyChanged += SettingsPropertyChanged;
            SetNativeControl(navView);

            var header = navView.GetHeaderView(0);
            profileImage = header.FindViewById<ImageView>(Resource.Id.profile_image);
            profileName = header.FindViewById<TextView>(Resource.Id.profile_name);

			if (FeatureFlags.LoginEnabled)
			{
				profileImage.Click += (sender, e2) => NavigateToLogin();
				profileName.Click += (sender, e2) => NavigateToLogin();

				UpdateName();
				UpdateImage();
			}
			else
			{
				profileImage.Visibility = ViewStates.Gone;
				profileName.Visibility = ViewStates.Gone;
			}

            navView.SetCheckedItem(Resource.Id.nav_feed);
        }

        void NavigateToLogin()
        {
            if (Settings.Current.IsLoggedIn)
                return;

            XamarinEvolve.Clients.UI.App.Logger.TrackPage(AppPage.Login.ToString(), "navigation");
            MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
        }

        void SettingsPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.Current.Email))
            {
                UpdateName();
                UpdateImage();
            }
        }

        void UpdateName()
        {
            profileName.Text = Settings.Current.UserDisplayName;
        }

        void UpdateImage()
        {
            Koush.UrlImageViewHelper.SetUrlDrawable (profileImage, Settings.Current.UserAvatar, Resource.Drawable.profile_generic);
        }

        public override void OnViewRemoved(Android.Views.View child)
        {
            base.OnViewRemoved(child);
            navView.NavigationItemSelected -= NavView_NavigationItemSelected;
            Settings.Current.PropertyChanged -= SettingsPropertyChanged;
        }

        IMenuItem previousItem;

        void NavView_NavigationItemSelected (object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {


            if (previousItem != null )
                previousItem.SetChecked(false);

            navView.SetCheckedItem(e.MenuItem.ItemId);

            previousItem = e.MenuItem;

            int id = 0;
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_feed:
                    id = (int)AppPage.Feed;
                    break;
                case Resource.Id.nav_sessions:
                    id = (int)AppPage.Sessions;
                    break;
				case Resource.Id.nav_speakers:
					id = (int)AppPage.Speakers;
					break;
				case Resource.Id.nav_events:
                    id = (int)AppPage.Events;
                    break;
                case Resource.Id.nav_sponsors:
                    id = (int)AppPage.Sponsors;
                    break;
                case Resource.Id.nav_venue:
                    id = (int)AppPage.Venue;
                    break;
                case Resource.Id.nav_floor_map:
                    id = (int)AppPage.FloorMap;
                    break;
                case Resource.Id.nav_conference_info:
                    id = (int)AppPage.ConferenceInfo;
                    break;
                case Resource.Id.nav_mini_hacks:
                    id = (int)AppPage.MiniHacks;
                    break;
                case Resource.Id.nav_settings:
                    id = (int)AppPage.Settings;
                    break;
                case Resource.Id.nav_evals:
                    id = (int)AppPage.Evals;
                    break;
            }
            this.Element.OnNavigationItemSelected(new XamarinEvolve.Clients.UI.NavigationItemSelectedEventArgs
                {
                   
                    Index = id
                });
        }

      
    }
}

