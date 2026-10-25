using AcierProtocol;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Network.Missions.Mission.Data;
using SharedBaseLib.Core;

public class MissionGameLogicMod : MissionBase<MissionGameLogicModData>
{
	private class Counter
	{
		public int Count = 1;

		public object Default { get; private set; }

		public Counter(object _value)
		{
			Default = _value;
		}
	}

	public enum ELogicValue
	{
		KillTokenKill = 0,
		SetHealthPercent = 1
	}

	private static BBDictionary<int, Counter> s_logicStateCounter = new BBDictionary<int, Counter>();

	public override void OnActivate()
	{
		base.OnActivate();
		if (base.Data.Combat.KillTokenKill != MissionGameLogicModContent.EBool.Null)
		{
			Register(ELogicValue.KillTokenKill, CombatController.Instance.EnableKillTokenKillRuntime);
			CombatController.Instance.EnableKillTokenKillRuntime = base.Data.Combat.KillTokenKill == MissionGameLogicModContent.EBool.True;
		}
		if (base.Data.Health.SetHealthPercentage != MissionGameLogicModContent.EBool.Null)
		{
			float num = LogicMain.PlayerEntity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
			LogicMain.PlayerEntity.EntityStats.Set_float(StatTypes.EStatType.Character_Conflict_Health, num * (float)base.Data.Health.Percentage / 100f);
		}
	}

	private static void Register(ELogicValue _type, object _value)
	{
		if (!s_logicStateCounter.ContainsKey((int)_type))
		{
			s_logicStateCounter[(int)_type] = new Counter(_value);
		}
		else
		{
			s_logicStateCounter[(int)_type].Count++;
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		if (base.Data.Combat.KillTokenKill != MissionGameLogicModContent.EBool.Null)
		{
			CombatController.Instance.EnableKillTokenKillRuntime = Unregister(ELogicValue.KillTokenKill, CombatController.Instance.EnableKillTokenKillRuntime);
		}
	}

	private static T Unregister<T>(ELogicValue _key, T _value)
	{
		Counter counter = s_logicStateCounter[(int)_key];
		counter.Count--;
		if (counter.Count > 0)
		{
			return _value;
		}
		T result = (T)counter.Default;
		s_logicStateCounter.Remove((int)_key);
		return result;
	}
}
