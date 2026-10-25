namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class CharacterVitality : CharacterVitalityBase<ICharacterVitalityCarrierEntity, CharacterVitality>
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.CharacterVitality;
			}
		}

		public CharacterVitality(ICharacterVitalityCarrierEntity _entity)
			: base(_entity)
		{
		}

		public static void InitCharacterVitality()
		{
			CharacterVitalityBase<ICharacterVitalityCarrierEntity, CharacterVitality>.InitCharacterVitalityBase();
		}
	}
}
