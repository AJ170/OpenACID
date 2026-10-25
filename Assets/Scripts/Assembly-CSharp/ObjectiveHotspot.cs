using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;
using SharedBaseLib.Core;
using UnityEngine;

public class ObjectiveHotspot : ObjectiveBase<ObjectiveHotspotData>, IObjectiveGOTarget
{
	private readonly BBList<GameObject> m_hotspotGOs = new BBList<GameObject>();

	private readonly BBList<GameObject> m_visitedHotspots = new BBList<GameObject>();

	private GameObject m_currentActive;

	private bool m_updateTargetPositions;

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return base.Data.Invert;
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return base.Data.Invert ? Mission.EObjectiveResult.Fulfilled : Mission.EObjectiveResult.Undefined;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return base.Data.Invert ? EProgressState.InProgress : EProgressState.NotStarted;
		}
	}

	public List<GameObject> TargetGameObjects
	{
		get
		{
			return m_hotspotGOs;
		}
	}

	public Type TargetComponentType
	{
		get
		{
			return typeof(HotspotMeta);
		}
	}

	private GameObject CurrentActiveHotSpot
	{
		get
		{
			return m_currentActive;
		}
		set
		{
			if (!(m_currentActive != value))
			{
				return;
			}
			if (base.Data.Invert && value == null)
			{
				base.VisualTrackerState = EProgressState.Failed;
				base.VisualSummaryState = false;
				base.Result = Mission.EObjectiveResult.Failed;
				Complete();
			}
			if (!base.Data.Invert && m_hotspotGOs.Contains(value))
			{
				if (!m_visitedHotspots.Contains(value))
				{
					m_visitedHotspots.Add(value);
				}
				if (m_hotspotGOs.Count == m_visitedHotspots.Count)
				{
					base.VisualTrackerState = EProgressState.Fulfilled;
					base.VisualSummaryState = true;
					base.Result = Mission.EObjectiveResult.Fulfilled;
					Complete();
				}
			}
			UpdateTargetPositions();
			m_currentActive = value;
		}
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		m_hotspotGOs.Clear();
		CheckAndAddGameobjects(GameObject.Find("Benches"));
		CheckAndAddGameobjects(GameObject.Find("Haystacks"));
		CheckAndAddGameobjects(GameObject.Find("Roof Gardens"));
	}

	public override void FrameUpdateImpl()
	{
		if (m_updateTargetPositions)
		{
			UpdateTargetPositions();
		}
		AssassinCharacterStateMachines assassinCharacterStateMachines = LogicMain.PlayerEntity.AssassinCharacterStateMachines;
		if (assassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInObject)
		{
			PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload interactingWithEntityPayload = (PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload)assassinCharacterStateMachines.PlayerMain.CurrentStatePayload;
			CurrentActiveHotSpot = interactingWithEntityPayload.EntityId.GameObjectEntity.GameObject;
		}
		else
		{
			CurrentActiveHotSpot = null;
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_visitedHotspots.ClearIfNotEmpty();
		m_currentActive = null;
		m_updateTargetPositions = false;
		UpdateTargetPositions();
		UpdateDescription();
	}

	private void CheckAndAddGameobjects(GameObject _root)
	{
		if (!_root)
		{
			return;
		}
		HotspotMeta[] componentsInChildren = _root.GetComponentsInChildren<HotspotMeta>();
		if (componentsInChildren == null)
		{
			return;
		}
		foreach (HotspotMeta hotspotMeta in componentsInChildren)
		{
			if (base.Data.HotSpots.Contains(hotspotMeta.UniqueID))
			{
				m_hotspotGOs.Add(hotspotMeta.gameObject);
			}
		}
	}

	private void UpdateTargetPositions()
	{
		m_updateTargetPositions = false;
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon == EMapIconType.Invalid_EMapIconType)
		{
			return;
		}
		for (int i = 0; i < m_hotspotGOs.Count; i++)
		{
			GameObject gameObject = m_hotspotGOs[i];
			if (!m_visitedHotspots.Contains(gameObject))
			{
				EntityWrapperComponent component = gameObject.GetComponent<EntityWrapperComponent>();
				if (component == null)
				{
					m_updateTargetPositions = true;
					TargetPositions.Add(gameObject.transform.position);
				}
				else if (component.Entity != null)
				{
					TargetPositions.Add(component.Entity.Cast<HideoutObject>().InteractionLocators.Center.transform.position);
				}
				else
				{
					m_updateTargetPositions = true;
					TargetPositions.Add(gameObject.transform.position);
				}
			}
		}
	}
}
