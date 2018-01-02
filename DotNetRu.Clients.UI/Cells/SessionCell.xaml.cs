using DotNetRu.Clients.Portable.Model;
using DotNetRu.Clients.UI.Helpers;
using DotNetRu.Clients.UI.Pages.Sessions;
using DotNetRu.DataStore.Audit.Models;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Cells
{
    public class SessionCell : ViewCell
    {
        private readonly INavigation navigation;

        public SessionCell(INavigation navigation = null)
        {
            this.Height = 120;
            this.View = new SessionCellView();
            this.navigation = navigation;
        }

        protected override async void OnTapped()
        {
            base.OnTapped();
            if (this.navigation == null)
            {
                return;
            }

            if (!(this.BindingContext is TalkModel session))
            {
                return;
            }

            App.Logger.TrackPage(AppPage.Talk.ToString(), session.Title);
            await NavigationService.PushAsync(this.navigation, new TalkPage(session));
        }
    }

    public partial class SessionCellView
    {
        public SessionCellView()
        {
            this.InitializeComponent();
        }

        // private void GenerateCategoryBadges()
        // {
        // CategoriesPlaceholder.Children.Clear();

        // var session = BindingContext as Session;

        // if (session != null)
        // {
        // foreach (var category in session.Categories.Take(4))
        // {
        // var grid = new Grid
        // {
        // Padding = new Thickness(0, 4),
        // HeightRequest = 28,
        // MinimumWidthRequest = 200,
        // VerticalOptions = LayoutOptions.Center,
        // HorizontalOptions = LayoutOptions.Start
        // };

        // if (Device.RuntimePlatform == Device.iOS)
        // {
        // var image = new CircleImage
        // {
        // FillColor = Color.FromHex(category.Color),
        // VerticalOptions = LayoutOptions.Center,
        // HorizontalOptions = LayoutOptions.FillAndExpand,
        // HeightRequest = 24
        // };

        // grid.Children.Add(image);
        // }
        // else
        // {
        // var box = new BoxView
        // {
        // BackgroundColor = Color.FromHex(category.Color),
        // VerticalOptions = LayoutOptions.Center,
        // HorizontalOptions = LayoutOptions.FillAndExpand,
        // HeightRequest = 24,
        // };

        // grid.Children.Add(box);
        // }

        // private var label = new Label
        // {
        // VerticalOptions = LayoutOptions.Center,
        // VerticalTextAlignment = TextAlignment.Center,
        // HorizontalOptions = LayoutOptions.Start,
        // HorizontalTextAlignment = TextAlignment.Center,
        // FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
        // TextColor = Color.White,
        // Text = category.BadgeName.ToUpperInvariant(),
        // Margin = new Thickness(5, 0),
        // WidthRequest = 60,
        // };

        // 	if (Device.RuntimePlatform == TargetPlatform.WinPhone || Device.RuntimePlatform == TargetPlatform.Windows)
        // 	{
        // 		label.FontSize = 10;
        // 	}

        // grid.Children.Add(label);

        // CategoriesPlaceholder.Children.Add(grid);
        // }
        // }
        // }
    }
}