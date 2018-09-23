using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
	[XmlRoot(Namespace="http://service.funbeatrace.com/MobileService.asmx", ElementName = "AuthorizeRaceAccessResponse", IsNullable=true)]
    public class AuthorizeRaceAccessResponse
    {
        [JsonProperty("d")]
		[XmlElement("AuthorizeRaceAccessResult")]
        public AuthorizeRaceAccessResult Result { get; set; }
    }
}
