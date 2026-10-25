using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Behaviours.ShopBundlePreview;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.MVC.Controllers.Assassin;
using Assets.Scripts.UI.ScreenManager.SceenPlaceholder;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UISceneData : MonoBehaviour
	{
		[CheckAssigned]
		[SerializeField]
		private UIRoot m_mainUIRoot;

		[SerializeField]
		[CheckAssigned]
		private UIPlaceholders m_uiPlaceholders;

		[CheckAssigned]
		[SerializeField]
		private Camera m_ui3DCamera;

		[CheckAssigned]
		[SerializeField]
		private Camera m_mainUICamera;

		[SerializeField]
		[CheckAssigned]
		private UIPlayerCard m_playerCard;

		[SerializeField]
		[CheckAssigned]
		private UIMysteryBoxBehaviour m_mysterBoxBehaviour;

		[SerializeField]
		[CheckAssigned]
		private EarthSpinManager m_eartSpinMgr;

		[SerializeField]
		[CheckAssigned]
		private GenericBackButton m_backButton;

		[CheckAssigned]
		[SerializeField]
		private UIRecivedItemManager m_recivedItemMgr;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_optionsButton;

		[CheckAssigned]
		[SerializeField]
		private UIOnScreenMessageBehaviour m_uiOnScreenMessage;

		[CheckAssigned]
		[SerializeField]
		private UINotificationPopupBehaviour m_notificationPopupBehaviour;

		[SerializeField]
		[CheckAssigned]
		private UIMenubars m_uiMenuBars;

		[SerializeField]
		[CheckAssigned]
		private LoadAnimationManager m_loadAnimationManager;

		[SerializeField]
		[CheckAssigned]
		private UISlantedPopup m_staticPopup;

		[SerializeField]
		[CheckAssigned]
		private UIAssassinContainer m_assassinContainer;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_fullScreenBlocker;

		[SerializeField]
		[CheckAssigned]
		private GameObject[] m_extraGamepadInteractables;

		[SerializeField]
		[CheckAssigned]
		private UIItemBundlePreview m_itemBundlePreview;

		[SerializeField]
		[CheckAssigned]
		private UIBannerBundlePreview m_eventBundlePreview;

		[SerializeField]
		[CheckAssigned]
		private UIUnlockMissionBundlePreview m_unlockMissionBundlePreview;

		[CheckAssigned]
		[SerializeField]
		private UIAssassinAndEquipmentPreviewController m_assassinAndEquipmentPreview;

		public UIBackgroundDimmingBehaviour DimmingBehaviour;

		public UIRoot MainUIRoot
		{
			get
			{
				return m_mainUIRoot;
			}
		}

		public UIPlaceholders Placeholders
		{
			get
			{
				return m_uiPlaceholders;
			}
		}

		public Camera UI3DCamera
		{
			get
			{
				return m_ui3DCamera;
			}
		}

		public UIPlayerCard PlayerCard
		{
			get
			{
				return m_playerCard;
			}
		}

		public UIMysteryBoxBehaviour MysteryBoxBehaviour
		{
			get
			{
				return m_mysterBoxBehaviour;
			}
		}

		public Camera MainUICamera
		{
			get
			{
				return m_mainUICamera;
			}
		}

		public EarthSpinManager EartSpinMgr
		{
			get
			{
				return m_eartSpinMgr;
			}
		}

		public GenericBackButton BackButton
		{
			get
			{
				return m_backButton;
			}
		}

		public UIRecivedItemManager RecivedItemMgr
		{
			get
			{
				return m_recivedItemMgr;
			}
		}

		public UIWidget OptionsButton
		{
			get
			{
				return m_optionsButton;
			}
		}

		public UIOnScreenMessageBehaviour UIOnScreenMessage
		{
			get
			{
				return m_uiOnScreenMessage;
			}
		}

		public UINotificationPopupBehaviour UINotificationPopupBehaviour
		{
			get
			{
				return m_notificationPopupBehaviour;
			}
		}

		public UIMenubars UIMenuBars
		{
			get
			{
				return m_uiMenuBars;
			}
		}

		public LoadAnimationManager LoadAnimationManager
		{
			get
			{
				return m_loadAnimationManager;
			}
		}

		public UISlantedPopup StaticPopup
		{
			get
			{
				return m_staticPopup;
			}
		}

		public UIAssassinContainer AssassinContainer
		{
			get
			{
				return m_assassinContainer;
			}
		}

		public GameObject FullScreenBlocker
		{
			get
			{
				return m_fullScreenBlocker;
			}
		}

		public GameObject[] ExtraGamepadInteractables
		{
			get
			{
				return m_extraGamepadInteractables;
			}
		}

		public UIItemBundlePreview ItemBundlePreview
		{
			get
			{
				return m_itemBundlePreview;
			}
		}

		public UIBannerBundlePreview EventBundlePreview
		{
			get
			{
				return m_eventBundlePreview;
			}
		}

		public UIUnlockMissionBundlePreview UnlockMissionBundlePreview
		{
			get
			{
				return m_unlockMissionBundlePreview;
			}
		}

		public UIAssassinAndEquipmentPreviewController AssassinAndEquipmentPreview
		{
			get
			{
				return m_assassinAndEquipmentPreview;
			}
		}

		public UIShopDetails ShopDetails { get; set; }

		private void Awake()
		{
		}
	}
}
