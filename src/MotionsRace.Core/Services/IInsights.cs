using System;
using System.Collections;

namespace MotionsRace.Core.Services
{
	public interface IInsights
	{
		void Report(Exception exception, string key, string value, Severity warningLevel = 0);
		void Report(Exception exception, IDictionary extraData, Severity warningLevel = 0);
		void Report(Exception exception = null, Severity warningLevel = 0);
	}

	public enum Severity
	{
		Warning,
		Error,
		Critical
	}
}