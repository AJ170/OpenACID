using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.Shop;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.ShopBundlePreview
{
	public class UIBaseBundlePreview : MonoBehaviour
	{
		protected const string c_openAnimationName = "ItemBundleOpen";

		protected const string c_closeAnimationName = "ItemBundleClose";

		protected static Color32 s_defaultbackgroundColor = new Color32(68, 136, 162, byte.MaxValue);

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		protected UIInfoPopup m_infoPopup;

		[CheckAssigned(NullAllowed = true)]
		[SerializeField]
		protected UIWidget m_infoPopupPlaceHolder;

		[SerializeField]
		[CheckAssigned]
		protected UnityEngine.Animation m_mainAnimation;

		[CheckAssigned]
		[SerializeField]
		protected UIWidget m_mainContainer;

		[CheckAssigned]
		[SerializeField]
		protected UIAlignmentBox m_alignmentBoxMain;

		[CheckAssigned]
		[SerializeField]
		protected UISprite m_background;

		[SerializeField]
		[CheckAssigned]
		protected UISprite m_headerBackground;

		[SerializeField]
		[CheckAssigned]
		protected UIExtendedLabel m_headline;

		[CheckAssigned]
		[SerializeField]
		protected UIExtendedLabel m_description;

		[CheckAssigned]
		[SerializeField]
		protected UIExtendedLabel m_categoryDescription;

		[CheckAssigned]
		[SerializeField]
		protected UIWidget m_resourceContainer;

		[CheckAssigned]
		[SerializeField]
		protected UIAlignmentBox m_resourceCostBox;

		[SerializeField]
		[CheckAssigned]
		protected GameObject m_oldPriceContainer;

		[CheckAssigned]
		[SerializeField]
		protected UIExtendedLabel m_oldPrice;

		[SerializeField]
		[CheckAssigned]
		protected UISprite m_oldPriceLine;

		[SerializeField]
		[CheckAssigned]
		protected UIAlignmentBox m_alignmentBoxNewPrice;

		[CheckAssigned]
		[SerializeField]
		protected UIExtendedLabel m_newPrice;

		[SerializeField]
		[CheckAssigned]
		protected UIExtendedLabel m_prePriceText;

		[SerializeField]
		[CheckAssigned]
		protected UISprite m_newPriceCurrencyIcon;

		[CheckAssigned]
		[SerializeField]
		protected GameObject m_discountContainer;

		[CheckAssigned]
		[SerializeField]
		protected UIExtendedLabel m_discountValue;

		[CheckAssigned]
		[SerializeField]
		protected UIButtonProxy m_actionButton;

		[SerializeField]
		[CheckAssigned]
		protected UIWidget m_timerContainer;

		[CheckAssigned]
		[SerializeField]
		protected UIExtendedLabel m_timerText;

		protected long m_availableEndTimeValue;

		protected DateTime? m_availableEndTimeInDateTime;

		protected string m_overrideTitle;

		protected string m_overrideDescription;

		protected int m_waitForAlignmentCounter;

		public List<EventDelegate> OnCloseButton = new List<EventDelegate>();

		public List<EventDelegate> OnAcceptedButton = new List<EventDelegate>();

		public event Action OnPreviewOpenCallback;

		public event Action OnPreviewCloseCallback;

		public void OnCloseButtonUnity()
		{
			OnClosePopup();
		}

		public void OnActionButtonUnity()
		{
			OnActionButton();
		}

		protected virtual void OnClosePopup()
		{
			if (base.gameObject.activeSelf)
			{
				Action onPreviewCloseCallback = this.OnPreviewCloseCallback;
				if (onPreviewCloseCallback != null)
				{
					onPreviewCloseCallback();
				}
				if (m_infoPopup != null)
				{
					m_infoPopup.Close();
				}
				StopAllCoroutines();
				StartCoroutine(OnStartAnimation("ItemBundleClose", delegate
				{
					base.gameObject.SetActive(false);
				}));
				EventDelegate.Execute(OnCloseButton);
			}
		}

		protected virtual void OnActionButton()
		{
		}

		protected virtual void OnUnityAwake()
		{
		}

		protected virtual void OnUnityEnable()
		{
		}

		protected virtual void OnUnityDisable()
		{
		}

		protected virtual void OnUnityDestroy()
		{
		}

		protected virtual void OnUnityUpdate()
		{
		}

		protected virtual void OnStartShopItemPurchased(bool _wasSentToInventory)
		{
			EventDelegate.Execute(OnAcceptedButton);
		}

		protected virtual void OnBeforePopupCloseCallback()
		{
		}

		protected void OnAnimationFinished()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
		}

		protected virtual void UpdatePreview()
		{
			ResetInfoPoup();
			m_waitForAlignmentCounter = 0;
		}

		protected void ExecuteOnOpenPreview()
		{
			Action onPreviewOpenCallback = this.OnPreviewOpenCallback;
			if (onPreviewOpenCallback != null)
			{
				onPreviewOpenCallback();
			}
		}

		protected void UpdateCurrency(int _oldPrice, Resource _newPrice, string _preText)
		{
			bool flag = _oldPrice > 0;
			bool flag2 = _newPrice != null;
			m_discountContainer.gameObject.SetActive(flag);
			m_oldPriceContainer.gameObject.SetActive(flag);
			m_alignmentBoxNewPrice.gameObject.SetActive(flag2);
			m_resourceContainer.gameObject.SetActive(flag2);
			if (flag2)
			{
				string spriteName = string.Empty;
				CurrencyDescription value = null;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)_newPrice.type, out value))
				{
					spriteName = value.SmallIconName;
				}
				m_newPrice.text = LocalizationManager.Instance.GetInteger(_newPrice.amount);
				m_newPriceCurrencyIcon.spriteName = spriteName;
				m_newPriceCurrencyIcon.gameObject.SetActive(true);
				if (flag)
				{
					m_prePriceText.UseLocalizedText = false;
					m_prePriceText.text = ((!string.IsNullOrEmpty(_preText)) ? LocalizationManager.Instance.Get(7755) : _preText);
					m_discountValue.text = LocalizationManager.Instance.Get(1785, new StringMapper { 
					{
						"value",
						Math.Abs(Math.Round((1f - (float)_newPrice.amount / (float)_oldPrice) * 100f))
					} });
					m_oldPrice.text = LocalizationManager.Instance.GetInteger(_oldPrice);
					m_oldPrice.color = UIDataMapper.UITextColors.DiscountOldPriceColorBright;
					Vector3[] localCorners = m_oldPrice.localCorners;
					Vector2 vector = localCorners[0] - localCorners[2];
					float z = Mathf.Atan2(vector.y, vector.x) * 180f / (float)Math.PI;
					m_oldPriceLine.width = (int)Mathf.Sqrt(m_oldPrice.width * m_oldPrice.width + m_oldPrice.height * m_oldPrice.height);
					m_oldPriceLine.transform.localRotation = Quaternion.Euler(0f, 0f, z);
					m_oldPriceLine.transform.localPosition = new Vector3(-m_oldPrice.width / 2, 0f);
					m_oldPriceLine.color = UIDataMapper.UITextColors.DiscountCrossOutLineColorBright;
				}
				m_alignmentBoxNewPrice.RepositionNow = true;
				m_resourceCostBox.RepositionNow = true;
			}
		}

		protected virtual void ResetInfoPoup()
		{
			if (m_infoPopup != null)
			{
				m_infoPopup.IsEnabled = false;
			}
			m_background.color = s_defaultbackgroundColor;
		}

		protected void OpenPreview()
		{
			ExecuteOnOpenPreview();
			UpdatePreview();
			base.gameObject.SetActive(true);
			StopAllCoroutines();
			StartCoroutine(OnStartAnimation("ItemBundleOpen"));
		}

		protected void OnAlignmentBoxReposition()
		{
			m_waitForAlignmentCounter--;
			if (m_waitForAlignmentCounter <= 0)
			{
				m_alignmentBoxMain.RepositionNow = true;
			}
		}

		protected void Update()
		{
			double num = 0.0;
			if (m_availableEndTimeValue > 0)
			{
				DateTime? availableEndTimeInDateTime = m_availableEndTimeInDateTime;
				if (availableEndTimeInDateTime.HasValue)
				{
					DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
					DateTime? availableEndTimeInDateTime2 = m_availableEndTimeInDateTime;
					num = (availableEndTimeInDateTime2.Value - utcNow).TotalSeconds;
					if (num <= 0.0)
					{
						UpdatePreview();
						return;
					}
				}
			}
			if (num > 0.0)
			{
				m_timerText.text = UIDuration.FromSeconds((long)num).ToString();
			}
			m_timerContainer.gameObject.SetActive(num > 0.0);
			OnUnityUpdate();
		}

		private void Awake()
		{
			if (m_infoPopup != null)
			{
				m_infoPopup.Close();
				m_infoPopup.OnBeforePopupCloseCallback += OnBeforePopupCloseCallback;
			}
			OnUnityAwake();
		}

		private void OnEnable()
		{
			OnUnityEnable();
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage).OnChanged += new SimpleDelegate.CallbackFunction(OnEventUpdatePopup);
			eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage).OnChanged += new SimpleDelegate.CallbackFunction(OnEventUpdatePopup);
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable && Singleton<UIScreenManager>.Instance.SceneData.ShopDetails != null)
			{
				UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
				if (buyShopItemHelper != null)
				{
					buyShopItemHelper.OnPurchasedEvent += new SimpleDelegate.CallbackFunction(OnShopItemPurchased);
				}
			}
		}

		private void OnDisable()
		{
			OnUnityDisable();
			m_overrideTitle = string.Empty;
			m_overrideDescription = string.Empty;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			if (eventManager != null)
			{
				eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage).OnChanged -= new SimpleDelegate.CallbackFunction(OnEventUpdatePopup);
				eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage).OnChanged -= new SimpleDelegate.CallbackFunction(OnEventUpdatePopup);
			}
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable && Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.ShopDetails != null)
			{
				UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
				if (buyShopItemHelper != null)
				{
					buyShopItemHelper.OnPurchasedEvent -= new SimpleDelegate.CallbackFunction(OnShopItemPurchased);
				}
			}
		}

		private void OnDestroy()
		{
			OnUnityDestroy();
			if (m_infoPopup != null)
			{
				m_infoPopup.OnBeforePopupCloseCallback -= OnBeforePopupCloseCallback;
			}
		}

		private void OnEventUpdatePopup(object[] _params)
		{
			UpdatePreview();
		}

		private void OnShopItemPurchased(object[] _params)
		{
			bool wasSentToInventory = (bool)_params[1];
			OnStartShopItemPurchased(wasSentToInventory);
			OnClosePopup();
		}

		private IEnumerator OnStartAnimation(string _name, Action _callback = null)
		{
			m_mainAnimation.SampleAnimation(0f, _name);
			m_mainAnimation.Play(_name);
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (m_mainAnimation.isPlaying)
			{
				yield return wait;
			}
			if (_callback != null)
			{
				_callback();
			}
		}
	}
}
