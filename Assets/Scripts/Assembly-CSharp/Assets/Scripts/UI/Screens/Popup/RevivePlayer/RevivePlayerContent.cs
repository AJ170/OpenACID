using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.RevivePlayer
{
	public class RevivePlayerContent : PopUpContentView<RevivePlayerContent.RevivePlayerData>
	{
		public class RevivePlayerData : PopUpContentData
		{
			public List<UIPopupResourcesContent.ResourceConfiguration> ResourcesCost;

			public string Description;

			public string Headline;

			public bool ResourceSymbolAdditive;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.RevivePlayer;
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

		[SerializeField]
		private UIAlignmentBox m_vbox;

		[SerializeField]
		private UIAlignmentBox m_iconVBox;

		[SerializeField]
		private UIExtendedLabel m_currencyLabelLeft;

		[SerializeField]
		private UISprite m_currencySpriteLeft;

		[SerializeField]
		private UIAlignmentBox m_currencyLeftVBox;

		[SerializeField]
		private UIExtendedLabel m_currencyLabelRight;

		[SerializeField]
		private UISprite m_currencySpriteRight;

		[SerializeField]
		private UIAlignmentBox m_currencyRightVBox;

		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private UIExtendedLabel m_headingLabel;

		[SerializeField]
		private UIPopupResourcesContent m_uiPopupResources;

		private void Awake()
		{
		}

		protected override void Render(RevivePlayerData _data)
		{
			m_uiPopupResources.ConfigurateItemsCost(_data.ResourcesCost, _data.ResourceSymbolAdditive);
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
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			m_currencyLabelLeft.text = LocalizationManager.Instance.GetInteger(profile.GetResourceAmountByType(_data.ResourcesCost[0].Resource.type));
			m_currencySpriteLeft.spriteName = UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions[(int)_data.ResourcesCost[0].Resource.type].SmallIconName;
			if (_data.ResourcesCost.Count > 1)
			{
				m_currencyLabelRight.transform.parent.gameObject.SetActive(true);
				m_currencyLabelRight.text = LocalizationManager.Instance.GetInteger(profile.GetResourceAmountByType(_data.ResourcesCost[1].Resource.type));
				m_currencySpriteRight.spriteName = UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions[(int)_data.ResourcesCost[1].Resource.type].SmallIconName;
			}
			else
			{
				m_currencyRightVBox.transform.parent.gameObject.SetActive(false);
			}
			m_currencyLeftVBox.RepositionNow = true;
			m_currencyRightVBox.RepositionNow = true;
			m_iconVBox.RepositionNow = true;
		}

		public override void UpdateLayout()
		{
			OnIconsAligned();
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
