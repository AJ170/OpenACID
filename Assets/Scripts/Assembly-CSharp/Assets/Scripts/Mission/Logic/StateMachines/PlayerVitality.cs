using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Selection;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PlayerVitality : CombatCharacterVitalityBase<PlayerCharacter, PlayerVitality>
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.PlayerVitality;
			}
		}

		public PlayerVitality(PlayerCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitPlayerVitality()
		{
			CombatCharacterVitalityBase<PlayerCharacter, PlayerVitality>.InitCombatCharacterVitalityBase();
			StateMachineBase<PlayerCharacter, PlayerVitality>.OverrideStateHandlers(EStateType.Character_Immortal, null, null, OnUpdateImmortal);
			StateMachineBase<PlayerCharacter, PlayerVitality>.OverrideStateHandlers(EStateType.Character_Alive, null, null, OnUpdateAlive);
			StateMachineBase<PlayerCharacter, PlayerVitality>.OverrideStateHandlers(EStateType.Character_Dead, OnEnterDead, null, null);
			StateMachineBase<PlayerCharacter, PlayerVitality>.AddTransition(EStateType.Character_Dead, EStateType.Character_Alive);
		}

		private static void OnUpdateImmortal(IStateMachine<PlayerCharacter> _stateMachine, IPayload _payload, IState<PlayerCharacter> _currentState)
		{
			Character entity = _stateMachine.Entity;
			CombatHelper.DecayKillTokenCount(entity);
			_stateMachine.Entity.AssassinInteraction.Update(UbiCore.Utils.Singleton<SelectionManager>.Instance.SelectedEntityId);
		}

		private new static void OnUpdateAlive(IStateMachine<PlayerCharacter> _stateMachine, IPayload _payload, IState<PlayerCharacter> _currentState)
		{
			CombatCharacterVitalityBase<PlayerCharacter, PlayerVitality>.OnUpdateAlive(_stateMachine, _payload, _currentState);
			Character entity = _stateMachine.Entity;
			CombatHelper.DecayKillTokenCount(entity);
			_stateMachine.Entity.AssassinInteraction.Update(UbiCore.Utils.Singleton<SelectionManager>.Instance.SelectedEntityId);
			EntityStats entityStats = entity.EntityStats;
			float num = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualDamageMultiplier);
			float num2 = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualWeaponDamage);
			float sourceStrength = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualStrength);
			float overrideValue = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_StrengthDamageOverride);
			float num3 = DerivedStatsHelper.StrengthDamage(sourceStrength, overrideValue);
			entityStats.Set_float(StatTypes.EStatType.Player_TotalDamage, num * (num2 + num3));
		}

		public new static void OnEnterDead(IStateMachine<PlayerCharacter> _stateMachine, IPayload _payload, IState<PlayerCharacter> _prevState, IState<PlayerCharacter> _currentState)
		{
			CombatCharacterVitalityBase<PlayerCharacter, PlayerVitality>.OnEnterDead(_stateMachine, _payload, _prevState, _currentState);
		}
	}
}
