using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Generic
{
	public class LockView : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_lockLabel;

		[SerializeField]
		private GameObject m_lockDisplay;

		public bool Visible
		{
			get
			{
				return m_lockDisplay.activeSelf;
			}
			set
			{
				m_lockDisplay.SetActive(value);
			}
		}

		private void Awake()
		{
		}

		public void SetLabel(StringId _id)
		{
			m_lockLabel.LocalizationKey = _id;
		}

		public void SetLabel(string _text)
		{
			m_lockLabel.UseLocalizedText = false;
			m_lockLabel.text = _text;
		}
	}
}
