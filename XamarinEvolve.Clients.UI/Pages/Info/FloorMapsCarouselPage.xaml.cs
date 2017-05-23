using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
    public partial class FloorMapsCarouselPage : CarouselPage, IProvidePageInfo
	{
		public AppPage PageType => AppPage.FloorMap;

        public FloorMapsCarouselPage()
        {
            InitializeComponent();

            Children.Add(new FloorMapPage(0));
            Children.Add(new FloorMapPage(1));
            Children.Add(new FloorMapPage(2));

            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                Title = Children[0].Title;
                CurrentPageChanged += (sender, e) =>
                {
                    Title = CurrentPage.Title;
                };
            }
        }
    }
}
