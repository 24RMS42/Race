using System;
using System.Collections;
using MotionsRace.Core.Services;
using Xamarin;


namespace MotionsRace.Touch
{
	public class XamarinInsights : IInsights
	{
		public void Report(Exception exception, string key, string value, Severity warningLevel = Severity.Warning)
		{
			Insights.Report(exception, key, value, GetXamarinInsightsSeverity(warningLevel));
		}

		public void Report(Exception exception, IDictionary extraData, Severity warningLevel = Severity.Warning)
		{
			Insights.Report(exception, extraData, GetXamarinInsightsSeverity(warningLevel));
		}

		public void Report(Exception exception = null, Severity warningLevel = Severity.Warning)
		{
			Insights.Report(exception, GetXamarinInsightsSeverity(warningLevel));
		}

		private Insights.Severity GetXamarinInsightsSeverity(Severity value)
		{
			switch (value)
			{
				case Severity.Error:
					return Insights.Severity.Error;
				case Severity.Critical :
					return Insights.Severity.Critical;
			}
			return Insights.Severity.Warning;
		}
	}
}