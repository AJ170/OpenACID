using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace UbiCore.Mission.Ai
{
	public interface IAIUpdateable
	{
		void UpdateTurn(ITreeRuntimeData _runtimeData);
	}
}
