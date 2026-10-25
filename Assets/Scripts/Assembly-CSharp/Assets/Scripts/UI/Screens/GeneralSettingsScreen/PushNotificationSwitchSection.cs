using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.GeneralSettingsScreen
{
	public class PushNotificationSwitchSection : MonoBehaviour
	{
		public delegate void SingleSwitchDelegate();

		[SerializeField]
		private UIExtendedLabel m_switchLabel;

		[SerializeField]
		public GameObject m_separator;

		[SerializeField]
		private UISlider m_switchSlider;

		private EPushNotificationGroup m_type;

		public EPushNotificationGroup PushNotificationGroup
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
				UpdateData();
			}
		}

		public static event SingleSwitchDelegate OnSingleSwitchDelegate;

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
			if (PushNotificationSwitchSection.OnSingleSwitchDelegate != null)
			{
				PushNotificationSwitchSection.OnSingleSwitchDelegate();
			}
		}

		private void UpdateData()
		{
			PushNotificationSwitchDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.PushNotificationSwitchDescription.TryGetValue((int)PushNotificationGroup, out value))
			{
				m_switchLabel.text = LocalizationManager.Instance.Get(value.HeadingId);
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
