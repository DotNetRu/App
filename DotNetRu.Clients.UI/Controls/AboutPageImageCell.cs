using System.Windows.Input;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Controls
{
    class AboutPageImageCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<AboutPageImageCell, ICommand>(x => x.Command, null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}
