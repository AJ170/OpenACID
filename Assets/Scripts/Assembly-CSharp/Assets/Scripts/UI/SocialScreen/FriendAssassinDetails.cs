using System;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.SocialScreen
{
	public class FriendAssassinDetails : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_headerLabel;

		[SerializeField]
		private UIExtendedLabel m_assassinNameLabel;

		[SerializeField]
		private UIExtendedLabel m_assassinClassLabel;

		[SerializeField]
		private UILabel m_assassinItemPowerLabel;

		[SerializeField]
		private FriendAssassinTab[] m_tabs = new FriendAssassinTab[0];

		[SerializeField]
		private float m_tweenDuration = 1f;

		[SerializeField]
		private int m_tweenSizeMin = 176;

		[SerializeField]
		private int m_tweenSizeMax = 218;

		private int m_activeTabs;

		private int m_selection = -1;

		private FriendProfile m_friendProfile;

		public void UpdateData(FriendProfile _friend)
		{
			if (_friend.friendAssassin == null || _friend.friendAssassin.Length == 0)
			{
				base.gameObject.SetActive(false);
				m_friendProfile = null;
				return;
			}
			m_friendProfile = _friend;
			base.gameObject.SetActive(true);
			Array.Sort(_friend.friendAssassin, SortAssassinsByItemPower);
			int num = 0;
			for (num = 0; num < m_tabs.Length && num < _friend.friendAssassin.Length; num++)
			{
				FriendAssassinTab friendAssassinTab = m_tabs[num];
				friendAssassinTab.gameObject.SetActive(true);
				friendAssassinTab.Assassin = _friend.friendAssassin[num];
				friendAssassinTab.Clock = ((num != 0) ? 2f : 1f);
				UIPortraitUtil.UpdatePortraitByColorDef(friendAssassinTab.Assassin, friendAssassinTab.Portrait);
			}
			for (m_activeTabs = num; num < m_tabs.Length; num++)
			{
				m_tabs[num].gameObject.SetActive(false);
				m_tabs[num].Assassin = null;
			}
			m_headerLabel.SetLocalizationKey((_friend.numAssassins <= m_tabs.Length) ? 4552 : 4437);
			m_selection = 0;
			base.enabled = true;
			UpdateAssassinDetails();
		}

		private int SortAssassinsByItemPower(FriendAssassin _a, FriendAssassin _b)
		{
			return _b.itemPower.CompareTo(_a.itemPower);
		}

		public void OnClickPreviewAssassin()
		{
			FriendAssassinTab friendAssassinTab = m_tabs[m_selection];
			FriendAssassin friendAssassin = friendAssassinTab.Assassin;
			AsyncCallback callback = delegate(IAsyncResult _response)
			{
				if (!_response.IsError())
				{
					GetForeignAssassinResponse getForeignAssassinResponse = ((AsyncDataResult<GetForeignAssassinResponse>)_response).Result();
					Singleton<UIScreenManager>.Instance.SceneData.AssassinAndEquipmentPreview.OpenPreview(m_friendProfile.userName, m_friendProfile.maxRankIdx, -1, getForeignAssassinResponse.assassin);
				}
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetForeignAssassinByAssassinId(m_friendProfile.userIdHash, friendAssassin.id, _asyncResult);
			}, null, callback);
		}

		public void OnClick()
		{
			int num = m_selection;
			for (int i = 0; i < m_activeTabs; i++)
			{
				if (UIEventTrigger.current == m_tabs[i].UIEventTrigger)
				{
					num = i;
				}
			}
			if (m_selection != num)
			{
				base.enabled = true;
				m_selection = num;
				UpdateAssassinDetails();
			}
		}

		private void UpdateAssassinDetails()
		{
			FriendAssassin assassin = m_tabs[m_selection].Assassin;
			long itemPower = assassin.itemPower;
			m_assassinNameLabel.text = AssassinExtensions.ComputeAssassinName(assassin.assassinName);
			m_assassinItemPowerLabel.text = LocalizationManager.Instance.GetInteger(itemPower);
			FriendUtil.SetAssassinClassLabel(m_assassinClassLabel, assassin.classTypeId, assassin.extendedClassTypeId);
		}

		private void Awake()
		{
			for (int i = 0; i < m_tabs.Length; i++)
			{
				FriendAssassinTab friendAssassinTab = m_tabs[i];
				friendAssassinTab.Widget.ResizeCollider();
				friendAssassinTab.gameObject.SetActive(false);
				friendAssassinTab.Clock = ((i != 0) ? 2f : 1f);
				EventDelegate.Add(friendAssassinTab.UIEventTrigger.onClick, OnClick);
			}
			m_selection = 0;
			m_activeTabs = 0;
			base.enabled = false;
		}

		private void Update()
		{
			float num = 0f;
			bool flag = true;
			for (int i = 0; i < m_activeTabs; i++)
			{
				FriendAssassinTab friendAssassinTab = m_tabs[i];
				bool flag2 = m_selection == i;
				if (!flag2 && friendAssassinTab.Clock < 1f)
				{
					friendAssassinTab.Clock = 2f - friendAssassinTab.Clock;
				}
				if (flag2 && friendAssassinTab.Clock > 1f)
				{
					friendAssassinTab.Clock = 2f - friendAssassinTab.Clock;
				}
				friendAssassinTab.Clock += Time.deltaTime / m_tweenDuration;
				friendAssassinTab.Clock = ((!flag2) ? Mathf.Clamp(friendAssassinTab.Clock, 1f, 2f) : Mathf.Clamp(friendAssassinTab.Clock, 0f, 1f));
				flag &= ((!flag2) ? (friendAssassinTab.Clock >= 2f) : (friendAssassinTab.Clock >= 1f));
				float clock = friendAssassinTab.Clock;
				if (flag2)
				{
					clock = 1f - clock;
					clock = 1f - clock * clock * clock;
				}
				else
				{
					clock = 2f - clock;
					clock = clock * clock * clock;
				}
				float num2 = Mathf.Lerp(m_tweenSizeMin, m_tweenSizeMax, clock);
				friendAssassinTab.Widget.SetRect(num, 0f, num2, num2);
				Color color = friendAssassinTab.Highlight.color;
				color.a = clock;
				friendAssassinTab.Highlight.color = color;
				num += num2;
			}
			if (flag)
			{
				for (int j = 0; j < m_activeTabs; j++)
				{
					m_tabs[j].Widget.ResizeCollider();
				}
				base.enabled = false;
			}
		}
	}
}
