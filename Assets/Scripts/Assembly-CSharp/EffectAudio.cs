using Assets.Scripts.Audio;
using Database;
using SharedBaseLib.Core;
using UbiCore;
using UnityEngine;

public class EffectAudio : EffectBase
{
	public enum EStartType
	{
		OnOrTriggerEvent = 0,
		OffEvent = 1
	}

	public enum EStopType
	{
		OffEvent = 0,
		StopDelay = 1
	}

	private enum EWasStartedType
	{
		NotStarted = 0,
		StartedFromOn = 1,
		StartedFromTrigger = 2,
		StartedFromOff = 3
	}

	private IAudioHandle m_handle;

	private EWasStartedType m_wasStarted;

	private float m_playDelayTick;

	private float m_stopDelayTick;

	private bool m_playAudioCalled;

	[SerializeField]
	private AudioControllerAsset m_audioAsset;

	[SerializeField]
	private EStartType m_startType;

	[SerializeField]
	private float m_playStartDelay;

	[SerializeField]
	private bool m_loop;

	[SerializeField]
	private EStopType m_stopType;

	[SerializeField]
	private float m_playStopDelay;

	private void Awake()
	{
	}

	private void Update()
	{
		if (m_wasStarted != 0)
		{
			if (!m_playAudioCalled)
			{
				UpdateStartDelayTick();
			}
			else
			{
				UpdateStopDelayTick();
			}
		}
	}

	private void UpdateStartDelayTick()
	{
		if (m_startType != 0 && (m_startType != EStartType.OffEvent || m_wasStarted != EWasStartedType.StartedFromOff))
		{
			return;
		}
		m_playDelayTick += Time.deltaTime;
		if (m_playDelayTick >= m_playStartDelay && m_handle != null && !m_handle.IsPlaying)
		{
			PlayResult playResult = Play(GetComponent<AudioSource>());
			if ((bool)playResult)
			{
			}
		}
	}

	private void UpdateStopDelayTick()
	{
		if (m_stopType == EStopType.StopDelay || (m_stopType == EStopType.OffEvent && m_wasStarted == EWasStartedType.StartedFromOff))
		{
			m_stopDelayTick += Time.deltaTime;
			if (m_stopDelayTick >= m_playStopDelay && m_handle != null)
			{
				m_handle.Stop();
				OnFinishedPlaying();
			}
		}
	}

	private bool Load()
	{
		if (m_handle == null)
		{
			GameID gameID = ((!(m_audioAsset != null)) ? GameID.Invalid : m_audioAsset.GameId);
			if (GameID.IsValid(gameID))
			{
				m_handle = AudioManager.Instance.CreateHandle(gameID, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				m_handle.OnFinishedEvent += new SimpleDelegate.CallbackFunction(OnFinishedPlayingCallback);
				m_handle.Load();
				return true;
			}
			return false;
		}
		m_handle.Load();
		return true;
	}

	private void OnFinishedPlayingCallback(params object[] _objects)
	{
		OnFinishedPlaying();
	}

	private void OnFinishedPlaying()
	{
		Reset();
		DeregisterAndDestroy();
	}

	private PlayResult Play(AudioSource _audioSource)
	{
		m_playAudioCalled = true;
		if (m_handle != null && !m_handle.IsPlaying)
		{
			if (m_loop && m_wasStarted != EWasStartedType.StartedFromOn)
			{
				m_loop = false;
			}
			return m_handle.Play(m_loop, _audioSource);
		}
		return PlayResult.Failed;
	}

	private void Reset()
	{
		m_wasStarted = EWasStartedType.NotStarted;
		if (m_handle != null)
		{
			m_handle.Stop();
			m_handle.Unload();
		}
		m_playDelayTick = (m_stopDelayTick = 0f);
		m_playAudioCalled = false;
	}

	public override void On()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		m_wasStarted = EWasStartedType.StartedFromOn;
		Load();
	}

	public override void Off()
	{
		if (m_startType != EStartType.OffEvent)
		{
			if (m_handle == null || !m_handle.IsPlaying || (m_handle.IsPlaying && m_loop))
			{
				if (MathHelper.Approximately(m_playStopDelay, 0f))
				{
					Reset();
					DeregisterAndDestroy();
				}
				else
				{
					m_wasStarted = EWasStartedType.StartedFromOff;
				}
			}
		}
		else
		{
			m_wasStarted = EWasStartedType.StartedFromOff;
		}
	}

	public override void Trigger()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		m_wasStarted = EWasStartedType.StartedFromTrigger;
		Load();
	}

	public override void OnDestroy()
	{
		if (m_handle != null)
		{
			m_handle.OnFinishedEvent -= new SimpleDelegate.CallbackFunction(OnFinishedPlayingCallback);
			if (m_handle.IsPlaying)
			{
				if (m_loop)
				{
					m_handle.Stop();
					m_handle.Unload();
				}
				else
				{
					m_handle.ChangeStoppedFinishedStrategy(EStoppedFinishedStrategy.UnloadAll);
					m_handle.DetachChannelFromParent();
				}
			}
			else
			{
				m_handle.Unload();
			}
		}
		base.OnDestroy();
	}
}
