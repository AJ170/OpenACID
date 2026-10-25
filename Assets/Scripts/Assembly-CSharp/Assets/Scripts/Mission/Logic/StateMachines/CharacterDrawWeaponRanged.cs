using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class CharacterDrawWeaponRanged : CharacterDrawWeaponBase
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.CharacterDrawWeaponRanged;
			}
		}

		public CharacterDrawWeaponRanged(Character _entity, EWeaponType _type)
			: base(_entity, EWeaponCategory.Ranged, _type)
		{
		}
	}
}
