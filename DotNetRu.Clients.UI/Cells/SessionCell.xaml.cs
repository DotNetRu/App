using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using XamarinEvolve.Clients.Portable;
using System.Windows.Input;
using ImageCircle.Forms.Plugin.Abstractions;
using System.Linq;

namespace XamarinEvolve.Clients.UI
{

    public class SessionCell: ViewCell
    {
        readonly INavigation navigation;
        public SessionCell (INavigation navigation = null)
        {
            Height = 120;
            View = new SessionCellView ();
            this.navigation = navigation;

        }

        protected override async void OnTapped()
        {
            base.OnTapped();
            if (navigation == null)
                return;
            var session = BindingContext as Session;
            if (session == null)
                return;
            
            App.Logger.TrackPage(AppPage.Session.ToString(), session.Title);
            await NavigationService.PushAsync(navigation, new SessionDetailsPage(session));
        }
    }

    public partial class SessionCellView : ContentView
    {
        public SessionCellView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            GenerateCategoryBadges();
        }

        private void GenerateCategoryBadges()
        {
            CategoriesPlaceholder.Children.Clear();

            var session = BindingContext as Session;

            if (session != null)
            {
				foreach (var category in session.Categories.Take(4))
                {
                    var grid = new Grid
                    {
                        Padding = new Thickness(0, 4),
                        HeightRequest = 28,
                        MinimumWidthRequest = 200,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Start
                    };

                    if (Device.OS == TargetPlatform.iOS)
                    {
                        var image = new CircleImage
                        {
                            FillColor = Color.FromHex(category.Color),
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HeightRequest = 24
                        };

                        grid.Children.Add(image);
                    }
                    else
                    {
                        var box = new BoxView
                        {
                            BackgroundColor = Color.FromHex(category.Color),
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HeightRequest = 24,
                        };

                        grid.Children.Add(box);
                    }

					var label = new Label
					{
						VerticalOptions = LayoutOptions.Center,
						VerticalTextAlignment = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.Start,
						HorizontalTextAlignment = TextAlignment.Center,
						FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                        TextColor = Color.White,
                        Text = category.BadgeName.ToUpperInvariant(),
                        Margin = new Thickness(5, 0),
						WidthRequest = 60,
                    };

					if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
					{
						label.FontSize = 10;
					}

                    grid.Children.Add(label);

                    CategoriesPlaceholder.Children.Add(grid);
                }
            }
        }

        public static readonly BindableProperty FavoriteCommandProperty = 
            BindableProperty.Create(nameof(FavoriteCommand), typeof(ICommand), typeof(SessionCellView), default(ICommand));

        public ICommand FavoriteCommand
        {
            get { return GetValue(FavoriteCommandProperty) as Command; }
            set { SetValue(FavoriteCommandProperty, value); }
        }
    }
}

