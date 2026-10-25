using System.Collections.Generic;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.EarlyAccess
{
	public class EarlyAccessContent : PopUpContentView<EarlyAccessContent.EarlyAccessData>
	{
		public class EarlyAccessData : PopUpContentData
		{
			public List<UIPopupResourcesContent.ResourceConfiguration> ResourcesCost;

			public string Description;

			public string Headline;

			public bool ResourceSymbolAdditive;

			public float RewardIncreaseFactor;

			public bool ShowResources;

			public bool ShowModifier;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.EarlyAccess;
				}
			}

			public override bool HasResourceContentDisplay
			{
				get
				{
					return true;
				}
			}
		}

		private static readonly StringId s_lootId = new StringId(5489);

		private static readonly StringId s_resourceId = new StringId(5482);

		[SerializeField]
		private UIAlignmentBox m_vbox;

		[SerializeField]
		private UIAlignmentBox m_iconVBox;

		[SerializeField]
		private GameObject m_iconLeft;

		[SerializeField]
		private GameObject m_iconRight;

		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private UIExtendedLabel m_lootLabel;

		[SerializeField]
		private UIExtendedLabel m_resourceLabel;

		[SerializeField]
		private UIExtendedLabel m_headingLabel;

		[SerializeField]
		private UIPortraitTexture m_resourceTexture;

		[SerializeField]
		private UIPortraitTexture m_lootTexture;

		[SerializeField]
		private UIPopupResourcesContent m_uiPopupResources;

		private EarlyAccessData m_data;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			m_data = null;
		}

		protected override void Render(EarlyAccessData _data)
		{
			m_data = _data;
			m_uiPopupResources.ConfigurateItemsCost(_data.ResourcesCost, _data.ResourceSymbolAdditive);
			m_lootLabel.LocalizationKey = s_lootId;
			m_headingLabel.text = _data.Headline;
			m_description.text = _data.Description;
			if (string.IsNullOrEmpty(m_headingLabel.text))
			{
				m_headingLabel.gameObject.SetActive(false);
			}
			if (string.IsNullOrEmpty(m_description.text))
			{
				m_description.gameObject.SetActive(false);
			}
			m_resourceLabel.text = LocalizationManager.Instance.Get(s_resourceId, new StringMapper { 
			{
				"more_coins_percentage",
				new UIPercentage(_data.RewardIncreaseFactor)
			} });
			if (_data.ShowResources)
			{
				m_iconLeft.SetActive(true);
				UIItemUtil.UpdateItemByColorDef(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_softCurrencyStack, Color.white, true, null, m_resourceTexture, false, UIItemUtil.ItemUVCoordType.ENone, false, 0f);
			}
			else
			{
				m_iconLeft.SetActive(false);
			}
			if (_data.ShowModifier)
			{
				m_iconRight.SetActive(true);
				UIItemUtil.UpdateItemByColorDef(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_additionalLootChest, Color.black, true, null, m_lootTexture, false, UIItemUtil.ItemUVCoordType.ENone, false, 0f);
				m_lootTexture.RarityColor = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				m_iconRight.SetActive(false);
			}
		}

		public override void UpdateLayout()
		{
			if (m_data.ShowResources)
			{
				EventDelegate.Add(m_iconVBox.OnReposition, OnIconsAligned, true);
				m_iconVBox.RepositionNow = true;
			}
			else
			{
				OnIconsAligned();
			}
		}

		private void OnIconsAligned()
		{
			EventDelegate.Add(m_vbox.OnReposition, OnAlignmentUpdate, true);
			m_vbox.RepositionNow = true;
		}

		private void OnAlignmentUpdate()
		{
			EventDelegate.Execute(onLayoutFinished);
		}
	}
}
