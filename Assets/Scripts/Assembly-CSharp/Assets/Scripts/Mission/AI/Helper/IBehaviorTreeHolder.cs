using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using UbiCore.Mission.Ai;

namespace Assets.Scripts.Mission.AI.Helper
{
	public interface IBehaviorTreeHolder : IAIUpdateable
	{
		BehaviorTree BehaviorTree { get; }

		void Init(IDelegateHolder _delegateHolder, string _assetPath);
	}
}
