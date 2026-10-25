using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.Widget
{
	public class StatsHeader : MonoBehaviour, IInitialized<StatsHeader.HeaderData>
	{
		public class HeaderData
		{
			public StringId Header;
		}

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_label;

		public void Initialize(HeaderData _data)
		{
			SetupData(_data);
		}

		public void Clear()
		{
		}

		private void Awake()
		{
		}

		private void SetupData(HeaderData _data)
		{
			if (m_label != null)
			{
				m_label.LocalizationKey = _data.Header;
			}
		}
	}
}
