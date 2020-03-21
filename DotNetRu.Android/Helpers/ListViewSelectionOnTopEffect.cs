using System;
using Android.Widget;
using DotNetRu.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Xpirit")]
[assembly: ExportEffect(typeof(ListViewSelectionOnTopEffect), "ListViewSelectionOnTopEffect")]

namespace DotNetRu.Droid.Helpers
{
    public class ListViewSelectionOnTopEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (this.Control is AbsListView listView)
                {
                    listView.SetDrawSelectorOnTop(true);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
