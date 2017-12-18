using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI.Controls
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
