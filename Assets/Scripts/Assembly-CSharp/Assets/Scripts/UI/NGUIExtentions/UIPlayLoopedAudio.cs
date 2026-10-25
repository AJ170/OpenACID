using System.Collections;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Utils;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtentions
{
	public class UIPlayLoopedAudio : MonoBehaviour
	{
		private IAudioHandle m_handle;

		private GameID m_audioController = GameID.Invalid;

		[SerializeField]
		private string m_guidString;

		[SerializeField]
		private AudioControllerAsset m_audioAsset;

		[SerializeField]
		private float m_volume = 1f;

		[SerializeField]
		private int m_triggerDelay = 500;

		private bool m_isPressed;

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

		public bool IsPlaying
		{
			get
			{
				return m_handle != null && m_handle.IsPlaying;
			}
		}

		public int TriggerDelay
		{
			get
			{
				return m_triggerDelay;
			}
			set
			{
				m_triggerDelay = value;
			}
		}

		public void Awake()
		{
			if (GameID.IsValid(AudioController))
			{
				m_handle = AudioManager.Instance.CreateHandle(AudioController, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
				m_handle.UnloadOnPlayFailed = false;
				m_handle.Load();
			}
		}

		public void Play()
		{
			if (m_handle != null)
			{
				m_handle.Volume = Volume;
				m_handle.Stop();
				m_handle.Play(true);
			}
		}

		public void PlayByButtonState(UIButtonProxy _proxy)
		{
			OnPress(_proxy.Button.state == UIButtonColor.State.Pressed);
		}

		private void OnPress(bool _isPressed)
		{
			m_isPressed = _isPressed;
			if (_isPressed)
			{
				StartCoroutine(PlayDelayed());
			}
			else
			{
				Stop();
			}
		}

		private IEnumerator PlayDelayed()
		{
			yield return new WaitForSeconds((float)TriggerDelay / 1000f);
			if (m_isPressed)
			{
				Play();
			}
		}

		public void Stop()
		{
			if (m_handle != null)
			{
				m_handle.Stop();
			}
		}

		public void OnDestroy()
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
