using AcierProtocol;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.UI
{
	public class CombatUiEvent : LogicUiEvent
	{
		public enum EType
		{
			Hit = 0,
			Heal = 1,
			DodgeNormal = 2,
			DodgeMiss = 3,
			KillMoveDeflect = 4,
			Enter = 5,
			Exit = 6,
			Select = 7,
			PrepareAttack = 8,
			Timer = 9,
			SoftCurrency = 10,
			SkillAvailable = 11,
			SkillUnavailable = 12,
			MissionGoalLoot = 13
		}

		public Character Source { get; private set; }

		public GameEntity Target { get; private set; }

		public float Delta { get; private set; }

		public EType Type { get; private set; }

		public Enum<SkillTypes> SkillType { get; private set; }

		public Assets.Scripts.Mission.Logic.Components.LogicInterface.ECombatEventModifierFlags Flags { get; private set; }

		public MissionGoalLoot MissionGoalLoot { get; private set; }

		private CombatUiEvent(Character _source, GameEntity _target)
		{
			Source = _source;
			Target = _target;
		}

		private CombatUiEvent(Character _source, GameEntity _target, EType _type)
			: this(_source, _target)
		{
			Type = _type;
		}

		public static CombatUiEvent CreateHitEvent(Character _source, Character _target, float _delta, Assets.Scripts.Mission.Logic.Components.LogicInterface.ECombatEventModifierFlags _flags)
		{
			CombatUiEvent combatUiEvent = new CombatUiEvent(_source, _target, EType.Hit);
			combatUiEvent.Delta = _delta;
			combatUiEvent.Flags = _flags;
			return combatUiEvent;
		}

		public static CombatUiEvent CreateHealEvent(Character _source, Character _target, float _delta, Assets.Scripts.Mission.Logic.Components.LogicInterface.ECombatEventModifierFlags _flags)
		{
			CombatUiEvent combatUiEvent = new CombatUiEvent(_source, _target, EType.Heal);
			combatUiEvent.Delta = _delta;
			combatUiEvent.Flags = _flags;
			return combatUiEvent;
		}

		public static CombatUiEvent CreateDodgeEvent(Character _source, Character _target, bool _missed)
		{
			return new CombatUiEvent(_source, _target, (!_missed) ? EType.DodgeNormal : EType.DodgeMiss);
		}

		public static CombatUiEvent CreateKillMoveDefelectEvent(Character _source, Character _target)
		{
			return new CombatUiEvent(_source, _target, EType.KillMoveDeflect);
		}

		public static CombatUiEvent CreateTimerEvent(Character _target, int _countdown)
		{
			CombatUiEvent combatUiEvent = new CombatUiEvent(null, _target, EType.Timer);
			combatUiEvent.Delta = _countdown;
			return combatUiEvent;
		}

		public static CombatUiEvent CreateSoftCurrencyEvent(Character _target, int _coins)
		{
			CombatUiEvent combatUiEvent = new CombatUiEvent(null, _target, EType.SoftCurrency);
			combatUiEvent.Delta = _coins;
			return combatUiEvent;
		}

		public static CombatUiEvent CreateEnterEvent(Character _target)
		{
			return new CombatUiEvent(null, _target, EType.Enter);
		}

		public static CombatUiEvent CreateExitEvent(Character _target)
		{
			return new CombatUiEvent(null, _target, EType.Exit);
		}

		public static CombatUiEvent CreateSelectTargetEvent(Character _target)
		{
			return new CombatUiEvent(null, _target, EType.Select);
		}

		public static CombatUiEvent CreatePrepareAttack(Character _source, Character _target, float _delta)
		{
			CombatUiEvent combatUiEvent = new CombatUiEvent(_source, _target, EType.PrepareAttack);
			combatUiEvent.Delta = _delta;
			return combatUiEvent;
		}

		public static CombatUiEvent CreateMissionGoalLoot(GameEntity _targetEntity, MissionGoalLoot _loot)
		{
			CombatUiEvent combatUiEvent = new CombatUiEvent(null, _targetEntity, EType.MissionGoalLoot);
			combatUiEvent.Target = _targetEntity;
			combatUiEvent.MissionGoalLoot = _loot;
			return combatUiEvent;
		}

		public static ILogicUiEvent CreateSkillAvailabilityEvent(Character _source, Enum<SkillTypes> _skillType, bool _available)
		{
			CombatUiEvent combatUiEvent = new CombatUiEvent(_source, _source);
			combatUiEvent.Type = ((!_available) ? EType.SkillUnavailable : EType.SkillAvailable);
			combatUiEvent.SkillType = _skillType;
			return combatUiEvent;
		}
	}
}
