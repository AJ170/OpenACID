using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveTriggerAreaManager : ObjectiveBase<ObjectiveTriggerAreaManagerData>
{
	[Serializable]
	public class TriggerAreaData
	{
		private const double c_BlendDelay = 0.20000000298023224;

		public AnimusObjectFade Target;

		public EEffectSpawnerSoundType SoundTypeIn = EEffectSpawnerSoundType.Ground_EEffectSpawnerSoundType;

		public EEffectSpawnerSoundType SoundTypeOut = EEffectSpawnerSoundType.GroundRemove_EEffectSpawnerSoundType;

		private float m_lastTimeUsed;

		private bool m_isVisible;

		public Bounds MeshBoundsInner { get; set; }

		public Bounds MeshBoundsOuter { get; set; }

		public bool IsVisible
		{
			get
			{
				return m_isVisible;
			}
			set
			{
				if (m_isVisible != value)
				{
					m_isVisible = value;
					InvokeVisibilityChanged(this);
				}
			}
		}

		public event Action<TriggerAreaData> VisibilityChanged;

		public void Peek()
		{
			float time = UbiCore.Utils.Singleton<Timer>.Instance.time;
			m_lastTimeUsed = Math.Max(time, m_lastTimeUsed);
			IsVisible = (double)m_lastTimeUsed + 0.20000000298023224 >= (double)time;
		}

		private void InvokeVisibilityChanged(TriggerAreaData _isVisible)
		{
			Action<TriggerAreaData> visibilityChanged = this.VisibilityChanged;
			if (visibilityChanged != null)
			{
				visibilityChanged(_isVisible);
			}
		}
	}

	public List<TriggerAreaData> Targets = new List<TriggerAreaData>();

	private static bool s_isInitialized;

	private static BBDictionary<int, AnimusObjectFade> s_effects = new BBDictionary<int, AnimusObjectFade>();

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
		Targets.Clear();
		for (int i = 0; i < base.Data.Targets.Count; i++)
		{
			ObjectiveTriggerAreaManagerData.TriggerAreaSetting triggerAreaSetting = base.Data.Targets[i];
			AnimusObjectFade value;
			if (Assets.Scripts.Engine.Helpers.Verify(s_effects.TryGetValue(triggerAreaSetting.ID, out value), "ObjectiveTriggerAreaManaget.OnPostRead: Effects do not contain {0}!", triggerAreaSetting.ID))
			{
				TriggerAreaData triggerAreaData = new TriggerAreaData();
				triggerAreaData.SoundTypeIn = triggerAreaSetting.SoundTypeIn;
				triggerAreaData.SoundTypeOut = triggerAreaSetting.SoundTypeOut;
				triggerAreaData.Target = value;
				triggerAreaData.MeshBoundsInner = value.gameObject.ComputeMeshBounds();
				TriggerAreaData triggerAreaData2 = triggerAreaData;
				triggerAreaData2.MeshBoundsInner = new Bounds(triggerAreaData2.MeshBoundsInner.center, triggerAreaData2.MeshBoundsInner.extents + Vector3.up * 100f);
				triggerAreaData2.MeshBoundsOuter = new Bounds(triggerAreaData2.MeshBoundsInner.center, triggerAreaData2.MeshBoundsInner.extents + new Vector3(2f, 100f, 2f));
				Targets.Add(triggerAreaData2);
			}
		}
	}

	private void TriggerAreaDataOnVisibilityChanged(TriggerAreaData _triggerAreaData)
	{
		if (_triggerAreaData.IsVisible)
		{
			PlaySound(_triggerAreaData.SoundTypeIn);
			_triggerAreaData.Target.FadeIn();
		}
		else
		{
			PlaySound(_triggerAreaData.SoundTypeOut);
			_triggerAreaData.Target.FadeOut();
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
		for (int i = 0; i < Targets.Count; i++)
		{
			TriggerAreaData triggerAreaData = Targets[i];
			if (triggerAreaData.Target != null)
			{
				triggerAreaData.Target.ToStart();
			}
			triggerAreaData.VisibilityChanged += TriggerAreaDataOnVisibilityChanged;
		}
	}

	public override void OnDeactivate()
	{
		for (int i = 0; i < Targets.Count; i++)
		{
			TriggerAreaData triggerAreaData = Targets[i];
			triggerAreaData.VisibilityChanged -= TriggerAreaDataOnVisibilityChanged;
		}
		base.OnDeactivate();
	}

	public override void FrameUpdateImpl()
	{
		if (Targets.Count == 0)
		{
			base.FrameUpdateImpl();
			return;
		}
		for (int i = 0; i < LogicMain.EntityManager.NpcCharacterCount; i++)
		{
			NpcCharacter npcCharacter = LogicMain.EntityManager.GetNpcCharacter(i);
			Vector3 position = npcCharacter.Position;
			for (int j = 0; j < Targets.Count; j++)
			{
				TriggerAreaData triggerAreaData = Targets[j];
				bool flag = triggerAreaData.MeshBoundsInner.Contains(position);
				bool flag2 = triggerAreaData.MeshBoundsInner.Contains(position);
				if ((flag && !triggerAreaData.IsVisible) || (flag2 && triggerAreaData.IsVisible))
				{
					triggerAreaData.Peek();
				}
			}
		}
		base.FrameUpdateImpl();
	}

	private static void PlaySound(EEffectSpawnerSoundType _soundType)
	{
		switch (_soundType)
		{
		case EEffectSpawnerSoundType.Object_EEffectSpawnerSoundType:
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_animus_tutorial_item_appear);
			break;
		case EEffectSpawnerSoundType.Ground_EEffectSpawnerSoundType:
		case EEffectSpawnerSoundType.GroundRemove_EEffectSpawnerSoundType:
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_animus_tutorial_ground_appear);
			break;
		}
	}
}
