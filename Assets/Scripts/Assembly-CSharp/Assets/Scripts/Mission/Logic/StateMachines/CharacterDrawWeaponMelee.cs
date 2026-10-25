using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class CharacterDrawWeaponMelee : CharacterDrawWeaponBase
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.CharacterDrawWeaponMelee;
			}
		}

		public CharacterDrawWeaponMelee(Character _entity, EWeaponType _type)
			: base(_entity, EWeaponCategory.Melee, _type)
		{
		}
	}
}
