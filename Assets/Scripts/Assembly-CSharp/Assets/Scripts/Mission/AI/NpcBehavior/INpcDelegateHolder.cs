using Assets.Scripts.Mission.AI.Actions;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Knowledge;

namespace Assets.Scripts.Mission.AI.NpcBehavior
{
	public interface INpcDelegateHolder : IDelegateHolder
	{
		ActionsContainer ActionsConditions { get; }

		CharacterKnowledgeContainer Knowledge { get; }
	}
}
