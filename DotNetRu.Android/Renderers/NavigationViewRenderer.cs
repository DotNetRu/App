using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using DotNetRu.Droid;
using DotNetRu.Droid.Helpers;
using Java.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Droid;

[assembly: ExportRenderer(typeof(XamarinEvolve.Clients.UI.NavigationView), typeof(NavigationViewRenderer))]

namespace DotNetRu.Droid
{
    using System.ComponentModel;
    using View = Android.Views.View;

    public class NavigationViewRenderer : ViewRenderer<XamarinEvolve.Clients.UI.NavigationView, NavigationView>
    {
        NavigationView navView;

        public override void OnViewAdded(View child)
        {
            base.OnViewAdded(child);

            this.navView.Menu.FindItem(Resource.Id.nav_events).SetVisible(true);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XamarinEvolve.Clients.UI.NavigationView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
            {
                return;
            }

            var view = Inflate(Forms.Context, Resource.Layout.nav_view, null);
            this.navView = view.JavaCast<NavigationView>();

            this.navView.NavigationItemSelected += this.NavView_NavigationItemSelected;

            Settings.Current.PropertyChanged += this.SettingsPropertyChanged;
            this.SetNativeControl(this.navView);

            this.navView.SetCheckedItem(Resource.Id.nav_feed);
        }

        void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public override void OnViewRemoved(View child)
        {
            base.OnViewRemoved(child);
            this.navView.NavigationItemSelected -= this.NavView_NavigationItemSelected;
            Settings.Current.PropertyChanged -= this.SettingsPropertyChanged;
        }

        IMenuItem previousItem;

        void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            if (this.previousItem != null) this.previousItem.SetChecked(false);

            this.navView.SetCheckedItem(e.MenuItem.ItemId);

            this.previousItem = e.MenuItem;

            int id = 0;
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_feed:
                    id = (int) AppPage.Feed;
                    break;
                case Resource.Id.nav_speakers:
                    id = (int) AppPage.Speakers;
                    break;
                case Resource.Id.nav_events:
                    id = (int) AppPage.Meetups;
                    break;
                case Resource.Id.nav_sponsors:
                    id = (int) AppPage.Friends;
                    break;
                case Resource.Id.nav_settings:
                    id = (int) AppPage.Settings;
                    break;
            }
            this.Element.OnNavigationItemSelected(
                new XamarinEvolve.Clients.UI.NavigationItemSelectedEventArgs {Index = id});
        }
    }
}