using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.Design.Internal;
using BottomNavigationItemView = Android.Support.Design.Internal.BottomNavigationItemView;
using BottomNavigationMenuView = Android.Support.Design.Internal.BottomNavigationMenuView;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;
using Color = Xamarin.Forms.Color;

namespace DotNetRu.Droid.Renderers
{
    public class SelectedShellItemRenderer : ShellItemRenderer, BottomNavigationView.IOnNavigationItemSelectedListener, IAppearanceObserver
    {
        #region IOnNavigationItemSelectedListener

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            return OnItemSelected(item);
        }

        #endregion IOnNavigationItemSelectedListener

        #region IAppearanceObserver

        public void OnAppearanceChanged(ShellAppearance appearance)
        {
            if (appearance != null)
                SetAppearance(appearance);
            else
                ResetAppearance();
        }

        #endregion IAppearanceObserver

        #region Fields

        BottomNavigationView _bottomView;
        FrameLayout _navigationArea;
        AView _outerLayout;
        IShellBottomNavViewAppearanceTracker _appearanceTracker;
        bool _disposed;

        #endregion

        public SelectedShellItemRenderer(IShellContext context) : base(context) { }

        public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            _outerLayout = inflater?.Inflate(Resource.Layout.BottomTabLayout, null);
            _bottomView = _outerLayout?.FindViewById<BottomNavigationView>(Resource.Id.bottomtab_tabbar);
            _navigationArea = _outerLayout?.FindViewById<FrameLayout>(Resource.Id.bottomtab_navarea);

            _bottomView?.SetBackgroundColor(Color.White.ToAndroid());
            _bottomView?.SetOnNavigationItemSelectedListener(this);

            if (ShellItem == null)
                throw new InvalidOperationException("Active Shell Item not set. Have you added any Shell Items to your Shell?");

            if (ShellItem.CurrentItem == null)
                throw new InvalidOperationException($"Content not found for active {ShellItem}. Title: {ShellItem.Title}. Route: {ShellItem.Route}.");

            HookEvents(ShellItem);
            SetupMenu();

            _appearanceTracker = ShellContext.CreateBottomNavViewAppearanceTracker(ShellItem);
            ((IShellController)ShellContext.Shell).AddAppearanceObserver(this, ShellItem);

            return _outerLayout;
        }

        #region Disposing

        private void Destroy()
        {
            if (ShellItem != null)
                UnhookEvents(ShellItem);

            ((IShellController)ShellContext?.Shell)?.RemoveAppearanceObserver(this);

            _navigationArea?.Dispose();
            _appearanceTracker?.Dispose();
            _outerLayout?.Dispose();

            if (_bottomView != null)
            {
                _bottomView?.SetOnNavigationItemSelectedListener(null);
                _bottomView?.Background?.Dispose();
                _bottomView?.Dispose();
            }

            _bottomView = null;
            _navigationArea = null;
            _appearanceTracker = null;
            _outerLayout = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;
            if (disposing)
                Destroy();

            base.Dispose(disposing);
        }

        // Use OnDestory become OnDestroyView may fire before events are completed.
        public override void OnDestroy()
        {
            Destroy();
            base.OnDestroy();
        }

        #endregion

        private void SetupMenu()
        {
            using var menu = _bottomView.Menu;
            var currentIndex = ((IElementController)ShellItem).LogicalChildren.IndexOf(ShellSection);

            var items = ShellItem.Items.Select(item => (item.Title, item.Icon, item.IsEnabled)).ToList();

            var setupMenuMethod = typeof(BottomNavigationViewUtils).GetMethod("SetupMenu", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            setupMenuMethod?.Invoke(null,
                new object[] {menu, _bottomView.MaxItemCount, items, currentIndex, _bottomView, Context});

            SetupTabTitleTextSize(ShellItem, _bottomView.MaxItemCount, items);

            UpdateTabBarVisibility();
        }

        private void SetupTabTitleTextSize(ShellItem shellItem, int maxBottomItems, List<(string title, ImageSource icon, bool tabEnabled)> items)
        {
            var numberOfMenuItems = items.Count;
            var showMore = numberOfMenuItems > maxBottomItems;
            var end = showMore ? maxBottomItems - 1 : numberOfMenuItems;

            for (var i = 0; i < end; i++)
            {
                var shellSection = shellItem.Items[i];
                ApplyTabTitleTextSize(shellSection, shellSection.Title, shellSection.IsChecked, i);
            }

            if (showMore)
            {
                ApplyTabTitleTextSize(ShellItem, ShellItem.Title,
                    ShellItem.Items.Skip(maxBottomItems - 1).Any(x => x.IsChecked), MoreTabId);
            }
        }

        private void ApplyTabTitleTextSize(BaseShellItem baseShellItem, string badgeText, bool isSelected, int itemId)
        {
            using var bottomNavigationMenuView = _bottomView.GetChildAt(0) as BottomNavigationMenuView;
            var itemView = bottomNavigationMenuView?.FindViewById<BottomNavigationItemView>(itemId);

            var itemTitle = itemView?.GetChildAt(1);

            var smallTextView = (itemTitle as BaselineLayout)?.GetChildAt(0) as TextView;
            var largeTextView = (itemTitle as BaselineLayout)?.GetChildAt(1) as TextView;

            smallTextView?.SetTextSize(ComplexUnitType.Sp, 8);
            largeTextView?.SetTextSize(ComplexUnitType.Sp, 10);
        }

        private void UpdateTabBarVisibility()
        {
            if (DisplayedPage == null)
                return;

            var visible = Shell.GetTabBarIsVisible(DisplayedPage);
            using (var menu = _bottomView.Menu)
            {
                if (menu.Size() == 1)
                    visible = false;
            }

            _bottomView.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}
