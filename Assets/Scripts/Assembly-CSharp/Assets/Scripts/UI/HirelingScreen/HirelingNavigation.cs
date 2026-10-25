using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.HirelingScreen
{
	public class HirelingNavigation : MonoBehaviour
	{
		[SerializeField]
		private UIScrollBar m_scrollBar;

		[SerializeField]
		private UIButton m_openPreviewButton;

		[SerializeField]
		private HirelingData m_hirelingData;

		private List<HirelingTab> m_tabList;

		private HirelingTab m_currentTab;

		public void RemoteInitializeWhenDataIsReady(List<HirelingTab> _hirelingTabListfromDataScript)
		{
			m_tabList = _hirelingTabListfromDataScript;
			for (int i = 0; i < _hirelingTabListfromDataScript.Count; i++)
			{
				EventDelegate.Add(m_tabList[i].gameObject.AddComponentIfNotExist<UIButton>().onClick, OnClickedTab);
			}
			m_tabList[0].Scale.from = Vector3.one;
			m_tabList[0].Scale.to = new Vector3(1.1f, 1.1f, 1.1f);
			m_tabList[0].Scale.ResetToBeginning();
			m_tabList[0].Scale.PlayForward();
			m_tabList[0].Highlight.PlayForward();
			m_currentTab = m_tabList[0];
			Singleton<StateManager>.Instance.ProfileManager.LastSelectedHirelingId = m_tabList[0].HirelingID;
			if (m_currentTab != null)
			{
				HirelingAssassin hirelingAssassinData = m_currentTab.HirelingData.HirelingAssassinData;
				m_openPreviewButton.gameObject.SetActive(hirelingAssassinData.hirelingType != HirelingAssassin.EHirelingType.RandomGenerated);
			}
			if (m_scrollBar != null)
			{
				m_scrollBar.value = 0f;
			}
		}

		public void OnClickOpenPreview()
		{
			if (!(m_currentTab != null))
			{
				return;
			}
			HirelingAssassin hirelingData = m_currentTab.HirelingData.HirelingAssassinData;
			UIAssasinId currentAssassinId = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId;
			AsyncCallback callback = delegate(IAsyncResult _response)
			{
				if (!_response.IsError())
				{
					GetForeignAssassinResponse getForeignAssassinResponse = ((AsyncDataResult<GetForeignAssassinResponse>)_response).Result();
					Singleton<UIScreenManager>.Instance.SceneData.AssassinAndEquipmentPreview.OpenPreview(hirelingData.userName, hirelingData.rankIdx, -1, getForeignAssassinResponse.assassin);
					UpdateScreenWithNewAssassind(hirelingData.hirelingId, getForeignAssassinResponse.assassin);
				}
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetForeignAssassinByHirelingId(hirelingData.hirelingId, currentAssassinId, _asyncResult);
			}, null, callback);
		}

		private void UpdateScreenWithNewAssassind(string _hirelingId, Assassin _hirelingAssassin)
		{
			m_hirelingData.UpdateHirelingData(_hirelingId, _hirelingAssassin);
			m_tabList = m_hirelingData.TabsList;
			m_currentTab = m_hirelingData.CurrentSelectedTab;
			if (m_currentTab != null)
			{
				HirelingAssassin hirelingAssassinData = m_currentTab.HirelingData.HirelingAssassinData;
				m_openPreviewButton.gameObject.SetActive(hirelingAssassinData.hirelingType != HirelingAssassin.EHirelingType.RandomGenerated);
			}
		}

		private void Awake()
		{
			m_openPreviewButton.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			if (m_tabList != null)
			{
				for (int i = 0; i < m_tabList.Count; i++)
				{
					HirelingTab hirelingTab = m_tabList[i];
					if (hirelingTab != null && hirelingTab.gameObject != null)
					{
						UIButton component = hirelingTab.GetComponent<UIButton>();
						if (component != null)
						{
							EventDelegate.Remove(component.onClick, OnClickedTab);
						}
					}
				}
				m_tabList.Clear();
			}
			m_currentTab = null;
		}

		private void OnClickedTab()
		{
			HirelingTab component = UIButton.current.GetComponent<HirelingTab>();
			if (component != null && m_currentTab != null && component.HirelingID == m_currentTab.HirelingID)
			{
				UIMissionHelper.StartCurrentSelectedMissionWithTutorialPopup();
			}
			else if (component != null)
			{
				if (m_currentTab != null)
				{
					m_currentTab.Scale.to = Vector3.one;
					m_currentTab.Scale.from = new Vector3(1.1f, 1.1f, 1.1f);
					m_currentTab.Scale.ResetToBeginning();
					m_currentTab.Scale.PlayForward();
					m_currentTab.Highlight.PlayReverse();
				}
				Singleton<StateManager>.Instance.ProfileManager.LastSelectedHirelingId = component.HirelingID;
				component.Scale.from = Vector3.one;
				component.Scale.to = new Vector3(1.1f, 1.1f, 1.1f);
				component.Scale.ResetToBeginning();
				component.Scale.PlayForward();
				component.Highlight.PlayForward();
				m_currentTab = component;
				HirelingAssassin hirelingAssassinData = m_currentTab.HirelingData.HirelingAssassinData;
				m_openPreviewButton.gameObject.SetActive(hirelingAssassinData.hirelingType != HirelingAssassin.EHirelingType.RandomGenerated);
			}
		}
	}
}
