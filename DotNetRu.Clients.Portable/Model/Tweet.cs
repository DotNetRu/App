using System;
using System.Windows.Input;
using DotNetRu.Utils.Helpers;
using FormsToolkit;
using Humanizer;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DotNetRu.Clients.Portable.Model
{
    public class Tweet
    {
        private string _tweetedImage;

        private string _fullImage;

        [JsonIgnore]
        public bool HasImage => !string.IsNullOrWhiteSpace(this._tweetedImage);

        [JsonProperty("tweetedImage")]
        public string TweetedImage
        {
            get => this._tweetedImage;
            set
            {
                this._tweetedImage = value;
                this._fullImage = value;
                if (!string.IsNullOrWhiteSpace(this._tweetedImage))
                {
                    this._tweetedImage += ":thumb";
                }
            }
        }

        ICommand _fullImageCommand;

        public ICommand FullImageCommand => this._fullImageCommand
                                            ?? (this._fullImageCommand = new Command(this.ExecuteFullImageCommand));

        void ExecuteFullImageCommand()
        {
            if (string.IsNullOrWhiteSpace(this._fullImage)) return;
            MessagingService.Current.SendMessage(MessageKeys.NavigateToImage, this._fullImage);
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
        public string TitleDisplay => this.Name;

        [JsonIgnore]
        public string SubtitleDisplay => "@" + this.ScreenName;

        [JsonIgnore]
        public string DateDisplay => this.CreatedDate.Humanize();

        [JsonIgnore]
        public Uri TweetedImageUri
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(this.TweetedImage))
                    {
                        return null;
                    }

                    return new Uri(this.TweetedImage);
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

