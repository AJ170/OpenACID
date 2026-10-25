using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Mission.AI.Formations
{
	public class FormationFactory
	{
		public static IFormation CreateFormation(EntityId _parentId, PatrolData.IPatrolDefinition _definition, bool _fixedLeaderSlot)
		{
			IFormation formation = null;
			EFormationType formationType = _definition.FormationType;
			if (formationType == EFormationType.Circle)
			{
				formation = new FormationCircle();
			}
			formation.Init(_parentId, _definition.MaxExtend, _definition.MaxRandomDeviation, _fixedLeaderSlot);
			formation.MaxCount = _definition.MaxCount;
			return formation;
		}
	}
}
