using System;

using Android.Widget;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using XamarinEvolve.Droid;

[assembly: ResolutionGroupName ("Xpirit")]
[assembly: ExportEffect (typeof (ListViewSelectionOnTopEffect), "ListViewSelectionOnTopEffect")]
namespace XamarinEvolve.Droid
{
    public class ListViewSelectionOnTopEffect : PlatformEffect
    {
        protected override void OnAttached ()
        {
            try 
            {
                var listView = this.Control as AbsListView;

                if (listView == null)
                    return;

                listView.SetDrawSelectorOnTop (true);
            } catch (Exception ex) {
                
            }
        }

        protected override void OnDetached ()
        {
            
        }
    }
}

