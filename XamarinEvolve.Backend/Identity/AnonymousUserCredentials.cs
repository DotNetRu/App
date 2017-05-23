using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XamarinEvolve.Backend.Identity
{
    public class AnonymousUserCredentials
    {
        [JsonProperty("anonymousUserId")]
        public string AnonymousUserId { get; set; }
    }
}