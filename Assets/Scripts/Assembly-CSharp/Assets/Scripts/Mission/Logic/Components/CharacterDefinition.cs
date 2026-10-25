using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class CharacterDefinition : Definition
	{
		public CharacterData.ICharacterDefinition Definition
		{
			get
			{
				return (CharacterData.ICharacterDefinition)base.EntityDefinition;
			}
			set
			{
				base.EntityDefinition = value;
			}
		}

		protected CharacterDefinition(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
