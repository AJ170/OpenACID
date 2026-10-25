using AcierProtocol;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.UI.State;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AvatarClassTrigger : TriggerBase<PlayerCharacter>
	{
		private readonly Assassin.EClassTypeId m_ClassType;

		public AvatarClassTrigger(IRule _owner, Assassin.EClassTypeId _classType)
			: base(ETriggerType.AvatarClass, _owner)
		{
			m_ClassType = _classType;
		}

		protected override bool IsActive(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.classTypeId == m_ClassType || Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.extendedClassTypeId == m_ClassType;
		}
	}
}
