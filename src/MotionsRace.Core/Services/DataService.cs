using System.Collections.Generic;
using System.Linq;
using MotionsRace.Core.Models;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;

namespace MotionsRace.Core.Services
{
	public class DataService : IDataService
	{
		private static Dictionary<int, string> _icons = new Dictionary<int, string>()
		{
			{1001, "iconset01"},
			{1003, "iconset02"},
			{1005, "iconset03"},
			{1008, "iconset22"},
			{1015, "iconset21"},
			{1033, "iconset20"},
			{1050, "iconset04"},
			{1055, "iconset05"},
			{1090, "iconset12"},
			{1091, "iconset20"},
			{1092, "iconset11"},
			{1095, "iconset10"},
			{1130, "iconset19"},
			{1131, "iconset14"},
			{1133, "iconset15"},
			{1140, "iconset08"},
			{1155, "iconset13"},
			{1552, "iconset13"},
			{1250, "iconset12"},
			{1333, "iconset07"},
			{1441, "iconset15"},
			{1535, "iconset12"},
			{1700, "iconset12"},
			{3520, "iconset18"},
			{0, "iconset10"}
		};

		private static object lockSQLiteOperation = new object();
		private string _localDatabase = "local.db";
		private SQLiteConnection _connection;

		public DataService(IMvxSqliteConnectionFactory factory)
		{
			lock (lockSQLiteOperation)
			{
				_connection = factory.GetConnection(_localDatabase);
				_connection.CreateTable<GetTrainingTypesResult>();
			}
		}

		public void SaveTrainingTypes(GetTrainingTypesResponse trainingsTypes)
		{
			lock (lockSQLiteOperation)
			{
				_connection.DropTable<GetTrainingTypesResult>();
				_connection.CreateTable<GetTrainingTypesResult>();
				_connection.InsertAll(trainingsTypes.Result);
				_connection.Commit();
			}
		}

		public List<GetTrainingTypesResult> GetTrainingTypes(int catId = 0)
		{
			List<GetTrainingTypesResult> result;
			lock (lockSQLiteOperation)
			{
				if (catId == 0)
					result = _connection.Table<GetTrainingTypesResult>().ToList();
				else
					result =
						_connection.Table<GetTrainingTypesResult>()
							.Where(x => x.ActivityCategoryID == catId)
							.ToList()
							.OrderBy(x => x.ActivityCategoryID)
							.ToList();
			}
			return result;
		}

		public GetTrainingTypesResult GetTrainingTypeById(int trainingTypeID = 0)
		{
			var result = new GetTrainingTypesResult();
			lock (lockSQLiteOperation)
			{
				if (trainingTypeID != 0)
					result = _connection.Table<GetTrainingTypesResult>().FirstOrDefault(x => x.TrainingTypeID == trainingTypeID);
			}
			return result;
		}

		public List<TrainingCategory> GetTrainingCategories()
		{
			var trainings = new List<GetTrainingTypesResult>();
			lock (lockSQLiteOperation)
			{
				trainings = _connection.Table<GetTrainingTypesResult>().ToList();
			}

			var trainingsCategories = new List<TrainingCategory>();
			if (trainings.Count > 0)
			{
				trainingsCategories = trainings.Select(y => new TrainingCategory
				{
					ActivityCategoryID = y.ActivityCategoryID,
					ActivityCategoryName = y.ActivityCategoryName,
					IconFile = GetIconByTrainingsCategoryId(y.ActivityCategoryID, TrainingCategoryState.NotSelected),
					IconFileSelected = GetIconByTrainingsCategoryId(y.ActivityCategoryID, TrainingCategoryState.Selected),
					IsVisible = true,
					IsSelected = false,
				}).GroupBy(x => x.ActivityCategoryID).Select(y => y.First())
					.Where(x => x.ActivityCategoryID >= Constants.CATEGORY_MIN_ID && x.ActivityCategoryID <= Constants.CATEGORY_MAX_ID)
					.ToList()
					.OrderBy(x => x.ActivityCategoryID).ToList();
			}
			return trainingsCategories;
		}

		public string GetIconByTrainingsCategoryId(int catId, TrainingCategoryState selected)
		{
			string result;
			if (!_icons.TryGetValue(catId, out result))
			{
				result =  _icons[0];
			}
			return selected == TrainingCategoryState.Selected ? result + "sel" : result;
		}
	}
}