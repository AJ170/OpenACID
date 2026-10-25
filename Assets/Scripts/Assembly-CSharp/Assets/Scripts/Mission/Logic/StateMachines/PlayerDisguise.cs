using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Utilities;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PlayerDisguise : StateMachineBase<AssassinCharacter, PlayerDisguise>
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.PlayerDisguise;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.PlayerDisguise_Undisguised;
			}
		}

		public PlayerDisguise(AssassinCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitPlayerDisguise()
		{
			StateMachineBase<AssassinCharacter, PlayerDisguise>.InitStateMachineBase();
			StateMachineBase<AssassinCharacter, PlayerDisguise>.AddState<State<AssassinCharacter>>(EStateType.PlayerDisguise_Disguised, OnEnterDisguised, OnExitDisguised, OnUpdate);
			StateMachineBase<AssassinCharacter, PlayerDisguise>.AddState<State<AssassinCharacter>>(EStateType.PlayerDisguise_Undisguised, null, null, OnUpdate);
			StateMachineBase<AssassinCharacter, PlayerDisguise>.AddTransition(EStateType.PlayerDisguise_Undisguised, EStateType.PlayerDisguise_Disguised);
			StateMachineBase<AssassinCharacter, PlayerDisguise>.AddTransition(EStateType.PlayerDisguise_Disguised, EStateType.PlayerDisguise_Undisguised);
		}

		private static void OnEnterDisguised(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<Character> _prevstate, IState<Character> _currentstate)
		{
			ToggleCharacterMesh(_statemachine.Entity, true);
		}

		private static void OnExitDisguised(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<Character> _prevstate, IState<Character> _currentstate)
		{
			ToggleCharacterMesh(_statemachine.Entity, false);
		}

		private static void OnUpdate(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<Character> _currentstate)
		{
			bool flag = _statemachine.Entity.EntityStats.Get_bool(StatTypes.EStatType.Player_Disguise);
			if (flag && _statemachine.CurrentStateType == EStateType.PlayerDisguise_Undisguised)
			{
				_statemachine.ChangeState(EStateType.PlayerDisguise_Disguised);
			}
			else if (!flag && _statemachine.CurrentStateType == EStateType.PlayerDisguise_Disguised)
			{
				_statemachine.ChangeState(EStateType.PlayerDisguise_Undisguised);
			}
		}

		private static void ToggleCharacterMesh(AssassinCharacter _entity, bool _useAlternativeMesh)
		{
			GameObject gameObject = _entity.GameObject;
			AlternativeCharacterMesh component = gameObject.GetComponent<AlternativeCharacterMesh>();
			component.UseAlternativeMesh = _useAlternativeMesh;
			for (int i = 0; i < 2; i++)
			{
				WeaponHandler weaponHandler = _entity.WeaponHandlerCache.GetWeaponHandler((EWeaponCategory)i);
				if (weaponHandler != null)
				{
					weaponHandler.ShowWeapon(!_useAlternativeMesh);
				}
			}
		}
	}
}
