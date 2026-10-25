using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class PatrolDefinition : Definition
	{
		public PatrolData.IPatrolDefinition Definition
		{
			get
			{
				return (PatrolData.IPatrolDefinition)base.EntityDefinition;
			}
			set
			{
				base.EntityDefinition = value;
			}
		}

		public PatrolDefinition(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
