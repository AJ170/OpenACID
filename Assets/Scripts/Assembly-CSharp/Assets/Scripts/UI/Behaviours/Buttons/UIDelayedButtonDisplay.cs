using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	public class UIDelayedButtonDisplay : MonoBehaviour
	{
		[SerializeField]
		private UISprite m_sprite;

		private UIExtendedButton m_button;

		private void Awake()
		{
			m_button = GetComponent<UIExtendedButton>();
			if (m_button == null)
			{
				UIButtonProxy component = GetComponent<UIButtonProxy>();
				if (component != null)
				{
					m_button = component.Button;
				}
			}
		}

		private void OnEnable()
		{
			if (m_button != null)
			{
				m_button.OnDelayUpdate += OnDelayUpdate;
			}
		}

		private void OnDisable()
		{
			if (m_button != null)
			{
				m_button.OnDelayUpdate -= OnDelayUpdate;
			}
		}

		private void OnDelayUpdate(float _total, float _normalized)
		{
			m_sprite.fillAmount = _normalized;
		}
	}
}
