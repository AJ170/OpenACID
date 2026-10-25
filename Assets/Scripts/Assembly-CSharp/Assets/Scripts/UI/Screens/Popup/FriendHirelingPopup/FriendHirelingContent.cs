using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.SocialScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.FriendHirelingPopup
{
	public class FriendHirelingContent : PopUpContentView<FriendHirelingContent.FriendHirelingData>
	{
		public class FriendHirelingData : PopUpContentData
		{
			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.FriendHireling;
				}
			}

			public HirelingAssassin Hireling { get; set; }
		}

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_classLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_assassinLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_usernameLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_levelLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_rewardLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_questionLabel;

		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture m_portraitTexture;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_resourceSprite;

		[CheckAssigned]
		[SerializeField]
		private UIHeroicPortraitTexture m_heroicPortrait;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_rewardNode;

		protected override void Render(FriendHirelingData _data)
		{
			HirelingAssassin hireling = _data.Hireling;
			FriendUtil.SetAssassinClassLabel(m_classLabel, hireling.classTypeId, Assassin.EClassTypeId.None);
			m_assassinLabel.text = hireling.GetHirelingName();
			m_usernameLabel.text = hireling.userName;
			m_levelLabel.text = LocalizationManager.Instance.GetInteger(hireling.itemPower);
			AcierProtocol.Resources addFriendReward = Singleton<StateManager>.Instance.TemplateManager.Globals.addFriendReward;
			bool flag = addFriendReward.resources.Length > 0;
			if (flag)
			{
				m_rewardLabel.text = LocalizationManager.Instance.GetInteger(addFriendReward.resources[0].amount);
				m_resourceSprite.enabled = UICurrencyHelper.SetCurrenySymbol(m_resourceSprite, addFriendReward.resources[0], true);
			}
			m_rewardNode.SetActive(flag);
			m_heroicPortrait.alpha = 0.1f;
			UIPortraitUtil.UpdateHeroicPortraitByHirelingAssassin(hireling, hireling.rankIdx, m_heroicPortrait);
			string text = NGUIText.EscapeSymbols(hireling.userName);
			m_questionLabel.SetLocalizationKey(4624, new StringMapper { 
			{
				"player_name",
				new UINGUIFormattedText(text, new NGUIColoredText(new Color(0.28627452f, 46f / 85f, 63f / 85f)))
			} });
			UIPortraitUtil.UpdatePortraitByColorDef(hireling, m_portraitTexture, true);
		}

		private void Awake()
		{
		}
	}
}
