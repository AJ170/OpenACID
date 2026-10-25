using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.AI.Knowledge
{
	public class CombatKnowledge : NpcBehaviorComponent
	{
		public CombatKnowledge(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public bool CombatActive()
		{
			return CombatController.IsActive;
		}

		public bool InCombat()
		{
			return base.NpcCharacter.GetIsInCombat();
		}

		public bool MeleeWeaponUnsheathedChoosen()
		{
			ICharacterDrawMeleeWeaponCarrier characterDrawMeleeWeaponCarrier = base.NpcCharacter.StateMachines as ICharacterDrawMeleeWeaponCarrier;
			return characterDrawMeleeWeaponCarrier != null && characterDrawMeleeWeaponCarrier.CharacterDrawMeleeWeapon.ChosenWeaponState == EWeaponState.Unsheathed;
		}

		public bool HasRangedWeapon()
		{
			ICharacterDrawRangedWeaponCarrier weaponCarrier = base.NpcCharacter.StateMachines as ICharacterDrawRangedWeaponCarrier;
			return HasRangedWeapon(weaponCarrier);
		}

		public bool HasBow()
		{
			return HasRangedWeapon(EWeaponType.Bow);
		}

		public bool HasCrossbow()
		{
			return HasRangedWeapon(EWeaponType.Crossbow);
		}

		public bool IsRangedSheathed()
		{
			return IsRangedInState(EWeaponState.Sheathed);
		}

		public bool DidSeeBombExplosion()
		{
			return base.NpcCharacter.NpcStats.Get_bool(StatTypes.EStatType.Npc_DidSeeBombExplosion);
		}

		public bool ConflictCountBelowCap()
		{
			int num = CombatController.Instance.AvailableParticipantSlots();
			int num2 = LogicMain.PlayerEntity.PlayerStats.Get_int(StatTypes.EStatType.Player_ChasingEnemies);
			return num - num2 > 0;
		}

		private bool IsRangedInState(EWeaponState _weaponState)
		{
			ICharacterDrawRangedWeaponCarrier characterDrawRangedWeaponCarrier = base.NpcCharacter.StateMachines as ICharacterDrawRangedWeaponCarrier;
			return characterDrawRangedWeaponCarrier != null && characterDrawRangedWeaponCarrier.CharacterDrawRangedWeapon != null && characterDrawRangedWeaponCarrier.CharacterDrawRangedWeapon.CurrentStateType == _weaponState.ToStateType();
		}

		private bool HasRangedWeapon(EWeaponType _weaponType)
		{
			ICharacterDrawRangedWeaponCarrier characterDrawRangedWeaponCarrier = base.NpcCharacter.StateMachines as ICharacterDrawRangedWeaponCarrier;
			return HasRangedWeapon(characterDrawRangedWeaponCarrier) && characterDrawRangedWeaponCarrier.CharacterDrawRangedWeapon.WeaponType == _weaponType;
		}

		private static bool HasRangedWeapon(ICharacterDrawRangedWeaponCarrier _weaponCarrier)
		{
			return _weaponCarrier != null && _weaponCarrier.CharacterDrawRangedWeapon != null && _weaponCarrier.CharacterDrawRangedWeapon.ChosenWeaponState != EWeaponState.Detached;
		}
	}
}
