using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;
using UbiCore.Utils;

public class ObjectiveSelectTarget : ObjectiveTarget<ObjectiveSelectTargetData>
{
	private const float c_ForceSelectionRange = 15f;

	private bool m_hadTargetSelected;

	private EntityId TargetToSelect
	{
		get
		{
			if (base.RelevantEntities.Count >= 1)
			{
				return base.RelevantEntities[base.RelevantEntities.Count - 1];
			}
			return EntityId.InvalidId;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return base.Data.FailOnUnselect;
		}
	}

	protected override EObjectiveResult InitialResult
	{
		get
		{
			return base.Data.FailOnUnselect ? EObjectiveResult.Fulfilled : EObjectiveResult.Undefined;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return base.Data.FailOnUnselect ? EProgressState.InProgress : EProgressState.NotStarted;
		}
	}

	private ObjectiveSelectTarget()
	{
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_hadTargetSelected = false;
	}

	public override void FrameUpdateImpl()
	{
		UpdateTargetPositions();
		if (!IsActive)
		{
			return;
		}
		bool flag = UbiCore.Utils.Singleton<SelectionManager>.Instance.SelectedEntityId == TargetToSelect;
		if (base.Data.ForceSelection)
		{
			EnemyCharacter enemyCharacter = TargetToSelect.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter != null)
			{
				if (enemyCharacter.GetDeadOrAboutToDie() || !enemyCharacter.IsAttached)
				{
					return;
				}
				float magnitude = (enemyCharacter.Position - LogicMain.PlayerEntity.Position).magnitude;
				if (magnitude < 15f)
				{
					UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = enemyCharacter;
					KillFromHideout activeKillFromHideout = PlayerHelper.Entity.GetActiveKillFromHideout();
					if (activeKillFromHideout != null)
					{
						activeKillFromHideout.SelectedEnemy = enemyCharacter;
						activeKillFromHideout.AutoSelectionEnabled = false;
					}
				}
			}
			else if (UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection is EnemyCharacter)
			{
				UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = null;
			}
		}
		else if (flag)
		{
			m_hadTargetSelected = true;
			if (!base.Data.FailOnUnselect)
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.VisualSummaryState = true;
				base.Result = EObjectiveResult.Fulfilled;
				Complete();
			}
		}
		else if (base.Data.FailOnUnselect && m_hadTargetSelected)
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

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon != 0 && TargetToSelect.IsValid)
		{
			GameEntity gameEntity = TargetToSelect.GameEntity;
			EnemyCharacter enemyCharacter = gameEntity.As<EnemyCharacter>();
			if (enemyCharacter != null && !enemyCharacter.GetIsDead())
			{
				TargetPositions.Add(gameEntity.Position);
			}
			else
			{
				base.Result = EObjectiveResult.Failed;
				base.VisualTrackerState = EProgressState.Failed;
				base.VisualSummaryState = true;
				Complete();
			}
		}
		base.Value = base.Data.Targets.Count - TargetPositions.Count;
	}
}
