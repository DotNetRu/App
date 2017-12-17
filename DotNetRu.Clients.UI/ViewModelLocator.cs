namespace XamarinEvolve.Clients.UI
{
    using XamarinEvolve.Clients.Portable;

    public static class ViewModelLocator
    {
        private static MeetupViewModel meetupViewModel;

        public static MeetupViewModel MeetupViewModel =>
            meetupViewModel ?? (meetupViewModel = new MeetupViewModel(navigation: null));
    }
}