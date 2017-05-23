using System;

using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using Plugin.ExternalMaps;
using Plugin.Messaging;
using FormsToolkit;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
    public class VenueViewModel : ViewModelBase
    {
        public bool CanMakePhoneCall => CrossMessaging.Current.PhoneDialer.CanMakePhoneCall;
        public string EventTitle => EventInfo.EventName;
        public string LocationTitle => EventInfo.VenueName;
        public string Address1 => EventInfo.Address1;
        public string Address2 => EventInfo.Address2;
        public double Latitude => EventInfo.Latitude;
        public double Longitude => EventInfo.Longitude;

        ICommand  navigateCommand;
        public ICommand NavigateCommand =>
            navigateCommand ?? (navigateCommand = new Command(async () => await ExecuteNavigateCommandAsync())); 

        async Task ExecuteNavigateCommandAsync()
        {
            Logger.Track(EvolveLoggerKeys.NavigateToEvolve);
            if(!await CrossExternalMaps.Current.NavigateTo(LocationTitle, Latitude, Longitude))
            {
                MessagingService.Current.SendMessage(MessageKeys.Message, new MessagingServiceAlert
                    {
                        Title = "Unable to Navigate",
                        Message = "Please ensure that you have a map application installed.",
                        Cancel = "OK"
                    });
            }
        }

        ICommand  callCommand;
        public ICommand CallCommand =>
            callCommand ?? (callCommand = new Command(ExecuteCallCommand)); 

        void ExecuteCallCommand()
        {
            Logger.Track(EvolveLoggerKeys.CallVenue);
            var phoneCallTask = CrossMessaging.Current.PhoneDialer;
            if (phoneCallTask.CanMakePhoneCall) 
                phoneCallTask.MakePhoneCall(EventInfo.VenuePhoneNumber);
        }
    }
}


