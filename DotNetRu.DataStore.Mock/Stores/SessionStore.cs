namespace XamarinEvolve.DataStore.Mock.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetRu.DataStore.Audit.DataObjects;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.DataStore.Mock.Abstractions;

    public class SessionStore : BaseStore<TalkModel>, ISessionStore
    {

        List<TalkModel> _sessions;
        ISpeakerStore _speakerStore;
        ICategoryStore _categoryStore;
        IFeedbackStore _feedbackStore;
        public SessionStore()
        {
            this._speakerStore = DependencyService.Get<ISpeakerStore>();
            this._categoryStore = DependencyService.Get<ICategoryStore>();
            this._feedbackStore = DependencyService.Get<IFeedbackStore>();
        }

        #region ISessionStore implementation

        public async override Task<TalkModel> GetItemAsync(string id)
        {
            if (!this._initialized)
                await this.InitializeStore();
            
            return this._sessions.FirstOrDefault(s => s.Id == id);
        }

        public async override Task<IEnumerable<TalkModel>> GetItemsAsync(bool forceRefresh = false)
        {
            if (!this._initialized)
                await this.InitializeStore();
            
            return this._sessions as IEnumerable<TalkModel>;
        }

        public async Task<IEnumerable<TalkModel>> GetSpeakerSessionsAsync(string speakerId)
        {
            if (!this._initialized)
                await this.InitializeStore();
            
            var results =  from session in this._sessions
                           where session.StartTime.HasValue
                           orderby session.StartTime.Value
                           from speaker in session.Speakers
                           where speaker.Id == speakerId
                           select session;
            
            return results;
        }

        public async Task<IEnumerable<TalkModel>> GetNextSessions(int maxNumber)
        {
            if (!this._initialized)
                await this.InitializeStore();

            var date = DateTime.UtcNow.AddMinutes(-30);

            var results = (from session in this._sessions
                where session.StartTime.HasValue && session.StartTime.Value > date
                                    orderby session.StartTime.Value
                                    select session).Take(maxNumber);


            var enumerable = results as TalkModel[] ?? results.ToArray();
            return !enumerable.Any() ? null : enumerable;
        }

        #endregion

        #region IBaseStore implementation
        bool _initialized = false;
        public async override Task InitializeStore()
        {
            if (this._initialized)
                return;
            
            this._initialized = true;
            var categories = (await this._categoryStore.GetItemsAsync()).ToArray();
            await this._speakerStore.InitializeStore();
            var speakers = (await this._speakerStore.GetItemsAsync().ConfigureAwait(false)).ToArray();
            this._sessions = new List<TalkModel>();
            int speaker = 0;
            int speakerCount = 0;
            int room = 0;
            int categoryCount = 0;
            int category = 0;
            var day = new DateTime(2016, 4, 27, 13, 0, 0, DateTimeKind.Utc);
            int dayCount = 0;
            for (int i = 0; i < this._titles.Length; i++)
            {
                var sessionSpeakers = new List<Speaker>();
                var sessionCategories = new List<Category>();

                categoryCount++;
                speakerCount++;
                
                for (int j = 0; j < speakerCount; j++)
                {
                    sessionSpeakers.Add(speakers[speaker]);
                    speaker++;
                    if (speaker >= speakers.Length)
                        speaker = 0;
                }

                if (i == 1)
                    sessionSpeakers.Add(this._sessions[0].Speakers.ElementAt(0));

                for (int j = 0; j < categoryCount; j++)
                {
                    sessionCategories.Add(categories[category]);
                    category++;
                    if (category >= categories.Length)
                        category = 0;
                }

                if (i == 1)
                    sessionCategories.Add(this._sessions[0].Categories.ElementAt(0));

                var ro = this._rooms[room];
                room++;
                if (room >= this._rooms.Length)
                    room = 0;

                this._sessions.Add(new TalkModel
                    {
                        Id = i.ToString(),
                        Abstract = "This is an abstract that is going to tell us all about how awsome this session is and that you should go over there right now and get ready for awesome!.",
                        Categories = sessionCategories,
                        Room = ro,
                        Speakers = sessionSpeakers,
                        Title = this._titles[i],
                        ShortTitle = this._titlesShort[i]
                    });
                
                this._sessions[i].FeedbackLeft = await this._feedbackStore.LeftFeedback(this._sessions[i]);

                this.SetStartEnd(this._sessions[i], day);

                if (i == this._titles.Length / 2)
                {
                    dayCount = 0;
                    day = new DateTime(2016, 4, 28, 13, 0, 0, DateTimeKind.Utc);
                }
                else
                {
                    dayCount++;
                    if (dayCount == 2)
                    {
                        day = day.AddHours(1);
                        dayCount = 0;
                    }
                }


                if (speakerCount > 2)
                    speakerCount = 0;
            }


            this._sessions.Add(new TalkModel
                                   {
                                       Id = this._sessions.Count.ToString(),
                                       Abstract = "Coming soon",
                                       Categories = categories.Take(1).ToList(),
                                       Room = this._rooms[0],

                                       // Speakers = new List<Speaker>{ speakers[0] },
                                       Title = "Something awesome!",
                                       ShortTitle = "Awesome",
                                   });
            this._sessions[this._sessions.Count - 1].FeedbackLeft = await this._feedbackStore.LeftFeedback(this._sessions[this._sessions.Count - 1]);
            this._sessions[this._sessions.Count - 1].StartTime = null;
            this._sessions[this._sessions.Count - 1].EndTime = null;
        }

        void SetStartEnd(TalkModel talkModel, DateTime day)
        {
            talkModel.StartTime = day;
            talkModel.EndTime = talkModel.StartTime.Value.AddHours(1);
        }

        public Task<TalkModel> GetAppIndexSession (string id)
        {
            return this.GetItemAsync (id);
        }

        Room[] _rooms = new [] 
        {
                new Room {Name = "Fossy Salon"},
                new Room {Name = "Crick Salon"},
                new Room {Name = "Franklin Salon"},
                new Room {Name = "Goodall Salon"},
                new Room {Name = "Linnaeus Salon"},
                new Room {Name = "Watson Salon"},
        };


        

        string[] _titles = new [] {
            "Create stunning apps with the Xamarin Designer for iOS",
            "Everyone can create beautiful apps with material design",
            "Dispelling design myths and making apps better",
            "3 Platforms: 1 codebase—your first Xamarin.Forms app",
            "Mastering XAML in Xamarin.Forms",
            "NuGet your code to all the platforms with portable class libraries",
            "A new world of possibilities for contextual awareness with iBeacons",
            "Wearables and IoT: Taking C# with you everywhere",
            "Create the next great mobile app in a weekend",
            "Best practices for effective iOS memory management",
            "Navigation design patterns for iOS and Android",
            "Is your app secure?",
            "Introduction to Xamarin.Insights",
            "Cross platform unit testing with xUnit",
            "Test automation in practice with Xamarin Test Cloud at MixRadio",
            "Why you should be building better mobile apps with reactive programming",
            "Create your own sci-fi with mobile augmented reality",
            "Addressing the OWASP mobile security threats using Xamarin"

        };

        string[] _titlesShort = new [] {
            "Stunning iOS Apps",
            "Material Design",
            "Making apps better",
            "3 Platforms: 1 codebase",
            "Mastering XAML",
            "NuGet your code",
            "iBeacons",
            "Wearables and IoT",
            "The next great app",
            "iOS Best Practices",
            "Navigation patterns",
            "Is your app secure?",
            "Xamarin.Insights",
            "xUnit",
            "Test Cloud at MixRadio",
            "Reactive programming",
            "Augmented reality",
            "OWASP mobile security"
        };

        #endregion
    }
}

