using System;
using System.Globalization;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveEscort : ObjectiveTarget<ObjectiveEscortData>, IObjectiveTimer
{
	private enum EFailReason
	{
		Unknown = 0,
		TargetDead = 1
	}

	private class EscortEntity
	{
		public MissionNpcSpawn MissionEntity { get; set; }

		public Assets.Scripts.Mission.Logic.Entities.Patrol Patrol { get; set; }

		public IPositionProvider IconPosition { get; set; }
	}

	private const float EscortProgressFullfilledThreshold = 0.99f;

	private EscortEntity m_escortEntity;

	private float m_escortProgress;

	private float m_pathLength;

	private float m_traveledLength;

	private bool m_escorteeThreatened;

	private EFailReason m_failReason;

	public bool HasTimer
	{
		get
		{
			return ThreateningRunning;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.InProgress;
		}
	}

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

	private bool ThreateningRunning
	{
		get
		{
			return m_escortEntity.Patrol.VictimComponent.ThreateningOrKillRunning;
		}
	}

	private bool ThreateningAndKillFinished
	{
		get
		{
			return m_escortEntity.Patrol.VictimComponent.ThreateningAndKillFinished;
		}
	}

	private float EscortProgress
	{
		get
		{
			return m_escortProgress;
		}
		set
		{
			if (!(Math.Abs(m_escortProgress - value) < float.Epsilon))
			{
				m_escortProgress = value;
				UpdateDescription();
			}
		}
	}

	public bool EscorteeThreatened
	{
		get
		{
			return m_escorteeThreatened;
		}
		set
		{
			if (m_escorteeThreatened != value)
			{
				m_escorteeThreatened = value;
				AudioManager.Instance.EventManager.OnEscorteeThreatened(m_escorteeThreatened);
			}
		}
	}

	private ObjectiveEscort()
	{
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		if (m_failReason != EFailReason.TargetDead)
		{
			return;
		}
		int textId = 7740;
		StringMapper stringMapper = null;
		if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.Data.ObjectiveType == EObjectiveType.Story_EObjectiveType)
		{
			EntityId entityID = m_escortEntity.MissionEntity.EntityID;
			CrowdCharacter crowdCharacter = entityID.TryGetEntity<CrowdCharacter>();
			if (crowdCharacter != null)
			{
				string localizedName = crowdCharacter.GetLocalizedName(CharacterHelper.ECharacterNameType.Plain);
				if (!string.IsNullOrEmpty(localizedName))
				{
					textId = 8425;
					StringMapper stringMapper2 = new StringMapper();
					stringMapper2.Add("objective_taget", localizedName);
					stringMapper = stringMapper2;
				}
			}
		}
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, textId, InfoEvent.ETextAnnotationMode.MessageOpen, -1, stringMapper, true);
	}

	public override void FrameUpdateImpl()
	{
		if (!IsActive || LogicMain.PlayerEntity == null)
		{
			return;
		}
		if (ThreateningAndKillFinished)
		{
			EscorteeThreatened = false;
			base.VisualTrackerState = EProgressState.Failed;
			base.VisualSummaryState = false;
			m_failReason = EFailReason.TargetDead;
			base.Result = Mission.EObjectiveResult.Failed;
			UpdateDescription();
		}
		else if (!ThreateningRunning)
		{
			EscortProgress = GetEscortProgress();
			EscorteeThreatened = false;
			if (EscortProgress > 0.99f)
			{
				EscortProgress = 1f;
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.VisualSummaryState = true;
				base.Result = Mission.EObjectiveResult.Fulfilled;
				UpdateDescription();
			}
		}
		else
		{
			EscorteeThreatened = true;
		}
		if (Result != 0 && m_escortEntity.Patrol.EscortComponent.IsActive)
		{
			m_escortEntity.Patrol.EscortComponent.IsActive = false;
		}
		UpdateTargetPositions();
	}

	public int GetTime()
	{
		return (m_escortEntity != null) ? ((int)Mathf.Ceil(m_escortEntity.Patrol.EscortComponent.ThreatenedTimeLeft)) : base.Data.MaxSurviveDurationSec;
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_escortProgress = 0f;
		m_escorteeThreatened = false;
		m_pathLength = 0f;
		m_traveledLength = 0f;
		m_failReason = EFailReason.Unknown;
		IMissionEntity missionEntity = Targets[0];
		m_escortEntity = new EscortEntity
		{
			MissionEntity = (MissionNpcSpawn)missionEntity
		};
		MissionNpcFormation component = m_escortEntity.MissionEntity.gameObject.GetComponent<MissionNpcFormation>();
		m_escortEntity.Patrol = component.EntityID.Entity.As<Assets.Scripts.Mission.Logic.Entities.Patrol>();
		MissionNpcSpawn component2 = m_escortEntity.MissionEntity.gameObject.GetComponent<MissionNpcSpawn>();
		m_escortEntity.IconPosition = component2.GameEntity;
		Escort escort = m_escortEntity.Patrol.EscortComponent ?? m_escortEntity.Patrol.AddEscorteeComponent();
		escort.Init(m_escortEntity.MissionEntity.EntityID, base.Data.EscortAreaRadius, base.Data.MaxSurviveDurationSec, base.Data.ResumePatrolDelaySec, base.Data.EnemyCountCap);
	}

	public override void OnDeactivate()
	{
		Targets.ClearIfNotEmpty();
		base.OnDeactivate();
	}

	public override string ToString()
	{
		return string.Format("{0}", base.Data.EscortAreaRadius);
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon != 0)
		{
			Vector3 position = m_escortEntity.IconPosition.Position;
			TargetPositions.Add(position);
		}
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		int time = GetTime();
		string value = ((time <= 0) ? string.Empty : time.ToString(CultureInfo.InvariantCulture));
		stringMapper.Add("timer", value);
		stringMapper.Add("progress", EscortProgress);
		stringMapper.Add("progress_traveled", m_traveledLength);
		stringMapper.Add("path_length", m_pathLength);
		stringMapper.Add("Seconds", value);
		stringMapper.Add("Current", EscortProgress);
		return stringMapper;
	}

	private float GetEscortProgress()
	{
		if (m_escortEntity == null)
		{
			return 0f;
		}
		Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = m_escortEntity.Patrol.PatrolComponent;
		if (patrolComponent.WaypointCount <= 1)
		{
			return 0f;
		}
		if (patrolComponent.IsTriggerReloadRoute)
		{
			return 0f;
		}
		m_pathLength = patrolComponent.ComputePathLength();
		m_traveledLength = patrolComponent.ComputeTraveledLength();
		return m_traveledLength / m_pathLength;
	}
}
