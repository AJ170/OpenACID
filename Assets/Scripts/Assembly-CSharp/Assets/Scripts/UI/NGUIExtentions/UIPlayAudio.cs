using Assets.Scripts.Audio;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtentions
{
	public class UIPlayAudio : MonoBehaviour
	{
		public enum ETriggerEvent
		{
			Click = 0,
			HoverIn = 1,
			HoverOut = 2,
			Pressed = 3,
			Released = 4,
			Selected = 5,
			Deselected = 6,
			Enabled = 7,
			None = 8
		}

		public enum EPlayType
		{
			StopAndPlay = 0,
			PlayIfNotPlaying = 1
		}

		private IAudioHandle m_handle;

		private bool m_hasBeenPlayed;

		private GameID m_audioController = GameID.Invalid;

		[SerializeField]
		private string m_guidString;

		[SerializeField]
		private AudioControllerAsset m_audioAsset;

		[SerializeField]
		private ETriggerEvent m_triggerEvent;

		[SerializeField]
		private float m_volume = 1f;

		[SerializeField]
		private EPlayType m_playType;

		[SerializeField]
		private bool m_once;

		public GameID AudioController
		{
			get
			{
				if (!GameID.IsValid(m_audioController) && !string.IsNullOrEmpty(m_guidString))
				{
					GameDB safeGameDb = Main.SafeGameDb;
					m_audioController = safeGameDb.FindGameIdFromGuid(m_guidString);
				}
				return m_audioController;
			}
			set
			{
				m_audioController = value;
				m_guidString = value.GuidString;
				if (Application.isPlaying)
				{
					if (m_handle != null)
					{
						m_handle.Stop();
						m_handle.Unload();
						m_handle = null;
					}
					if (GameID.IsValid(AudioController))
					{
						m_handle = AudioManager.Instance.CreateHandle(AudioController, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
						m_handle.UnloadOnPlayFailed = false;
						m_handle.Load();
					}
				}
			}
		}

		public ETriggerEvent TriggerEvent
		{
			get
			{
				return m_triggerEvent;
			}
			set
			{
				m_triggerEvent = value;
			}
		}

		public float Volume
		{
			get
			{
				return m_volume;
			}
			set
			{
				m_volume = value;
				if (m_handle != null)
				{
					m_handle.Volume = m_volume;
				}
			}
		}

		public EPlayType PlayType
		{
			get
			{
				return m_playType;
			}
			set
			{
				m_playType = value;
			}
		}

		public bool Once
		{
			get
			{
				return m_once;
			}
			set
			{
				m_once = value;
			}
		}

		private void Awake()
		{
			if (GameID.IsValid(AudioController))
			{
				m_handle = AudioManager.Instance.CreateHandle(AudioController, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
				m_handle.UnloadOnPlayFailed = false;
				m_handle.Load();
			}
		}

		private void OnEnable()
		{
			if (m_triggerEvent == ETriggerEvent.Enabled && base.gameObject.activeSelf)
			{
				Play();
			}
		}

		public void Play()
		{
			if (m_handle != null && (!m_once || !m_hasBeenPlayed))
			{
				m_hasBeenPlayed = true;
				m_handle.Volume = Volume;
				if (PlayType == EPlayType.StopAndPlay)
				{
					m_handle.Stop();
					m_handle.Play();
				}
				else if (PlayType == EPlayType.PlayIfNotPlaying && !m_handle.IsPlaying)
				{
					m_handle.Play();
				}
			}
		}

		private void OnClick()
		{
			if (TriggerEvent == ETriggerEvent.Click)
			{
				Play();
			}
		}

		private void OnHover(bool _isHover)
		{
			if (TriggerEvent == ETriggerEvent.HoverIn && _isHover)
			{
				Play();
			}
			else if (TriggerEvent == ETriggerEvent.HoverOut && !_isHover)
			{
				Play();
			}
		}

		private void OnPress(bool _isPressed)
		{
			if (TriggerEvent == ETriggerEvent.Pressed && _isPressed)
			{
				Play();
			}
			else if (TriggerEvent == ETriggerEvent.Released && !_isPressed)
			{
				Play();
			}
		}

		private void OnSelect(bool _isSelected)
		{
			if (TriggerEvent == ETriggerEvent.Selected && _isSelected)
			{
				Play();
			}
			else if (TriggerEvent == ETriggerEvent.Deselected && !_isSelected)
			{
				Play();
			}
		}

		private void OnDestroy()
		{
			if (m_handle != null)
			{
				if (m_handle.IsPlaying)
				{
					m_handle.ChangeStoppedFinishedStrategy(EStoppedFinishedStrategy.UnloadAll);
				}
				else
				{
					m_handle.Unload();
				}
			}
		}
	}
}
