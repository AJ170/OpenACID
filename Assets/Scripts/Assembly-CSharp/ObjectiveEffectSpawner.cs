using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveEffectSpawner : ObjectiveBase<ObjectiveEffectSpawnerData>
{
	[Serializable]
	public class StaticDelayData
	{
		public int Delay;

		public AnimusObjectFade Effect;

		public EEffectSpawnerSoundType SoundType;

		private EEffectSpawnerSoundType m_soundType;
	}

	public List<StaticDelayData> StaticEffects = new List<StaticDelayData>();

	public List<AnimusObjectFade> HideObjects = new List<AnimusObjectFade>();

	private static bool s_isInitialized;

	private static BBDictionary<int, AnimusObjectFade> s_effects = new BBDictionary<int, AnimusObjectFade>();

	private int m_staticSpawnIndex;

	private float m_staticSpawnTimeDeltaSec;

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
			return true;
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Undefined;
		}
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		if (!s_isInitialized)
		{
			InitStatic();
		}
		StaticEffects.Clear();
		HideObjects.Clear();
		foreach (ObjectiveEffectSpawnerData.ObjectDelay uniqueId in base.Data.UniqueIds)
		{
			StaticEffects.Add(new StaticDelayData
			{
				Delay = uniqueId.DelayMs,
				SoundType = uniqueId.SoundType,
				Effect = s_effects[uniqueId.ID]
			});
		}
		foreach (int hideObjectId in base.Data.HideObjectIds)
		{
			if (s_effects.ContainsKey(hideObjectId))
			{
				HideObjects.Add(s_effects[hideObjectId]);
			}
		}
	}

	private static void InitStatic()
	{
		s_isInitialized = true;
		UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged += OnMissionStateChanged;
		AnimusObjectFade[] array = UnityEngine.Object.FindObjectsOfType<AnimusObjectFade>();
		for (int i = 0; i < array.Length; i++)
		{
			s_effects[array[i].UniqueID] = array[i];
		}
	}

	private static void OnMissionStateChanged(EMissionState _state)
	{
		if (s_isInitialized && _state == EMissionState.Unloading)
		{
			s_effects.Clear();
			UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged -= OnMissionStateChanged;
			s_isInitialized = false;
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_staticSpawnIndex = 0;
		m_staticSpawnTimeDeltaSec = 0f;
		foreach (StaticDelayData staticEffect in StaticEffects)
		{
			if (staticEffect.Effect != null)
			{
				staticEffect.Effect.ToStart();
			}
		}
		foreach (AnimusObjectFade hideObject in HideObjects)
		{
			if (hideObject != null)
			{
				hideObject.ToStart();
			}
		}
	}

	public override void FrameUpdateImpl()
	{
		if (StaticEffects.Count == 0)
		{
			base.FrameUpdateImpl();
			return;
		}
		m_staticSpawnTimeDeltaSec += LogicMain.FrameTime;
		StaticDelayData staticDelayData = StaticEffects[m_staticSpawnIndex];
		if (StaticEffects.Count > m_staticSpawnIndex && m_staticSpawnTimeDeltaSec >= (float)staticDelayData.Delay / 1000f)
		{
			m_staticSpawnTimeDeltaSec -= (float)staticDelayData.Delay / 1000f;
			staticDelayData.Effect.FadeIn();
			PlaySound(staticDelayData);
			m_staticSpawnIndex++;
		}
		if (m_staticSpawnIndex >= StaticEffects.Count)
		{
			base.Result = Mission.EObjectiveResult.Fulfilled;
			base.VisualTrackerState = EProgressState.Fulfilled;
			Complete();
		}
		base.FrameUpdateImpl();
	}

	private static void PlaySound(StaticDelayData _staticDelayData)
	{
		switch (_staticDelayData.SoundType)
		{
		case EEffectSpawnerSoundType.Object_EEffectSpawnerSoundType:
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_animus_tutorial_item_appear);
			break;
		case EEffectSpawnerSoundType.Ground_EEffectSpawnerSoundType:
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_animus_tutorial_ground_appear);
			break;
		}
	}
}
