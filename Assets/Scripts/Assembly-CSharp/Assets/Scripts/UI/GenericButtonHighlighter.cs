using System;
using System.Collections.Generic;
using Assets.Scripts.Database;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class GenericButtonHighlighter : MonoBehaviour
	{
		public enum EColorType
		{
			Invalid = 0,
			Green = 1,
			Blue = 2
		}

		private enum EButtonIdentifier
		{
			Invalid = 0,
			Inventory = 1
		}

		[SerializeField]
		private UISprite m_Sprite;

		[SerializeField]
		private EColorType m_colorType;

		[SerializeField]
		private List<UISprite> m_spritesToTurnOff;

		[SerializeField]
		private UISprite m_exclamationMark;

		[SerializeField]
		private EButtonIdentifier m_buttonIdentifier;

		[SerializeField]
		private UIButton m_button;

		[SerializeField]
		private UIEventTrigger m_eventTrigger;

		private TweenScale m_tweenScale;

		public EColorType ColorType
		{
			set
			{
				m_colorType = value;
				Configure();
			}
		}

		private void Awake()
		{
			if (m_button == null)
			{
				m_button = base.gameObject.AddComponentIfNotExist<UIButton>();
			}
			SetupTrigger();
			SetupScaleTween();
			EventDelegate.Add(Singleton<StateManager>.Instance.ItemNotificationManager.onInventoryNotificationChange, CheckInventoryNotifications);
		}

		private void Start()
		{
			Configure();
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, UpdateState);
		}

		public void Configure()
		{
			Color32 color;
			Color32 color2;
			Color32 color3;
			switch (m_colorType)
			{
			case EColorType.Invalid:
				color = m_Sprite.color;
				color2 = m_Sprite.color;
				color3 = m_Sprite.color;
				break;
			case EColorType.Green:
			{
				color = UIDataMapper.ButtonConfigurations[1].PressedColor;
				color2 = UIDataMapper.ButtonConfigurations[1].HoverColor;
				color3 = UIDataMapper.ButtonConfigurations[1].DefaultColor;
				for (int j = 0; j < m_spritesToTurnOff.Count; j++)
				{
					m_spritesToTurnOff[j].color = UIDataMapper.ButtonConfigurations[1].GlowColor;
				}
				break;
			}
			case EColorType.Blue:
			{
				color = UIDataMapper.ButtonConfigurations[0].PressedColor;
				color2 = UIDataMapper.ButtonConfigurations[0].HoverColor;
				color3 = UIDataMapper.ButtonConfigurations[0].DefaultColor;
				for (int i = 0; i < m_spritesToTurnOff.Count; i++)
				{
					m_spritesToTurnOff[i].color = UIDataMapper.ButtonConfigurations[0].GlowColor;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (m_button != null)
			{
				m_Sprite.color = color3;
				m_button.defaultColor = color3;
				m_button.hover = color2;
				m_button.pressed = color;
				m_button.duration = 0.05f;
			}
			CheckInventoryNotifications();
		}

		public void UpdateState()
		{
			if (!m_button)
			{
				return;
			}
			Configure();
			for (int i = 0; i < m_spritesToTurnOff.Count; i++)
			{
				if (m_button.isEnabled)
				{
					m_spritesToTurnOff[i].enabled = true;
				}
				else
				{
					m_spritesToTurnOff[i].enabled = false;
				}
			}
		}

		private void SetupTrigger()
		{
			m_eventTrigger = base.gameObject.AddComponentIfNotExist<UIEventTrigger>();
			EventDelegate.Add(m_eventTrigger.onPress, OnPressFunction);
			EventDelegate.Add(m_button.onPressedStateChange, OnStateChanged);
		}

		private void OnDestroy()
		{
			EventDelegate.Remove(m_eventTrigger.onPress, OnPressFunction);
			EventDelegate.Remove(m_button.onPressedStateChange, OnStateChanged);
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, UpdateState);
			EventDelegate.Remove(Singleton<StateManager>.Instance.ItemNotificationManager.onInventoryNotificationChange, CheckInventoryNotifications);
		}

		private void SetupScaleTween()
		{
			m_tweenScale = m_Sprite.gameObject.AddComponentIfNotExist<TweenScale>();
			m_tweenScale.enabled = false;
			m_tweenScale.from = Vector3.one;
			m_tweenScale.to = new Vector3(0.9f, 0.9f, 0.9f);
			m_tweenScale.duration = 0.05f;
			m_tweenScale.ResetToBeginning();
		}

		public void OnPressFunction()
		{
			if (m_button.isEnabled)
			{
				m_tweenScale.ResetToBeginning();
				for (int i = 0; i < m_spritesToTurnOff.Count; i++)
				{
					m_spritesToTurnOff[i].enabled = false;
				}
				EventDelegate.Remove(m_tweenScale.onFinished, UpdateSprite);
				m_tweenScale.PlayForward();
			}
		}

		private void UpdateSprite()
		{
			for (int i = 0; i < m_spritesToTurnOff.Count; i++)
			{
				m_spritesToTurnOff[i].Update();
				m_spritesToTurnOff[i].alpha = 0.69f;
				m_spritesToTurnOff[i].alpha = 0.7f;
			}
		}

		private void OnStateChanged()
		{
			UpdateSprite();
			if (!m_button.ExtendedPressed)
			{
				m_tweenScale.PlayReverse();
				for (int i = 0; i < m_spritesToTurnOff.Count; i++)
				{
					if (m_button.isEnabled)
					{
						m_spritesToTurnOff[i].enabled = true;
					}
					m_spritesToTurnOff[i].Update();
				}
				EventDelegate.Add(m_tweenScale.onFinished, UpdateSprite, true);
			}
			else
			{
				for (int j = 0; j < m_spritesToTurnOff.Count; j++)
				{
					m_spritesToTurnOff[j].enabled = false;
				}
				m_tweenScale.PlayForward();
			}
			for (int k = 0; k < m_spritesToTurnOff.Count; k++)
			{
				if (!m_button.isEnabled)
				{
					m_spritesToTurnOff[k].enabled = false;
				}
			}
		}

		private void CheckInventoryNotifications()
		{
			if (m_buttonIdentifier == EButtonIdentifier.Inventory)
			{
				if (UbiCore.Utils.Singleton<LocalStorage>.Instance.ItemUnseen)
				{
					ShowExclamationmark();
				}
				else
				{
					HideExclamationmark();
				}
			}
		}

		private void ShowExclamationmark()
		{
			if (m_exclamationMark != null)
			{
				m_exclamationMark.alpha = 1f;
			}
		}

		private void HideExclamationmark()
		{
			if (m_exclamationMark != null)
			{
				m_exclamationMark.alpha = 0f;
			}
		}
	}
}
