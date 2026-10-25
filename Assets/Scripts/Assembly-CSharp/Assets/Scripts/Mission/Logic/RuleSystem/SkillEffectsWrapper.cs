using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public class SkillEffectsWrapper : EffectsWrapper
	{
		public ISkillPayload Payload { get; set; }

		public SkillEffectsWrapper(BBList<IEffect> _effects)
			: base(_effects)
		{
		}

		public SkillEffectsWrapper(BBList<IEffect> _effects, Predicate<IEffect, IRulesCarrier> _predicate)
			: base(_effects, _predicate)
		{
		}

		protected SkillEffectsWrapper(BBList<IEffect> _effects, ISkillPayload _payload, Predicate<IEffect, IRulesCarrier> _predicate, IRulesCarrier _entity, BuffedReadonlyStatValueCollection _statValueCollection)
			: base(_effects, _predicate, _entity, _statValueCollection)
		{
			Payload = _payload;
		}

		protected override void PrepareEffect(IEffect _effect)
		{
			ISkillEffect skillEffect = _effect as ISkillEffect;
			if (skillEffect != null)
			{
				skillEffect.Payload = Payload;
			}
			base.PrepareEffect(_effect);
		}

		public override object Clone()
		{
			return new SkillEffectsWrapper(m_Effects, Payload, m_EffectsPredicate, base.Entity, base.StatValueCollection);
		}
	}
}
