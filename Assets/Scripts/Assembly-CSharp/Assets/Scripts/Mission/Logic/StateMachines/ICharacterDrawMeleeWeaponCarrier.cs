using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface ICharacterDrawMeleeWeaponCarrier : IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, ICharacterDrawWeaponCarrier
	{
		CharacterDrawWeaponMelee CharacterDrawMeleeWeapon { get; }
	}
}
