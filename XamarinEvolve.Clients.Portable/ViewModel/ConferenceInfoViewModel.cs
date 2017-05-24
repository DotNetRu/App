using System;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using FormsToolkit;
using Plugin.Share;
using System.Net.Http;
using Plugin.Connectivity;
using Newtonsoft.Json;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
    public class WiFiRoot
    {
        public string SSID { get; set; }
        public string Password { get; set; }
    }

    public class ConferenceInfoViewModel : ViewModelBase
    {
		public string CodeOfConductPageTitle => AboutThisApp.CodeOfConductPageTitle;

        public async Task<bool> UpdateConfigs()
        {
			if (IsBusy)
                return false;


            try 
            {
                IsBusy = true;
                try 
                {
                    if (CrossConnectivity.Current.IsConnected) 
                    {
                        using (var client = new HttpClient ()) 
                        {
                            client.Timeout = TimeSpan.FromSeconds (5);
                        }
                    }
                } 
                catch 
                {
                }
            } 
            finally 
            {
                IsBusy = false;
            }

            return true;
        }
    }
}

