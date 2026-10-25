using System;
using AcierProtocol;
using Assets.Scripts.UI.InfoPopup.Misc;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.ModuleScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup.Content
{
	public class ModuleInfoPopupContent : InfoPopUpContentView<ModuleInfoPopupContent.ModuleInfoPopUpData>
	{
		public class ModuleInfoPopUpData : InfoPopUpContentData
		{
			public InventoryElement Element;

			public string OverrideModuleColorGuid = string.Empty;

			public override UIInfoPopup.EContentType Type
			{
				get
				{
					return UIInfoPopup.EContentType.Module;
				}
			}

			public ModuleData Module
			{
				get
				{
					return (Element == null || !Element.IsModule()) ? null : Element.moduleData;
				}
			}
		}

		[SerializeField]
		private GameObject m_mainDataContainer;

		[SerializeField]
		private UIExtendedLabel m_helpTextLabel;

		[SerializeField]
		private UIExtendedLabel m_valueLabel;

		[SerializeField]
		private UIExtendedLabel m_unitLabel;

		[SerializeField]
		private UIExtendedLabel m_qualityLabel;

		[SerializeField]
		private UIPortraitTexture m_imageTexture;

		[SerializeField]
		private ModuleStatsListHandler m_moduleStatsListHandler;

		[SerializeField]
		private UIExtendedLabel m_durationLabel;

		[SerializeField]
		private UIProgressBar m_durationProgressBar;

		private ModuleInfoPopUpData m_data;

		private DateTime m_lastTime;

		public override UIPortraitTexture GetPortraitTexture()
		{
			return m_imageTexture;
		}

		private void Awake()
		{
		}

		protected override void Render(ModuleInfoPopUpData _data)
		{
			m_data = _data;
			bool flag = _data.Element != null;
			m_mainDataContainer.SetActive(flag);
			m_helpTextLabel.gameObject.SetActive(!flag);
			m_moduleStatsListHandler.IsEnable = flag;
			if (flag)
			{
				m_qualityLabel.gameObject.SetActive(true);
				Quality.EQuality itemDisplayQuality = m_data.Element.GetItemDisplayQuality();
				m_qualityLabel.text = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)itemDisplayQuality].NameId);
				m_unitLabel.text = m_data.Module.BuildUnitStringRaw(0);
				m_valueLabel.text = m_data.Module.BuildUnitValue().ToString();
				m_moduleStatsListHandler.Init(m_data.Element.moduleData);
				UIItemUtil.UpdateItem(m_data.Element, true, m_imageTexture, false, string.Empty, 0f);
			}
			else
			{
				m_helpTextLabel.text = "NOTEST LOCA";
			}
		}

		private void Update()
		{
			if (m_data != null && m_data.Module != null)
			{
				DateTime now = Singleton<StateManager>.Instance.ServerTimeManager.Now;
				if (!(now == m_lastTime))
				{
					m_durationLabel.text = m_data.Module.BuildTimeString();
					m_durationProgressBar.value = 1f - m_data.Module.RemainingPercentage();
					m_lastTime = now;
				}
			}
		}
	}
}
