using UnityEngine;

namespace Assets.Scripts.UI.Screens.GeneralSettingsScreen
{
	public class GlobalPushNotificationSection : MonoBehaviour
	{
		public delegate void GlobalSwitchDelegate();

		[SerializeField]
		private UIExtendedLabel m_switchLabel;

		[SerializeField]
		private GameObject m_separator;

		[SerializeField]
		public UISlider m_switchSlider;

		public static event GlobalSwitchDelegate OnGlobalSwitchDelegate;

		private void OnEnable()
		{
			EventDelegate.Add(m_switchSlider.onChange, OnSwitchUpdate);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_switchSlider.onChange, OnSwitchUpdate);
		}

		private void OnSwitchUpdate()
		{
			if (GlobalPushNotificationSection.OnGlobalSwitchDelegate != null)
			{
				GlobalPushNotificationSection.OnGlobalSwitchDelegate();
			}
		}

		public void SetSliderValue(float _value)
		{
			m_switchSlider.value = _value;
			GenericToggleHighlighter component = m_switchSlider.GetComponent<GenericToggleHighlighter>();
			component.OnValueChange();
		}

		public float GetSliderValue()
		{
			return m_switchSlider.value;
		}
	}
}
