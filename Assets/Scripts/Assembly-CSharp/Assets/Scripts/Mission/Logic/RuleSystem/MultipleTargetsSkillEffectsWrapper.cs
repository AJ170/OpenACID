using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public class MultipleTargetsSkillEffectsWrapper : SkillEffectsWrapper
	{
		private readonly BBList<EntityId> m_TargetEntitiyIds = new BBList<EntityId>();

		public BBList<EntityId> TargetEntitiyIds
		{
			get
			{
				return m_TargetEntitiyIds;
			}
		}

		public MultipleTargetsSkillEffectsWrapper(BBList<IEffect> _effects)
			: base(_effects)
		{
		}

		public MultipleTargetsSkillEffectsWrapper(BBList<IEffect> _effects, Predicate<IEffect, IRulesCarrier> _predicate)
			: base(_effects, _predicate)
		{
		}

		protected MultipleTargetsSkillEffectsWrapper(BBList<IEffect> _effects, ISkillPayload _payload, Predicate<IEffect, IRulesCarrier> _predicate, IRulesCarrier _entity, BuffedReadonlyStatValueCollection _statValueCollection)
			: base(_effects, _payload, _predicate, _entity, _statValueCollection)
		{
		}

		protected override void PrepareEffect(IEffect _effect)
		{
			base.PrepareEffect(_effect);
			IMultipleTargetsEffect multipleTargetsEffect = _effect as IMultipleTargetsEffect;
			if (multipleTargetsEffect != null)
			{
				multipleTargetsEffect.TargetEntityIds.ClearIfNotEmpty();
				multipleTargetsEffect.TargetEntityIds.AddRange(TargetEntitiyIds);
			}
		}
	}
}
