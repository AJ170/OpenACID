using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;

public class ObjectiveKill : ObjectiveTarget<ObjectiveKillData>
{
	protected override bool InitialVisualSummaryState
	{
		get
		{
			return base.Data.DoNotKill;
		}
	}

	protected override EObjectiveResult InitialResult
	{
		get
		{
			return base.Data.DoNotKill ? EObjectiveResult.Fulfilled : EObjectiveResult.Undefined;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return base.Data.DoNotKill ? EProgressState.InProgress : EProgressState.NotStarted;
		}
	}

	private ObjectiveKill()
	{
	}

	public override void FrameUpdateImpl()
	{
		UpdateTargetPositions();
		if (!base.Data.DoNotKill)
		{
			if (AreSufficientTargetsDead())
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.VisualSummaryState = true;
				base.Result = EObjectiveResult.Fulfilled;
				Complete();
			}
		}
		else if (IsAnyTargetDead())
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.VisualSummaryState = false;
			base.Result = EObjectiveResult.Failed;
			Complete();
		}
	}

	protected override bool IsEntityToBeRegistered(GameEntity _entity, IMissionEntity _missionEntity)
	{
		bool flag = _entity is EnemyCharacter;
		if (!flag)
		{
		}
		return flag;
	}

	protected override void OnValueChanged()
	{
		base.OnValueChanged();
		UpdateDescription();
	}

	private bool AreSufficientTargetsDead()
	{
		int num = 0;
		for (int i = 0; i < Targets.Count; i++)
		{
			EnemyCharacter enemyCharacter = Targets[i].EntityID.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter == null || enemyCharacter.GetIsDead())
			{
				num++;
			}
		}
		int num2 = ((base.Data.Minimum != 0) ? base.Data.Minimum : Targets.Count);
		return num >= num2;
	}

	private bool IsAnyTargetDead()
	{
		for (int i = 0; i < Targets.Count; i++)
		{
			EntityId entityID = Targets[i].EntityID;
			if (entityID.IsValid)
			{
				EnemyCharacter enemyCharacter = entityID.TryGetEntity<EnemyCharacter>();
				if (enemyCharacter == null)
				{
					return true;
				}
				if (enemyCharacter.GetIsDead())
				{
					return true;
				}
			}
		}
		return false;
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		int num = 0;
		if (ObjectiveData.MapIcon != 0)
		{
			for (int i = 0; i < Targets.Count; i++)
			{
				if (!Targets[i].EntityID.IsValid)
				{
					num++;
					continue;
				}
				GameEntity gameEntity = Targets[i].GameEntity;
				if (gameEntity == null)
				{
					num++;
					continue;
				}
				EnemyCharacter enemyCharacter = gameEntity.As<EnemyCharacter>();
				if (enemyCharacter == null || enemyCharacter.GetIsDead())
				{
					num++;
				}
				else if (!enemyCharacter.GetIsDisguised())
				{
					TargetPositions.Add(gameEntity.Position);
				}
			}
		}
		base.Value = Math.Min(num, (base.Data.Minimum != 0) ? base.Data.Minimum : Targets.Count);
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper["count"] = ((base.Data.Minimum == 0) ? base.Data.Targets.Count : base.Data.Minimum);
		return stringMapper;
	}
}
