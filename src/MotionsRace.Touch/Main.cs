using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace MotionsRace.Touch
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.

			// -------- Xamarin Insights integration START -----

			//Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
			//{
			//	if (isStartupCrash) {
			//		Insights.PurgePendingCrashReports().Wait();
			//	}
			//};

			//#if DEBUG
			//Insights.Initialize(Insights.DebugModeKey);
			//#else
			//Insights.Initialize("0cbbe56579e4c167c34a9aa6a0b0ea08ef1f85bb");
			//#endif

			// -------- Xamarin Insights integration END -----

            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}