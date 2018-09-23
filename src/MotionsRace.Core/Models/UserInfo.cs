using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionsRace.Core.Models
{
    public class UserInfo
    {
        public int PersonID {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Motto {get; set;}
        public string PictureURL {get; set;}
		[System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
		public string ImageGUID { get; set; }
    }
}
