using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	[RequireComponent(typeof(UIExtendedButton))]
	public class UIButtonGlow : MonoBehaviour
	{
		[SerializeField]
		private UISprite[] m_glowSprites;

		private UIExtendedButton m_button;

		private void Awake()
		{
			if (m_glowSprites == null)
			{
				m_glowSprites = new UISprite[0];
			}
			m_button = GetComponent<UIExtendedButton>();
			for (int i = 0; i < m_glowSprites.Length; i++)
			{
				m_glowSprites[i].enabled = true;
			}
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_button.onStateChange, OnStateChange);
			EventDelegate.Add(m_button.onVisualsUpdated, OnVisualsUpdated);
			EventDelegate.Add(m_button.onTypeChange, OnTypeChange);
			OnStateChange();
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onStateChange, OnStateChange);
			EventDelegate.Remove(m_button.onVisualsUpdated, OnVisualsUpdated);
			EventDelegate.Add(m_button.onTypeChange, OnTypeChange);
		}

		private void OnTypeChange()
		{
			UpdateColors();
		}

		private void OnVisualsUpdated()
		{
			for (int i = 0; i < m_glowSprites.Length; i++)
			{
				m_glowSprites[i].Invalidate(true);
			}
			UpdateColors();
		}

		private void OnStateChange()
		{
			for (int i = 0; i < m_glowSprites.Length; i++)
			{
				m_glowSprites[i].enabled = m_button.state != UIButtonColor.State.Disabled && m_button.state != UIButtonColor.State.Pressed;
			}
			UpdateColors();
		}

		private void UpdateColors()
		{
			ButtonConfiguration value;
			if (UIDataMapper.ButtonConfigurations.TryGetValue((int)m_button.Type, out value))
			{
				for (int i = 0; i < m_glowSprites.Length; i++)
				{
					m_glowSprites[i].color = value.GlowColor;
					m_glowSprites[i].Invalidate(true);
				}
			}
		}
	}
}
