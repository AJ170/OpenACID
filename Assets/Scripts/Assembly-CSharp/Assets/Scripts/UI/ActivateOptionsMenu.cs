using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.NGUIExtentions;
using Assets.Scripts.UI.ScreenManager;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class ActivateOptionsMenu : MonoBehaviour
	{
		[SerializeField]
		private UIButton m_trigger;

		private UIPlayAudio[] m_buttonSoundsActivate;

		private UIPlayAudio[] m_buttonSoundsDeactivate;

		private void Start()
		{
			EventDelegate.Add(m_trigger.onClick, OptionsScreenHelper.ActivateMenu);
			m_buttonSoundsActivate = GetComponents<UIPlayAudio>();
			m_buttonSoundsDeactivate = Singleton<UIScreenManager>.Instance.SceneData.BackButton.GetComponents<UIPlayAudio>();
		}

		private void OnDestroy()
		{
			for (int i = 0; i < m_buttonSoundsActivate.Length; i++)
			{
				m_buttonSoundsActivate[i] = null;
			}
			for (int j = 0; j < m_buttonSoundsDeactivate.Length; j++)
			{
				m_buttonSoundsDeactivate[j] = null;
			}
			EventDelegate.Remove(m_trigger.onClick, OptionsScreenHelper.ActivateMenu);
		}

		private void Update()
		{
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			if (!gamepad.PauseDown)
			{
				return;
			}
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.OptionsMenu);
			if (screen == null)
			{
				return;
			}
			if (!screen.Active)
			{
				if (UIGamepadHandler.CanHitCollider(m_trigger.gameObject))
				{
					OptionsScreenHelper.ActivateMenu();
					for (int i = 0; i < m_buttonSoundsActivate.Length; i++)
					{
						m_buttonSoundsActivate[i].Play();
					}
				}
			}
			else
			{
				OptionsScreenHelper.DeactivateMenu();
				for (int j = 0; j < m_buttonSoundsDeactivate.Length; j++)
				{
					m_buttonSoundsDeactivate[j].Play();
				}
			}
		}
	}
}
