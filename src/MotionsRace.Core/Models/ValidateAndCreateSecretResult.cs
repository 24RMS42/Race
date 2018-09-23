using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionsRace.Core.Models
{
    public class ValidateAndCreateSecretResult
    {
        public string LoginID { get; set; }
        public string LoginSecret { get; set; }
        public string Message { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
