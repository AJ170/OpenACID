using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.ModulePopup
{
	public class ModulePopupContent : PopUpContentView<ModulePopupContent.ModulePopupData>
	{
		public class PopupModuleConfiguration
		{
			public readonly InventoryElement Element;

			public readonly string ModuleGuid = string.Empty;

			public readonly ModuleData Module;

			public readonly LootTemplate.ItemTemplate PayoutModule;

			public readonly long Amount = 1L;

			public readonly Color32 RarityColor = Color.white;

			public readonly string OverrideColorDefGuid = string.Empty;

			public PopupModuleConfiguration(InventoryElement _element)
			{
				Element = _element;
				Module = ((_element == null || !_element.IsModule()) ? null : _element.moduleData);
				ModuleGuid = ((_element == null) ? string.Empty : _element.guid);
				Amount = 1L;
				Quality.EQuality itemDisplayQuality = _element.GetItemDisplayQuality();
				RarityColor = UIItemUtil.GetItemQualityColor(itemDisplayQuality);
			}
		}

		public class ModulePopupData : PopUpContentData
		{
			public PopupModuleConfiguration Module;

			public List<UIPopupResourcesContent.ResourceConfiguration> ResourcesCost;

			public bool ResourceSymbolAdditive;

			public string Description;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.Module;
				}
			}
		}

		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private UISprite m_moduleBackgroundImage;

		[SerializeField]
		private UIPortraitTexture m_moduleTexture;

		public override Vector2 Size
		{
			get
			{
				return m_moduleTexture.localSize;
			}
		}

		private void Awake()
		{
			m_description.supportEncoding = true;
		}

		protected override void Render(ModulePopupData _data)
		{
			m_moduleBackgroundImage.gameObject.SetActive(false);
			m_description.text = _data.Description;
			ConfigurateModule(_data.Module);
		}

		public override void OnLayoutFinished()
		{
			base.OnLayoutFinished();
			m_moduleBackgroundImage.gameObject.SetActive(true);
		}

		public override void UpdateLayout()
		{
			OnAlignmentUpdate();
		}

		private void OnAlignmentUpdate()
		{
			EventDelegate.Execute(onLayoutFinished);
		}

		private void ConfigurateModule(PopupModuleConfiguration _module)
		{
			m_moduleTexture.IsResizeWidgetToOrgSize = true;
			m_moduleBackgroundImage.color = _module.RarityColor;
			UIItemUtil.UpdateItem(_module.Element, true, m_moduleTexture, false, string.Empty, 0f);
		}
	}
}
