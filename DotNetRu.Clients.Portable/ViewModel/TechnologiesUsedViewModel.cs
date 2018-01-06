using DotNetRu.Clients.Portable.Model;
using MvvmHelpers;

namespace DotNetRu.Clients.Portable.ViewModel
{
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
                        Name = "Censored",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/jamesmontemagno/Censored"
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
                        Name = "Messaging Plugin",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "https://github.com/cjlotz/Xamarin.Plugins"
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
                        Name = "Pull to Refresh Layout",
                        Command = this.LaunchBrowserCommand,
                        Parameter =
                            "https://github.com/jamesmontemagno/Xamarin.Forms-PullToRefreshLayout"
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
                    new MenuItem
                    {
                        Name = "Xamarin Insights",
                        Command = this.LaunchBrowserCommand,
                        Parameter = "http://xamarin.com/insights"
                    }
                });
        }
    }
}