using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AnimationStateMachineSkillTrigger<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public AnimatorStateMachineStatic.StateMachineHashValue StateMachineHash { get; private set; }

		public AnimationTaskManager.ELayer Layer { get; private set; }

		public AnimationStateMachineSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource, AnimatorStateMachineStatic.StateMachineHashValue _stateMachineHash, AnimationTaskManager.ELayer _layer)
			: base(ETriggerType.AnimationStateMachine, _owner, _frequency, _categoryFlags, _targetSource)
		{
			StateMachineHash = _stateMachineHash;
			Layer = _layer;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return TargetEntityId.GetEntity<IGameObjectEntity>().UnityComponentCache.AnimationController.StateMachine.GetCurrentStateMachine(Layer).CompareNameHash(StateMachineHash);
		}
	}
}
