using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public interface ITreeRuntimeData
	{
		BBList<BehaviorComponent> LastTicked { get; set; }

		BBHashSet<BehaviorComponent> LastTickedSet { get; }

		void Init();

		void Dispose();

		IBehaviorRuntimeData RequestBehaviorRuntimeData(BehaviorComponent _component);
	}
}
