using Database;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class PlayAudio : MonoBehaviour
	{
		private IAudioHandle m_handle;

		private bool m_isLoaded;

		private bool m_isLoading;

		private bool m_wasPlayCalled;

		private float m_startDelayTick;

		private float m_stopDelayTick;

		[SerializeField]
		private AudioControllerAsset m_audioAsset;

		[SerializeField]
		private float m_startDelay;

		[SerializeField]
		private float m_stopDelay;

		[SerializeField]
		private bool m_loop;

		public float StartDelay
		{
			get
			{
				return m_startDelay;
			}
			set
			{
				m_startDelay = value;
			}
		}

		public float StopDelay
		{
			get
			{
				return m_stopDelay;
			}
			set
			{
				m_stopDelay = value;
			}
		}

		public bool Loop
		{
			get
			{
				return m_loop;
			}
			set
			{
				m_loop = value;
			}
		}

		public AudioSource Audio { get; private set; }

		public void ReloadAudioHandle(GameID _audioController = null)
		{
			if (m_handle != null)
			{
				OnDisable();
			}
			GameID eController = _audioController ?? ((!(m_audioAsset != null)) ? GameID.Invalid : m_audioAsset.GameId);
			m_handle = AudioManager.Instance.CreateHandle(eController, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
			if (base.enabled)
			{
				OnEnable();
			}
		}

		private void Awake()
		{
			if (m_audioAsset != null)
			{
				m_handle = AudioManager.Instance.CreateHandle(m_audioAsset.GameId, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				Audio = GetComponent<AudioSource>();
			}
		}

		private void OnEnable()
		{
			Load();
		}

		private void OnDisable()
		{
			m_wasPlayCalled = false;
			m_startDelayTick = 0f;
			m_stopDelayTick = 0f;
			m_isLoading = false;
			OnDestroy();
		}

		protected bool AllowUpdate()
		{
			return m_handle != null;
		}

		protected bool AllowStartTick()
		{
			return !m_wasPlayCalled;
		}

		protected bool AllowStopTick()
		{
			return m_loop && m_stopDelay > 0f;
		}

		private void Update()
		{
			if (!AllowUpdate())
			{
				return;
			}
			if (AllowStartTick())
			{
				m_startDelayTick += Time.deltaTime;
				if (m_startDelayTick >= m_startDelay)
				{
					Play();
				}
			}
			else if (AllowStopTick())
			{
				m_stopDelayTick += Time.deltaTime;
				if (m_stopDelayTick >= m_stopDelay)
				{
					m_handle.Stop();
				}
			}
		}

		private void Play()
		{
			Load();
			m_handle.Play(m_loop, GetComponent<AudioSource>());
			m_wasPlayCalled = true;
		}

		private void Load()
		{
			if (!m_isLoaded && !m_isLoading)
			{
				m_isLoading = true;
				m_handle.LoadStrategyEvent.OnLoaded.Add(OnLoaded);
				m_handle.Load();
			}
		}

		private void Unload()
		{
			m_isLoaded = false;
			m_isLoading = false;
			m_handle.Stop();
			m_handle.LoadStrategyEvent.OnLoaded.Remove(OnLoaded);
			m_handle.Unload();
		}

		private void OnLoaded(params object[] _objects)
		{
			m_isLoaded = true;
			m_isLoading = false;
		}

		private void OnDestroy()
		{
			if (m_handle == null)
			{
				return;
			}
			if (m_handle.IsPlaying)
			{
				if (m_loop)
				{
					Unload();
					return;
				}
				m_handle.ChangeStoppedFinishedStrategy(EStoppedFinishedStrategy.UnloadAll);
				m_handle.DetachChannelFromParent();
			}
			else
			{
				Unload();
			}
		}
	}
}
