using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionsRace.Core.Services
{
    public interface INavigationService
    {
        void GoBack();
        void ClearNavStack();
        void ClearPrevPageFromBackStack();
    }
}
