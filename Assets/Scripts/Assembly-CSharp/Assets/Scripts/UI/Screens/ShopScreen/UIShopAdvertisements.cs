using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.TweenHandling;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopAdvertisements : MonoBehaviour
	{
		[SerializeField]
		private UIShopAdvertisementDetails m_advertisement1X2Prefab;

		[SerializeField]
		private UIShopAdvertisementDetails m_advertisement1X1Prefab;

		[SerializeField]
		private UIShopScrollers m_parent;

		[SerializeField]
		private GameObject m_smallAdContainer;

		[SerializeField]
		private UIExtendedTable m_largeAdTable;

		private UIShopAdvertisementDetails m_firstActiveAd;

		private readonly BBSimpleList<UIShopAdvertisementDetails> m_createdDetails = new BBSimpleList<UIShopAdvertisementDetails>();

		private readonly List<EventDelegate> m_onAdsChanged = new List<EventDelegate>();

		public int ActivatedItemsCount { get; set; }

		public List<EventDelegate> OnAdsChanged
		{
			get
			{
				return m_onAdsChanged;
			}
		}

		public void Init()
		{
			CreateCategoryItem(EBannerType.Advertisement1X2);
			CreateCategoryItem(EBannerType.Advertisement1X1);
		}

		public GameObject GetFirstAd()
		{
			return (!(m_firstActiveAd != null)) ? null : m_firstActiveAd.gameObject;
		}

		public int UpdateAdvertisments()
		{
			m_firstActiveAd = m_createdDetails[0];
			ActivatedItemsCount = 0;
			for (int i = 0; i < m_createdDetails.Count; i++)
			{
				m_createdDetails[i].Clear();
				m_createdDetails[i].UpdateData(null);
			}
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive)
			{
				return ActivatedItemsCount;
			}
			EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
			ReadOnlyCollection<EventBase> events = eventShopAdvertisementsManager.Events;
			for (int j = 0; j < events.Count; j++)
			{
				EventShopAdvertisement eventShopAdvertisement = (EventShopAdvertisement)events[j];
				if (eventShopAdvertisement.CanShowBannerInShop() && eventShopAdvertisement.Template.IsHappyHourBannerAndAllowedToBeShown())
				{
					ActivatedItemsCount++;
					EBannerType eBannerType = ((!eventShopAdvertisement.Template.IsBigBannerPrefered) ? EBannerType.Advertisement1X1 : EBannerType.Advertisement1X2);
					UIShopAdvertisementDetails adDetailsByType = GetAdDetailsByType(eBannerType);
					adDetailsByType.UpdateAdvertisementData(eventShopAdvertisement, eBannerType);
				}
			}
			return ActivatedItemsCount;
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			m_firstActiveAd = null;
			m_onAdsChanged.Clear();
			for (int i = 0; i < m_createdDetails.Count; i++)
			{
				UIShopAdvertisementDetails uIShopAdvertisementDetails = m_createdDetails[i];
				if (uIShopAdvertisementDetails != null)
				{
					if (m_parent != null)
					{
						EventDelegate.Remove(uIShopAdvertisementDetails.OnSelected, m_parent.OnItemSelected);
					}
					UnityEngine.Object.Destroy(uIShopAdvertisementDetails);
				}
			}
			m_createdDetails.Clear();
		}

		private UIShopAdvertisementDetails GetAdDetailsByType(EBannerType _type)
		{
			for (int i = 0; i < m_createdDetails.Count; i++)
			{
				UIShopAdvertisementDetails uIShopAdvertisementDetails = m_createdDetails[i];
				if (!uIShopAdvertisementDetails.IsActive && uIShopAdvertisementDetails.LayoutType == _type)
				{
					return uIShopAdvertisementDetails;
				}
			}
			return CreateCategoryItem(_type);
		}

		private UIShopAdvertisementDetails CreateCategoryItem(EBannerType _type)
		{
			GameObject gameObject = null;
			GameObject gameObject2 = null;
			switch (_type)
			{
			case EBannerType.Advertisement1X2:
				gameObject = m_largeAdTable.gameObject;
				gameObject2 = m_advertisement1X2Prefab.gameObject;
				break;
			case EBannerType.Advertisement1X1:
				gameObject = m_smallAdContainer;
				gameObject2 = m_advertisement1X1Prefab.gameObject;
				break;
			default:
				return null;
			}
			GameObject gameObject3 = NGUITools.AddChild(gameObject, gameObject2);
			UIShopAdvertisementDetails component = gameObject3.GetComponent<UIShopAdvertisementDetails>();
			component.LayoutType = _type;
			EventDelegate.Add(component.OnSelected, m_parent.OnItemSelected);
			m_createdDetails.Add(component);
			return component;
		}

		public int AddEnterScreenTweensForLargeAdvertisements(TweenHandler _handler)
		{
			int num = 0;
			for (int i = 0; i < m_createdDetails.Count; i++)
			{
				UIShopAdvertisementDetails uIShopAdvertisementDetails = m_createdDetails[i];
				if (uIShopAdvertisementDetails.IsActive && uIShopAdvertisementDetails.LayoutType == EBannerType.Advertisement1X2)
				{
					num++;
					BBTargetedTween[] enterScreenTweens = uIShopAdvertisementDetails.EnterScreenTweens;
					for (int j = 0; j < enterScreenTweens.Length; j++)
					{
						enterScreenTweens[j].StartOrder = num;
					}
					_handler.AddTweens(enterScreenTweens, 1, false);
				}
			}
			return num;
		}

		public void AddNewCategorySelectedForLargeAdvertisements(UIShopBaseItem _selectedCategoryDetails, TweenHandler _tweenHandler)
		{
			for (int i = 0; i < m_createdDetails.Count; i++)
			{
				UIShopAdvertisementDetails uIShopAdvertisementDetails = m_createdDetails[i];
				if (uIShopAdvertisementDetails.IsActive && uIShopAdvertisementDetails.LayoutType == EBannerType.Advertisement1X2)
				{
					UIShopAdvertisementDetails component = uIShopAdvertisementDetails.GetComponent<UIShopAdvertisementDetails>();
					BBTargetedTween[] connectedTweens = ((!(_selectedCategoryDetails == component)) ? component.FadeOutTweens : component.SelectTweens);
					_tweenHandler.AddTweens(connectedTweens, 0, false);
					component.ActivateColliders(false);
				}
			}
		}

		public void UpdateLayout(UITable.OnReposition _onRepositionItemTable)
		{
			UIExtendedTable largeAdTable = m_largeAdTable;
			largeAdTable.onReposition = (UITable.OnReposition)Delegate.Combine(largeAdTable.onReposition, _onRepositionItemTable);
			m_largeAdTable.repositionNow = true;
		}
	}
}
