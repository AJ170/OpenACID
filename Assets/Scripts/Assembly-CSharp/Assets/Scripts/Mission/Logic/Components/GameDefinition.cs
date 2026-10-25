using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class GameDefinition : Definition
	{
		public GameData.IGameDefinition Definition
		{
			get
			{
				return (GameData.IGameDefinition)base.EntityDefinition;
			}
			set
			{
				base.EntityDefinition = value;
			}
		}

		public GameDefinition(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
