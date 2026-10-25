using AcierProtocol;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.Motion.WithoutAgent;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UnityEngine;

public class ObjectiveStayInRangeMobile : ObjectiveStayInRange<ObjectiveStayInRangeMobileData>
{
	public MissionTarget ToTarget;

	public MissionMonoBehaviour Target;

	private IMissionSpawnEntity m_entity;

	private bool m_lineOfSightGotEstablishedOnce;

	public override int FailMessageId
	{
		get
		{
			return 7739;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return true;
		}
	}

	public bool WasOutOfRangeToLong
	{
		get
		{
			return base.GetBackInRangeTimer > (float)base.Data.TimerSeconds;
		}
	}

	public bool IsTargetDead
	{
		get
		{
			if (m_entity == null || m_entity.EntityID == EntityId.InvalidId)
			{
				return true;
			}
			Character character = m_entity.EntityID.TryGetEntity<Character>();
			if (character == null || character.GetIsDead())
			{
				return true;
			}
			return false;
		}
	}

	private bool IsTargetAtFinish { get; set; }

	protected override float TargetRange
	{
		get
		{
			return base.Data.Range;
		}
	}

	protected override Vector3 TargetPosition
	{
		get
		{
			return (m_entity == null || !m_entity.EntityID.IsValid) ? Vector3.zero : m_entity.GameEntity.Position;
		}
	}

	private ObjectiveStayInRangeMobile()
	{
	}

	public override void OnActivate()
	{
		base.OnActivate();
		if (base.Data.IsVisibleInEagleVision)
		{
			LogicMain.PlayerEntity.Vision.AddEntity(m_entity.EntityID);
		}
		Helper.DemandNavAgentSurrogateFrameUpdate(m_entity.EntityID, true);
	}

	public override void OnDeactivate()
	{
		Helper.DemandNavAgentSurrogateFrameUpdate(m_entity.EntityID, false);
		if (base.Data.IsVisibleInEagleVision)
		{
			LogicMain.PlayerEntity.Vision.RemoveEntity(m_entity.EntityID);
		}
		base.OnDeactivate();
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		IMissionBase missionObject = GetMissionObject(base.Data.TargetID);
		m_entity = missionObject as IMissionSpawnEntity;
		if (m_entity != null)
		{
			Target = (MissionMonoBehaviour)missionObject;
		}
		ToTarget = (MissionTarget)GetMissionObject(base.Data.ToTargetID);
	}

	protected override void ShowFailMessage()
	{
		if (!IsTargetDead && WasOutOfRangeToLong)
		{
			base.ShowFailMessage();
		}
	}

	public override void FrameUpdateImpl()
	{
		if (LogicMain.Player == null || Target == null)
		{
			return;
		}
		base.FrameUpdateImpl();
		if (VisualTrackerState == EProgressState.NotStarted)
		{
			return;
		}
		if (IsTargetDead)
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.Result = Mission.EObjectiveResult.Failed;
			base.VisualSummaryState = false;
			Complete();
			return;
		}
		if (ToTarget != null)
		{
			UpdateTargetReachData();
			if (IsTargetAtFinish)
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.Result = Mission.EObjectiveResult.Fulfilled;
				Complete();
				return;
			}
		}
		if (base.IsInside)
		{
			base.VisualTrackerState = EProgressState.InRange;
			return;
		}
		Invalidate();
		if (WasOutOfRangeToLong)
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.VisualSummaryState = false;
			base.Result = Mission.EObjectiveResult.Failed;
			Complete();
		}
	}

	protected override bool GetComputeIsInRange()
	{
		if (!base.GetComputeIsInRange())
		{
			return false;
		}
		if (!base.Data.MaintainLineOfSight)
		{
			return true;
		}
		bool flag = LogicMain.EntityTracking.IsCharacterInLineOfSight(m_entity.EntityID, false, 0f);
		if (!flag)
		{
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TailingKeepLineOfSight);
		}
		else
		{
			m_lineOfSightGotEstablishedOnce = true;
		}
		return flag || !m_lineOfSightGotEstablishedOnce;
	}

	private void UpdateTargetReachData()
	{
		if (!(ToTarget == null))
		{
			float sqrMagnitude = (ToTarget.transform.position - TargetPosition).sqrMagnitude;
			float num = ToTarget.Data.Range * ToTarget.Data.Range;
			IsTargetAtFinish = sqrMagnitude <= num;
		}
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("target_distance_maximum", base.Data.Range);
		stringMapper.Add("threshold", base.Data.Range);
		return stringMapper;
	}

	public override string ToString()
	{
		string text = ObjectiveBase<ObjectiveStayInRangeMobileData>.FormatEntity(Target, 0);
		return text + "\n" + base.ToString();
	}
}
