using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AnimationStateMachineStateSkillTrigger<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public AnimatorStateMachineStatic.StateHashValue StateHash { get; private set; }

		public AnimationTaskManager.ELayer Layer { get; private set; }

		public AnimationStateMachineStateSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource, AnimatorStateMachineStatic.StateHashValue _stateHash, AnimationTaskManager.ELayer _layer)
			: base(ETriggerType.AnimationStateMachineState, _owner, _frequency, _categoryFlags, _targetSource)
		{
			StateHash = _stateHash;
			Layer = _layer;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return TargetEntityId.GetEntity<IGameObjectEntity>().UnityComponentCache.AnimationController.StateMachine.GetCurrentState(Layer) == StateHash.NameHash;
		}
	}
}
