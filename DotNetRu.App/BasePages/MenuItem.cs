using System.Windows.Input;

using MvvmHelpers;

namespace DotNetRu.Clients.Portable.Model
{
    public class MenuItem : ObservableObject
    {
        private string name;

        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        public string Icon {get; set; }

        public ImageSource ImageSource { get; set; }

        public string Parameter {get; set; }

        public AppPage Page { get; set; }

        public ICommand Command {get; set; }
    }
}

