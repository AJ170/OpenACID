using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IObjectiveBase : IMissionBase
	{
		string Description { get; }

		ObjectiveBaseData ObjectiveData { get; }

		EProgressState VisualTrackerState { get; }

		EObjectiveResult Result { get; }

		string ShortDescription { get; }

		string SecondaryDescription { get; }

		BBList<Vector3> TargetPositions { get; }

		bool VisualSummaryState { get; }

		string RawDescription { get; }

		string RawShortDescription { get; }

		string RawSecondaryDescription { get; }

		event DescriptionChangedDelegate OnDescriptionChanged;

		event StateChanged OnStateChanged;

		void PrepareRevive();

		void UpdateDescription();

		void UpdateFrame();

		void UpdateTurn();

		StringMapper GetStringMapper();

		void PrepareRawDescriptions();
	}
}
