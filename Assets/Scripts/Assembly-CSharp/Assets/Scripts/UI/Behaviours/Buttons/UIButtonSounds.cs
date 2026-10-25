using System;
using Assets.Scripts.UI.NGUIExtentions;
using Database;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	[RequireComponent(typeof(UIExtendedButton))]
	public class UIButtonSounds : MonoBehaviour
	{
		[SerializeField]
		private EButtonSoundFamily m_soundFamily;

		[SerializeField]
		private EButtonIntent m_soundIntent;

		private UIExtendedButton m_button;

		private readonly BBSimpleList<UIPlayAudio> m_players = new BBSimpleList<UIPlayAudio>(2);

		private bool m_suppressAudio;

		public EButtonSoundFamily SoundFamily
		{
			get
			{
				return m_soundFamily;
			}
			set
			{
				m_soundFamily = value;
				RecreateConfiguration();
			}
		}

		public EButtonIntent ButtonIntent
		{
			get
			{
				return m_soundIntent;
			}
			set
			{
				m_soundIntent = value;
				RecreateConfiguration();
			}
		}

		public void SetSuppressAudio(bool suppress)
		{
			m_suppressAudio = suppress;
			if (m_suppressAudio)
			{
				RecreateConfiguration();
			}
			else
			{
				ClearSoundPlayers();
			}
		}

		private void OnEnable()
		{
			m_button = GetComponent<UIExtendedButton>();
			UpdateConfiguration();
			EventDelegate.Add(m_button.onBeforeClick, OnButtonBeforeClick);
			EventDelegate.Add(m_button.onTypeChange, OnButtonTypeChange);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onBeforeClick, OnButtonBeforeClick);
			EventDelegate.Remove(m_button.onTypeChange, OnButtonTypeChange);
		}

		private void OnButtonBeforeClick()
		{
			for (int i = 0; i < m_players.Count; i++)
			{
				m_players[i].Play();
			}
		}

		private void OnButtonTypeChange()
		{
			RecreateConfiguration();
		}

		private void RecreateConfiguration()
		{
			ClearSoundPlayers();
			UpdateConfiguration();
		}

		private void ClearSoundPlayers()
		{
			for (int i = 0; i < m_players.Count; i++)
			{
				UnityEngine.Object.Destroy(m_players[i]);
			}
			m_players.Clear();
		}

		private void UpdateConfiguration()
		{
			if (m_button == null || m_suppressAudio || m_players.Count != 0)
			{
				return;
			}
			EButtonSoundFamily soundFamily = m_soundFamily;
			if (soundFamily != 0 && soundFamily == EButtonSoundFamily.Transition)
			{
				CreatePlayer(DefaultAudioControllers.Transition);
			}
			switch (m_soundIntent)
			{
			case EButtonIntent.Default:
				switch (m_button.Type)
				{
				case EButtonTypes.Normal:
					CreatePlayer(DefaultAudioControllers.Normal);
					break;
				case EButtonTypes.Action:
					CreatePlayer(DefaultAudioControllers.Action);
					break;
				case EButtonTypes.Image:
					CreatePlayer(DefaultAudioControllers.Normal);
					break;
				case EButtonTypes.List:
					CreatePlayer(DefaultAudioControllers.Normal);
					break;
				case EButtonTypes.Event:
					CreatePlayer(DefaultAudioControllers.Normal);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				break;
			case EButtonIntent.Ok:
				CreatePlayer(DefaultAudioControllers.Ok);
				break;
			case EButtonIntent.Accept:
				CreatePlayer(DefaultAudioControllers.Accept);
				break;
			case EButtonIntent.Cancel:
				CreatePlayer(DefaultAudioControllers.Cancel);
				break;
			case EButtonIntent.Close:
				CreatePlayer(DefaultAudioControllers.Close);
				break;
			case EButtonIntent.Reset:
				CreatePlayer(DefaultAudioControllers.Reset);
				break;
			case EButtonIntent.Buy:
				CreatePlayer(DefaultAudioControllers.Buy);
				break;
			case EButtonIntent.Equip:
				CreatePlayer(DefaultAudioControllers.Equip);
				break;
			case EButtonIntent.Unequip:
				CreatePlayer(DefaultAudioControllers.Unequip);
				break;
			case EButtonIntent.Claim:
				CreatePlayer(DefaultAudioControllers.Claim);
				break;
			case EButtonIntent.Delete:
				CreatePlayer(DefaultAudioControllers.Delete);
				break;
			case EButtonIntent.Decrypt:
				CreatePlayer(DefaultAudioControllers.Decypt);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private UIPlayAudio CreatePlayer(GameID _id)
		{
			if (GameID.IsValid(_id))
			{
				UIPlayAudio uIPlayAudio = base.gameObject.AddComponent<UIPlayAudio>();
				uIPlayAudio.AudioController = _id;
				uIPlayAudio.Volume = 1f;
				uIPlayAudio.TriggerEvent = UIPlayAudio.ETriggerEvent.None;
				uIPlayAudio.PlayType = UIPlayAudio.EPlayType.StopAndPlay;
				m_players.Add(uIPlayAudio);
				return uIPlayAudio;
			}
			return null;
		}
	}
}
