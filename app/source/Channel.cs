using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VNMC2013
{
    public class Channel
    {
        public int Id { get; set; }
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}
