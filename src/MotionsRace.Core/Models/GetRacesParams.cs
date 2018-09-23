using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
	[XmlRootAttribute("GetRacesAPersonCanLoginTo")]
    public class GetRacesParams
    {
        public string applicationID { get; set; }
        public string loginID { get; set; }
        public string loginSecret { get; set; }
        public string hostName { get; set; }      
    }
}
