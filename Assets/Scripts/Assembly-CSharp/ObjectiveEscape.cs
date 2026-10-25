using System;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveEscape : ObjectiveTarget<ObjectiveEscapeData>, IMonitoredHidingBehaviour
{
	private float m_curSqrDistance;

	private Vector3 m_escapeFromPosition;

	private bool m_isInside;

	private float m_sqrRange;

	private Vector3 m_fallbackPosition;

	private MissionArea m_escapeArea;

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return false;
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Undefined;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.InProgress;
		}
	}

	private float CurSqrDistance
	{
		get
		{
			return m_curSqrDistance;
		}
		set
		{
			if (!(Math.Abs(m_curSqrDistance - value) < float.Epsilon))
			{
				m_curSqrDistance = value;
				UpdateDescription();
			}
		}
	}

	public bool PreventFailingOnIncrease
	{
		get
		{
			return true;
		}
	}

	private ObjectiveEscape()
	{
	}

	public override void FrameUpdateImpl()
	{
		GameObject player = LogicMain.Player;
		if (!(player == null))
		{
			CurSqrDistance = (player.transform.position - m_escapeFromPosition).sqrMagnitude;
			m_isInside = CurSqrDistance <= m_sqrRange;
			if (m_isInside)
			{
				base.VisualTrackerState = EProgressState.InProgress;
				base.VisualSummaryState = false;
				base.Result = Mission.EObjectiveResult.Undefined;
			}
			else if (!base.Data.HideOutside || PlayerHelper.IsHiding)
			{
				base.VisualTrackerState = EProgressState.InProgress;
				base.VisualSummaryState = true;
				base.Result = Mission.EObjectiveResult.Fulfilled;
			}
		}
	}

	protected override void Complete()
	{
		if (VisualTrackerState == EProgressState.InProgress)
		{
			base.VisualTrackerState = EProgressState.Fulfilled;
		}
		base.Complete();
	}

	private void ComputeEscapeFromPosition()
	{
		if (Targets.Count <= 0)
		{
			m_escapeFromPosition = m_fallbackPosition;
			return;
		}
		IMissionEntity lastReleasedUnit = GetLastReleasedUnit();
		if (lastReleasedUnit.LastKnownPosition.HasValue)
		{
			m_escapeFromPosition = lastReleasedUnit.LastKnownPosition.Value;
			return;
		}
		EntityId entityID = lastReleasedUnit.EntityID;
		if (entityID.IsValid)
		{
			m_escapeFromPosition = entityID.Entity.As<GameEntity>().Position;
		}
		else
		{
			m_escapeFromPosition = m_fallbackPosition;
		}
	}

	private IMissionEntity GetLastReleasedUnit()
	{
		IMissionEntity result = null;
		double num = 0.0;
		foreach (IMissionEntity target in Targets)
		{
			IMissionEntity missionEntity = target.As<IMissionEntity>();
			if (missionEntity != null && (double)missionEntity.ReleaseTime >= num)
			{
				num = missionEntity.ReleaseTime;
				result = missionEntity;
			}
		}
		return result;
	}

	public override void OnDeactivate()
	{
		if (base.Data.HideOutside)
		{
			UbiCore.Utils.Singleton<MissionManager>.Instance.ObjectivesRelyingOnHidingSpotsCount--;
		}
		RemoveArea();
		base.OnDeactivate();
	}

	private void RemoveArea()
	{
		UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.DestroyPerimeter(m_escapeArea);
		m_escapeArea = null;
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_sqrRange = base.Data.Distance * base.Data.Distance;
		m_curSqrDistance = 0f;
		m_fallbackPosition = default(Vector3);
		m_isInside = false;
		if (Targets.Count == 0)
		{
			m_fallbackPosition = LogicMain.Player.transform.position;
		}
		if (base.Data.HideOutside)
		{
			UbiCore.Utils.Singleton<MissionManager>.Instance.ObjectivesRelyingOnHidingSpotsCount++;
		}
		ComputeEscapeFromPosition();
		m_escapeArea = UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.CreatePerimeter(base.gameObject.transform, m_escapeFromPosition, base.Data.Distance, base.Data.ActivateAtId, base.Data.RemoveAtId);
		TargetPositions.ClearIfNotEmpty();
		TargetPositions.Add(m_escapeFromPosition);
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		float num = Mathf.Sqrt(CurSqrDistance);
		stringMapper.Add("current_distance", num);
		stringMapper.Add("target_distance", base.Data.Distance);
		stringMapper.Add("remaining_distance", Mathf.Max(0f, base.Data.Distance - num));
		stringMapper.Add("Current", num);
		stringMapper.Add("Threshold", base.Data.Distance);
		return stringMapper;
	}

	public override string ToString()
	{
		return string.Format("Escape: {0}m", base.Data.Distance);
	}

	public bool MonitorsLimitType(ELimitType _limitType)
	{
		return m_isInside;
	}

	public bool WillFailOnIncrease(int _increase)
	{
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7765, InfoEvent.ETextAnnotationMode.MessageOpen, -1);
		return false;
	}
}
