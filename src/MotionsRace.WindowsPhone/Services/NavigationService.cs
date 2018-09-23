using MotionsRace.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MotionsRace.WindowsPhone.Services
{
    public class NavigationService : INavigationService
    {
        public void GoBack()
        {
            var frame = Window.Current.Content as Frame;
            frame.GoBack();
        }

        public void ClearNavStack()
        {
            var frame = Window.Current.Content as Frame;
            frame.BackStack.Clear();
        }

        public void ClearPrevPageFromBackStack()
        {
            var frame = Window.Current.Content as Frame;
            frame.BackStack.Remove(frame.BackStack.Last());
        }
    }
}