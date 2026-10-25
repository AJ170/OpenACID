using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class StopApproachingEffect<T> : EffectBase<T> where T : AssassinCharacter
	{
		public StopApproachingEffect(IRule _owner)
			: base(EEffectType.StopApproaching, _owner)
		{
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			_entity.StopMoving();
			return null;
		}
	}
}
