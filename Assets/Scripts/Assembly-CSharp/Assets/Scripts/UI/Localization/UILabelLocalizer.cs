using UnityEngine;

namespace Assets.Scripts.UI.Localization
{
	[ExecuteInEditMode]
	[AddComponentMenu("NGUI/Extras/Label Localizer")]
	public class UILabelLocalizer : UILocalization
	{
		private UILabel m_targetLabel;

		private void Awake()
		{
			m_targetLabel = GetComponent<UILabel>();
		}

		protected override void DoLocalize(StringId _key)
		{
			if (m_targetLabel == null)
			{
				m_targetLabel = GetComponent<UILabel>();
			}
			if (m_targetLabel != null)
			{
				m_targetLabel.text = LocalizationManager.Instance.Get(_key);
			}
		}
	}
}
