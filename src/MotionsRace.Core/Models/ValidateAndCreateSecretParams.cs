using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
	[XmlRootAttribute("ValidateAndCreateSecret")]
    public class ValidateAndCreateSecretParams
    {
        public string applicationID { get; set; }
        public string username { get; set; }
        public string secret { get; set; }
        public string method { get; set; }
    }
}
