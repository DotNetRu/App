using System;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Controls
{
    public class NavigationView : ContentView
    {
        public void OnNavigationItemSelected(NavigationItemSelectedEventArgs e)
        {
            this.NavigationItemSelected?.Invoke(this, e);
        }

        public event NavigationItemSelectedEventHandler NavigationItemSelected;

    }

    /// <summary>
    /// Arguments to pass to event handlers
    /// </summary>
    public class NavigationItemSelectedEventArgs : EventArgs
    {
        public int Index { get; set; }
    }

    public delegate void NavigationItemSelectedEventHandler(object sender, NavigationItemSelectedEventArgs e);

}

