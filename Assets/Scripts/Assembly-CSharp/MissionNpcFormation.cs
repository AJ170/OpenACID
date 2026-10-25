using System.Collections;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Ingame;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class MissionNpcFormation : MissionFormationBase<MissionNpcFormationData>
{
	private EMapIconType m_mapIconType;

	private IAudioHandle m_soundHandle;

	private Assets.Scripts.Mission.Logic.Entities.Patrol m_patrolEntity;

	private AudioSource m_audioSource;

	public override EntityId EntityID { get; protected set; }

	public override EMapIconType MapIcon
	{
		get
		{
			return m_mapIconType;
		}
	}

	public void SetMapIconType(EMapIconType _mapIconType)
	{
		m_mapIconType = _mapIconType;
	}

	public override void OnPreActivate()
	{
		GameObject gameObject = new GameObject("Patrol");
		gameObject.transform.parent = base.transform;
		base.OnPreActivate();
		EntityWrapperComponent wrapper = gameObject.AddComponent<EntityWrapperComponent>();
		PatrolData.IPatrolDefinition definition = UbiCore.Utils.Singleton<PatrolData>.Instance.GetDefinition(base.Data.Type);
		m_patrolEntity = Assets.Scripts.Mission.Logic.Helpers.CreateRegisterAttachEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>(definition, wrapper, base.transform);
		EntityID = m_patrolEntity.Id;
		StartCoroutine(PreparePatrolSound(definition.AudioSourcePrefab, base.Data.Type));
		m_mapIconType = base.Data.MapIcon;
		Assets.Scripts.Mission.Helpers.SetTransformAndActivate(gameObject, base.transform.position, base.transform.rotation);
		if (m_patrolEntity.PatrolDefinition.Definition.InteractionReason == EInteractionReason.HidingInPatrol || m_patrolEntity.PatrolDefinition.Definition.InteractionReason == EInteractionReason.FreeCapturedNpcs)
		{
			IconMaterial material = IconMaterial.InteractHaystack;
			switch (base.Data.Type)
			{
			case EPatrolType.CourtesanHidingSpot:
				material = IconMaterial.InteractCourtesans;
				break;
			case EPatrolType.CapturedNpcs:
				material = IconMaterial.InteractFreeCaptured;
				break;
			}
			AddApproachIcon(m_patrolEntity, material);
		}
		int num = base.Waypoints.Length;
		for (int i = 0; i < num; i++)
		{
			MissionWaypoint missionWaypoint = base.Waypoints[i];
			missionWaypoint.MissionNpcFormation = this;
		}
	}

	private IEnumerator PreparePatrolSound(UnityResource _resouce, EPatrolType _type)
	{
		IEnumerator ite = _resouce.PrepareAsync();
		while (ite.MoveNext())
		{
			yield return 0;
		}
		if (!_resouce.WrappedObjectIsNull())
		{
			GameObject prefab = _resouce.GetObject<GameObject>();
			GameObject instance = UnityEngine.Object.Instantiate(prefab);
			instance.transform.parent = m_patrolEntity.GameObject.transform;
			instance.transform.localPosition = Vector3.zero;
			LoadSound(_type, instance);
		}
	}

	private void LoadSound(EPatrolType _type, GameObject _instance)
	{
		m_audioSource = _instance.GetComponent<AudioSource>();
		GameID gameID = null;
		switch (_type)
		{
		case EPatrolType.CevilianHidingSpot:
			gameID = UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_monks_singing;
			break;
		case EPatrolType.CourtesanHidingSpot:
			gameID = UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_courtesans_laughter;
			break;
		}
		if (gameID != null)
		{
			m_soundHandle = AudioManager.Instance.CreateHandle(gameID, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
			StartCoroutine(SoundDistanceWatchdog());
		}
	}

	private IEnumerator SoundDistanceWatchdog()
	{
		WaitForSeconds wait = new WaitForSeconds(0.1f);
		float lastDistance = float.MaxValue;
		float audioSourceDistance = m_audioSource.maxDistance * m_audioSource.maxDistance;
		while (m_soundHandle != null)
		{
			yield return wait;
			if (!(LogicMain.Player == null))
			{
				float currentDistance = (LogicMain.Player.transform.position - base.transform.position).sqrMagnitude;
				if (currentDistance < lastDistance && currentDistance <= audioSourceDistance)
				{
					PlaySound();
				}
				if (currentDistance > lastDistance && currentDistance > audioSourceDistance)
				{
					StopSound();
				}
				lastDistance = currentDistance;
			}
		}
	}

	private void PlaySound()
	{
		if (m_soundHandle != null && !m_soundHandle.IsPlaying)
		{
			m_soundHandle.Load();
			m_soundHandle.OnFinishedEvent.Add(PlaySoundDelayed);
			m_soundHandle.Play(false, m_audioSource);
		}
	}

	private void PlayLoopingSound()
	{
		if (m_soundHandle != null && !m_soundHandle.IsPlaying)
		{
			m_soundHandle.Load();
			m_soundHandle.Play(true, m_audioSource);
		}
	}

	private void PlaySoundDelayed(object[] _params)
	{
		if (m_soundHandle != null && m_soundHandle.OnFinishedEvent != null)
		{
			m_soundHandle.OnFinishedEvent.Remove(PlaySoundDelayed);
			if (base.Data.Type == EPatrolType.CevilianHidingSpot)
			{
				PlayLoopingSound();
			}
			else
			{
				StartCoroutine(PlaySoundDelayed());
			}
		}
	}

	private IEnumerator PlaySoundDelayed()
	{
		yield return new WaitForSeconds(Random.Range(1, 3));
		PlaySound();
	}

	private void StopSound()
	{
		if (m_soundHandle != null && m_soundHandle.IsPlaying)
		{
			m_soundHandle.OnFinishedEvent.Remove(PlaySoundDelayed);
			m_soundHandle.Stop();
		}
	}

	public override void OnActivate()
	{
		Assets.Scripts.Mission.Logic.Entities.Patrol entity = EntityID.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
		if (base.Data.Type == EPatrolType.CapturedNpcs && base.Data.Radius > 0f)
		{
			MissionNpcSpawn component = base.gameObject.GetComponent<MissionNpcSpawn>();
			GameEntity gameEntity = component.GameEntity;
			Captive captive = entity.CaptiveComponent ?? entity.AddCaptiveComponent();
			captive.Init(gameEntity.Id, base.Data.Radius, 1);
			IsActive = true;
		}
		entity.OnActivate();
	}

	public override void OnDeactivate()
	{
		if (EntityID.IsValid)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = EntityID.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			LogicMain.EntityManager.UnregisterEntity(entity);
			UnloadSounds();
		}
		base.OnDeactivate();
	}

	private void OnEntityUnregistered(IEntity _sender)
	{
		StopSound();
	}

	private void OnEntityRegistered(IEntity _sender)
	{
		PlaySound();
	}

	private void UnloadSounds()
	{
		if (m_soundHandle != null)
		{
			m_soundHandle.Unload();
			m_soundHandle = null;
		}
	}

	public override void OnWaypointLoaded(MissionWaypointData _waypoint)
	{
		if (EntityID.IsValid)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = EntityID.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			BBList<Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint> currentRoute = entity.PatrolComponent.CurrentRoute;
			int num = currentRoute.FindIndex((Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint wp) => wp.ID == _waypoint.ID);
			if (num == -1)
			{
				entity.PatrolComponent.TriggerReloadMainRoute(_waypoint.ActivationIterationType, _waypoint.ActivationIterationStartType);
			}
		}
	}

	public override void OnWaypointUnload(MissionWaypointData _waypoint)
	{
		if (EntityID.IsValid)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = EntityID.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			entity.PatrolComponent.TriggerReloadMainRoute();
		}
	}

	private static void AddApproachIcon(Assets.Scripts.Mission.Logic.Entities.Patrol _patrolEntity, IconMaterial _material)
	{
		ApproachEntityIcon approachEntityIcon = new ApproachEntityIcon();
		_patrolEntity.HeadIcon.IconAttachment = approachEntityIcon;
		approachEntityIcon.Visible = true;
		approachEntityIcon.Offset3D = Assets.Scripts.UI.Ingame.Constants.PatrolIconOffset;
		approachEntityIcon.SetIcon(_material, 5);
		approachEntityIcon.SetInteractable(true, (IEntityIcon _patrolIcon) => PlayerHelper.AssassinInteraction.TapEntity(_patrolIcon.EntityId));
	}
}
