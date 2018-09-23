using System.Collections.Generic;


namespace MotionsRace.Core.Services
{
	public interface IDataService
	{
		void SaveTrainingTypes(GetTrainingTypesResponse trainingsTypes);
		List<GetTrainingTypesResult> GetTrainingTypes (int catid = 0);
		GetTrainingTypesResult GetTrainingTypeById (int TrainingTypeID = 0);
		List<TrainingCategory> GetTrainingCategories ();
	}
}