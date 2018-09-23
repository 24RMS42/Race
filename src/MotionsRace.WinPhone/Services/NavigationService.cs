using Microsoft.Phone.Controls;
using MotionsRace.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MotionsRace.WinPhone.Services
{
    public class NavigationService : INavigationService
    {
        public void GoBack()
        {
            var frame = ((PhoneApplicationFrame)Application.Current.RootVisual);
            frame.GoBack();
        }

        public void ClearNavStack()
        {
            var frame = ((PhoneApplicationFrame)Application.Current.RootVisual);
            while (frame.BackStack.Any())
            {
                frame.RemoveBackEntry();
            }

        }

        public void ClearPrevPageFromBackStack()
        {
            //var frame = ((PhoneApplicationFrame)Application.Current.RootVisual);
            //frame.BackStack.Remove(frame.BackStack.Last());
        }
    }
}
