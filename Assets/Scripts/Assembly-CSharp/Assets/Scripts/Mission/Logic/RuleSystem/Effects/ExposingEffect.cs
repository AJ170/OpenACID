using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	internal class ExposingEffect : EffectBase<AssassinCharacter>
	{
		public ExposingEffect(IRule _owner)
			: base(EEffectType.Exposing, _owner)
		{
		}

		protected override IEffectInstance Apply(AssassinCharacter _assassin, IBuffedStatsCollectionHandle _stats)
		{
			_assassin.RegisterExposingInteraction();
			return null;
		}
	}
}
