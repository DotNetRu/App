using System;
using System.Windows.Input;
using Xamarin.Forms;
using FormsToolkit;
using Newtonsoft.Json;
using Humanizer;
using XamarinEvolve.Utils;

namespace XamarinEvolve.Clients.Portable
{
	using XamarinEvolve.Utils.Helpers;

	public class Tweet
    {
        string _tweetedImage;
        string _fullImage;

        [JsonIgnore]
        public bool HasImage => !string.IsNullOrWhiteSpace(_tweetedImage);

        [JsonProperty("tweetedImage")]
        public string TweetedImage
        {
            get => _tweetedImage;
            set
            {
                _tweetedImage = value;
                _fullImage = value;
                if (!string.IsNullOrWhiteSpace(_tweetedImage))
                {
                    _tweetedImage += ":thumb";
                }
            }
        }

        ICommand _fullImageCommand;

        public ICommand FullImageCommand =>
            _fullImageCommand ?? (_fullImageCommand = new Command(ExecuteFullImageCommand));

        void ExecuteFullImageCommand()
        {
            if (string.IsNullOrWhiteSpace(_fullImage))
                return;
            MessagingService.Current.SendMessage(MessageKeys.NavigateToImage, _fullImage);
        }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screenName")]
        public string ScreenName { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public string TitleDisplay => Name;

        [JsonIgnore]
        public string SubtitleDisplay => "@" + ScreenName;

        [JsonIgnore]
        public string DateDisplay => CreatedDate.Humanize();

        [JsonIgnore]
        public Uri TweetedImageUri
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(TweetedImage))
                        return null;

                    return new Uri(TweetedImage);
                }
                catch
                {
                    // TODO ignored
                }
                return null;
            }
        }
    }
}

