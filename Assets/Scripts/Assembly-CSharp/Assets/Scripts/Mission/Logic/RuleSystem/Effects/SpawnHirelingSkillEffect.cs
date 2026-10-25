using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SpawnHirelingSkillEffect : SkillEffectBase<PlayerCharacter, ISkillPayload>
	{
		public SpawnHirelingSkillEffect(IRule _owner)
			: base(EEffectType.SpawnHireling, _owner, ETargetSource.RuleOwner)
		{
		}

		protected override IEffectInstance Apply(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			AvatarData.IAvatarDefinition hirelingDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetHirelingDefinition();
			SpawnHelper.SpawnHireling(hirelingDefinition, LogicMain.Hireling, LogicMain.LogicInterface.SelectedEntityId);
			return null;
		}
	}
}
