using System.Windows.Input;
using MvvmHelpers;

namespace XamarinEvolve.Clients.Portable
{
    public class MenuItem : ObservableObject
    {
        string name;
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        string subtitle;
        public string Subtitle
        {
            get => this.subtitle;
            set => this.SetProperty(ref this.subtitle, value);
        }

        public string Icon {get;set;}
        public string Parameter {get;set;}

        public AppPage Page { get; set; }
        public ICommand Command {get;set;}
    }
}

