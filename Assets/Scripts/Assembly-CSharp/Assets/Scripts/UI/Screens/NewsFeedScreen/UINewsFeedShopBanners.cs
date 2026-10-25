using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.UI.MVC.Controllers.Banner;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.TweenHandling;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.NewsFeedScreen
{
	public class UINewsFeedShopBanners : MonoBehaviour
	{
		[SerializeField]
		[CheckAssigned]
		private GameObject m_shopBanner1X1Templet;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_shopBanner1X2Templet;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_eventBanner1X1Templet;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_eventBanner1X2Templet;

		[SerializeField]
		[CheckAssigned]
		private UIRect m_anchorTargetRect;

		[CheckAssigned]
		[SerializeField]
		private UITable m_bannerTable;

		[SerializeField]
		[CheckAssigned]
		private UIScrollView m_scrollview;

		[CheckAssigned]
		[SerializeField]
		private BBTargetedTween m_refTween;

		private int m_bannersAdded;

		private int m_shopBanner1x1Height;

		private int m_shopBanner1x2Height;

		private int m_eventBanner1x1Height;

		private int m_eventBanner1x2Height;

		public bool HasBannersToShow
		{
			get
			{
				EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
				EventProgressionChallenges firstValidEventProgressionEvent = eventProgressionChallengesManager.GetFirstValidEventProgressionEvent();
				if (firstValidEventProgressionEvent != null)
				{
					return true;
				}
				EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
				return eventShopAdvertisementsManager.HasActiveNewsFeedAdvertisements();
			}
		}

		public void UpdateFeedBanners(float _tweenDistance, TweenHandler _tweenHandler)
		{
			m_bannerTable.transform.ClearChildren();
			m_anchorTargetRect.ForceUpdate(true);
			m_scrollview.InvalidateBounds();
			m_bannersAdded = 0;
			int _spaceAvailable = (int)Mathf.Max(m_scrollview.bounds.size.y, GetBannerTypeSpace(EBannerType.Advertisement1X2));
			CreateProgressionChallengeBanners(ref _spaceAvailable);
			CreateShopAdBanners(ref _spaceAvailable);
			int childCount = m_bannerTable.transform.childCount;
			bool active = childCount > 0;
			m_bannerTable.gameObject.SetActive(active);
			m_bannerTable.RepositionOption = UITable.ELockReposition.DontMoveXAxis;
			m_bannerTable.Reposition();
			for (int i = 0; i < m_bannerTable.Children.Count; i++)
			{
				Transform transform = m_bannerTable.Children[i];
				BBTweenPosition bBTweenPosition = transform.gameObject.AddComponentIfNotExist<BBTweenPosition>();
				bBTweenPosition.CopyFrom(m_refTween);
				bBTweenPosition.SetEndToCurrentValue();
				bBTweenPosition.from = new Vector3(bBTweenPosition.to.x - _tweenDistance, 0f);
				bBTweenPosition.enabled = false;
				bBTweenPosition.StartOrder = i;
				_tweenHandler.AddTweens(new BBTweenPosition[1] { bBTweenPosition }, 1, false);
			}
		}

		private void Awake()
		{
			m_shopBanner1x1Height = m_shopBanner1X1Templet.GetComponent<UIWidget>().height;
			m_shopBanner1x2Height = m_shopBanner1X2Templet.GetComponent<UIWidget>().height;
			m_eventBanner1x1Height = m_eventBanner1X1Templet.GetComponent<UIWidget>().height;
			m_eventBanner1x2Height = m_eventBanner1X2Templet.GetComponent<UIWidget>().height;
		}

		private void OnDestroy()
		{
			if (m_bannerTable != null && m_bannerTable.transform != null)
			{
				m_bannerTable.transform.ClearChildren();
			}
		}

		private void CreateProgressionChallengeBanners(ref int _spaceAvailable)
		{
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			EventProgressionChallenges firstValidEventProgressionEvent = eventProgressionChallengesManager.GetFirstValidEventProgressionEvent();
			if (firstValidEventProgressionEvent != null)
			{
				EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
				EBannerType type = EBannerType.EventBanner1X2;
				if (eventShopAdvertisementsManager.HasActiveNewsFeedAdvertisements())
				{
					type = EBannerType.EventBanner1X1;
				}
				CreateBanner(type, firstValidEventProgressionEvent, ref _spaceAvailable);
			}
		}

		private void CreateShopAdBanners(ref int _spaceAvailable)
		{
			bool flag = GetBannersToShowCount() > 1;
			EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
			ReadOnlyCollection<EventBase> events = eventShopAdvertisementsManager.Events;
			for (int i = 0; i < events.Count; i++)
			{
				EventShopAdvertisement eventShopAdvertisement = (EventShopAdvertisement)events[i];
				if (eventShopAdvertisement.CanShowBannerInNewsFeed() && eventShopAdvertisement.Template.IsHappyHourBannerAndAllowedToBeShown())
				{
					EBannerType type = EBannerType.Advertisement1X2;
					if (flag || !eventShopAdvertisement.Template.IsBigBannerPrefered)
					{
						type = EBannerType.Advertisement1X1;
					}
					CreateBanner(type, eventShopAdvertisement, ref _spaceAvailable);
					if (_spaceAvailable < m_shopBanner1x1Height)
					{
						break;
					}
				}
			}
		}

		private static int GetBannersToShowCount()
		{
			int num = 0;
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			EventProgressionChallenges firstValidEventProgressionEvent = eventProgressionChallengesManager.GetFirstValidEventProgressionEvent();
			if (firstValidEventProgressionEvent != null)
			{
				num++;
			}
			EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
			return num + eventShopAdvertisementsManager.GetNumberOfActiveBannersInNewsFeed();
		}

		private void CreateBanner(EBannerType _type, EventBase _baseEvent, ref int _spaceAvailable)
		{
			int bannerTypeSpace = GetBannerTypeSpace(_type);
			if (bannerTypeSpace <= _spaceAvailable)
			{
				UIBannerBaseController uIBannerBaseController = CreateCategoryItem(_type);
				uIBannerBaseController.UpdateBannerData(_baseEvent);
				uIBannerBaseController.InstantShowItem();
				m_bannersAdded++;
				_spaceAvailable -= bannerTypeSpace;
			}
		}

		private int GetBannerTypeSpace(EBannerType _type)
		{
			int num = ((m_bannersAdded > 0) ? ((int)m_bannerTable.padding.y) : 0);
			switch (_type)
			{
			case EBannerType.Advertisement1X2:
				num += m_shopBanner1x2Height;
				break;
			case EBannerType.Advertisement1X1:
				num += m_shopBanner1x1Height;
				break;
			case EBannerType.EventBanner1X2:
				num += m_eventBanner1x2Height;
				break;
			case EBannerType.EventBanner1X1:
				num += m_eventBanner1x1Height;
				break;
			}
			return num;
		}

		private UIBannerBaseController CreateCategoryItem(EBannerType _type)
		{
			GameObject gameObject = null;
			switch (_type)
			{
			case EBannerType.Advertisement1X2:
				gameObject = m_shopBanner1X2Templet.gameObject;
				break;
			case EBannerType.Advertisement1X1:
				gameObject = m_shopBanner1X1Templet.gameObject;
				break;
			case EBannerType.EventBanner1X2:
				gameObject = m_eventBanner1X2Templet.gameObject;
				break;
			case EBannerType.EventBanner1X1:
				gameObject = m_eventBanner1X1Templet.gameObject;
				break;
			default:
				return null;
			}
			GameObject gameObject2 = NGUITools.AddChild(m_bannerTable.gameObject, gameObject);
			UIBannerBaseController component = gameObject2.GetComponent<UIBannerBaseController>();
			component.LayoutType = _type;
			UIWidget component2 = component.GetComponent<UIWidget>();
			component2.leftAnchor.Set(m_anchorTargetRect.transform, 0f, 0f);
			component2.rightAnchor.Set(m_anchorTargetRect.transform, 1f, 0f);
			component2.ResetAnchors();
			component2.UpdateAnchors();
			component2.ForceUpdate(true);
			return component;
		}
	}
}
