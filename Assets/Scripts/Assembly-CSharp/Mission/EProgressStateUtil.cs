using System;
using AcierProtocol;

namespace Mission
{
	public static class EProgressStateUtil
	{
		public static EProgressState ToTrackerState(EObjectiveProgress _tracker)
		{
			switch (_tracker)
			{
			case EObjectiveProgress.NotStarted_EObjectiveProgress:
				return EProgressState.NotStarted;
			case EObjectiveProgress.Succeeded_EObjectiveProgress:
				return EProgressState.Fulfilled;
			case EObjectiveProgress.Failed_EObjectiveProgress:
				return EProgressState.Failed;
			case EObjectiveProgress.InRange_EObjectiveProgress:
				return EProgressState.InRange;
			case EObjectiveProgress.InProgress_EObjectiveProgress:
				return EProgressState.InProgress;
			default:
				throw new ArgumentOutOfRangeException("_tracker");
			}
		}
	}
}
