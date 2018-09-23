using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MotionsRace.Core
{
	public static class ListExtentions
	{
		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> thisCollection)
		{
			if (thisCollection == null) return null;
			var oc = new ObservableCollection<T>();

			foreach (var item in thisCollection)
			{
				oc.Add(item);
			}

			return oc;
		}
	}


}

