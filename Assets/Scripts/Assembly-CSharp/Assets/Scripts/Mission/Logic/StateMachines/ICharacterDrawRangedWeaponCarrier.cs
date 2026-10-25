using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface ICharacterDrawRangedWeaponCarrier : IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, ICharacterDrawWeaponCarrier
	{
		CharacterDrawWeaponRanged CharacterDrawRangedWeapon { get; }
	}
}
