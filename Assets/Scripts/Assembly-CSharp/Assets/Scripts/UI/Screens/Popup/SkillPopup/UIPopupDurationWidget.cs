using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Types;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.SkillPopup
{
	public class UIPopupDurationWidget : MonoBehaviour
	{
		private static readonly StringId s_TimeId = 1153;

		[SerializeField]
		private UIExtendedLabel m_label;

		[SerializeField]
		private GameObject m_parent;

		[SerializeField]
		private UIProgressBar m_progressBar;

		public bool Visible
		{
			get
			{
				return m_parent.activeSelf;
			}
			set
			{
				m_parent.SetActive(value);
			}
		}

		private void Awake()
		{
		}

		public void Init(UIDuration _duration, float _progress)
		{
			if (_duration != null)
			{
				m_label.text = LocalizationManager.Instance.Get(s_TimeId, new StringMapper { { "Training_Time_Left", _duration } });
			}
			m_progressBar.value = 0f;
		}
	}
}
