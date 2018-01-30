using DotNetRu.Clients.Portable.ViewModel;

namespace DotNetRu.Clients.UI
{
    public static class ViewModelLocator
    {
        private static MeetupViewModel meetupViewModel;

        public static MeetupViewModel MeetupViewModel =>
            meetupViewModel ?? (meetupViewModel = new MeetupViewModel(navigation: null));
    }
}