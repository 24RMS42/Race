using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionsRace.Core.Models
{
    public class LoginResponse
    {
        public string LoginID { get; set; }
        public string LoginSecretHashed { get; set; }
        public int PersonID { get; set; }
    }
}
