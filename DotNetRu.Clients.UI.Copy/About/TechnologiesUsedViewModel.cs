namespace DotNetRu.Clients.Portable.ViewModel
{
    using DotNetRu.Clients.Portable.Model;

    using MvvmHelpers;

    public class TechnologiesUsedViewModel : ViewModelBase
    {
        public ObservableRangeCollection<MenuItem> TechnologyItems { get; } = new ObservableRangeCollection<MenuItem>();

        public TechnologiesUsedViewModel()
        {
            this.TechnologyItems.AddRange(
                new[]
                {
                    new MenuItem
                        {
                            Name = "Visual Studio App Center",
                            Command = this.LaunchBrowserCommand,
                            Parameter = "https://appcenter.ms"
                        },
                    new MenuItem
                        {
                            Name = "Bottom Navigation Bar",
                            Command = this.LaunchBrowserCommand,
                            Parameter = "https://github.com/NAXAM/bottomtabbedpage-xamarin-forms"
                        },
                    new MenuItem
                    {
                        Name = "Connectivity Plugin",
                        Command = this.LaunchBrowserCommand,
                        Parameter =
                            "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Connectivity"
                    },
                    new MenuItem
                    {
                        Name = "Humanizer",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/Humanizr/Humanizer"
                    },
                    new MenuItem
                    {
                        Name = "Image Circles",
                        Command = this.LaunchBrowserCommand,
                        Parameter =
                            "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ImageCircle"
                    },
                    new MenuItem
                    {
                        Name = "LinqToTwitter",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/JoeMayo/LinqToTwitter"
                    },
                    new MenuItem
                    {
                        Name = "Mvvm Helpers",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/jamesmontemagno/mvvm-helpers"
                    },
                    new MenuItem
                    {
                        Name = "Noda Time",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/nodatime/nodatime"
                    },
                    new MenuItem
                    {
                        Name = "Permissions Plugin",
                        Command = this.LaunchBrowserCommand,
                        Parameter =
                            "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Permissions"
                    },
                    new MenuItem
                    {
                        Name = "Settings Plugin",
                        Command = this.LaunchBrowserCommand,
                        Parameter =
                            "https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Settings"
                    },
                    new MenuItem
                    {
                        Name = "Toolkit for Xamarin.Forms",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/jamesmontemagno/xamarin.forms-toolkit"
                    },
                    new MenuItem
                    {
                        Name = "Xamarin.Forms",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "http://xamarin.com/forms"
                    },
                });
        }
    }
}