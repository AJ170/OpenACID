using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class AvatarDefinition : CharacterDefinition
	{
		public new AvatarData.AvatarDefinition Definition
		{
			get
			{
				return (AvatarData.AvatarDefinition)base.Definition;
			}
			set
			{
				base.Definition = value;
			}
		}

		public AvatarDefinition(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
