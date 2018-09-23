using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
	[XmlRoot(Namespace="http://service.funbeatrace.com/MobileService.asmx", ElementName = "GetMyNewsFeedResponse", IsNullable=true)]
    public class GetMyNewsFeedResponse
    {
        [JsonProperty("d")]
		[XmlArray("GetMyNewsFeedResult")]
		[XmlArrayItem("MyNewsFeed")]
        public List<GetMyNewsFeedResult> Result { get; set; }
    }
}
