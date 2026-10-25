using System;
using AcierProtocol;
using Mission;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class ObjectiveResultWrapper
	{
		public AcierProtocol.EObjectiveResult Result = AcierProtocol.EObjectiveResult.Failed_EObjectiveResult;

		public EObjectiveProgress Tracker = EObjectiveProgress.Failed_EObjectiveProgress;

		public ObjectiveResultWrapper(ObjectiveResult _completionResult)
		{
			if (_completionResult != null)
			{
				Result = _completionResult.Result;
				Tracker = _completionResult.Tracker;
			}
		}

		public ObjectiveResultWrapper(AcierProtocol.EObjectiveResult _result, EObjectiveProgress _tracker)
		{
			Result = _result;
			Tracker = _tracker;
		}

		public ObjectiveResult ToProtobuf()
		{
			ObjectiveResult objectiveResult = new ObjectiveResult();
			objectiveResult.Tracker = Tracker;
			objectiveResult.Result = Result;
			return objectiveResult;
		}

		public static bool ToSummaryState(EObjectiveProgress _tracker)
		{
			switch (_tracker)
			{
			case EObjectiveProgress.Succeeded_EObjectiveProgress:
			case EObjectiveProgress.InRange_EObjectiveProgress:
			case EObjectiveProgress.InProgress_EObjectiveProgress:
				return true;
			default:
				return false;
			}
		}

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

		public static global::Mission.EObjectiveResult ToResult(AcierProtocol.EObjectiveResult _initialResult)
		{
			switch (_initialResult)
			{
			case AcierProtocol.EObjectiveResult.Undefined_EObjectiveResult:
				return global::Mission.EObjectiveResult.Undefined;
			case AcierProtocol.EObjectiveResult.Succeeded_EObjectiveResult:
				return global::Mission.EObjectiveResult.Fulfilled;
			case AcierProtocol.EObjectiveResult.Failed_EObjectiveResult:
				return global::Mission.EObjectiveResult.Failed;
			default:
				throw new ArgumentOutOfRangeException("_initialResult");
			}
		}
	}
}
