using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Animations.ValueObservers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Screens.AssassinInventoryScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.UI.Debriefing;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIInventoryItemView : MonoBehaviour
	{
		public enum ELockType
		{
			None = -1,
			BuyInventory = 0,
			BuyBluePrint = 1,
			Locked = 2
		}

		private const string c_reservedSlotIcon = "aUI_icon_lock_S";

		private const string c_emptyModuleIcon = "aUI_icon_boost";

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_textureContainer;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_amountLabel;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_itemAmountContainer;

		[CheckAssigned]
		[SerializeField]
		private UIPortraitTexture m_mainTexture;

		[SerializeField]
		[CheckAssigned]
		private CompareItemHandler m_compareItemHandler;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_backgroundIconSprite;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_backgroundIconFrame;

		[CheckAssigned]
		[SerializeField]
		private BBTweenAlpha m_backgroundIconTween;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_newIconSprite;

		[CheckAssigned]
		[SerializeField]
		private BBTweenAlpha m_highlightTween;

		[SerializeField]
		[CheckAssigned]
		private UIButtonProxy m_button;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_countDownSprite;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_blockerTween;

		[CheckAssigned]
		[SerializeField]
		private BoxCollider m_collider;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_contentRoot;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_nonEquipTween;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_animation;

		[CheckAssigned]
		[SerializeField]
		private UIBoolObserver m_swipeTrigger;

		[SerializeField]
		[CheckAssigned]
		private TextureOffsetAnimation m_swipeEffect;

		[CheckAssigned]
		[SerializeField]
		private UIBoolObserver m_swapEquipmentObserver;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_lockedItemAnim;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_lockedItemTween;

		private bool m_isLocked = true;

		private ELockType m_lockedType = ELockType.None;

		private bool m_selected;

		private InventoryItemBase m_currentItem;

		private InventoryItemBase m_nextItem;

		private int m_index = -1;

		private int m_nextIndex = -1;

		private bool m_initialized;

		public int Index
		{
			get
			{
				return m_index;
			}
		}

		public bool IsLocked
		{
			get
			{
				return m_isLocked;
			}
			set
			{
				m_isLocked = value;
				ReservedSlotIcon();
			}
		}

		public bool IsBuyInventory
		{
			get
			{
				return m_button.gameObject.activeSelf;
			}
		}

		public BBTweenAlpha LockedItemTween
		{
			get
			{
				return m_lockedItemTween;
			}
		}

		public BBTweenAlpha BackgroundIconTween
		{
			get
			{
				return m_backgroundIconTween;
			}
		}

		public bool Clickable
		{
			get
			{
				return m_collider.enabled;
			}
			set
			{
				m_collider.enabled = value;
			}
		}

		public bool Selected
		{
			get
			{
				return m_selected;
			}
			set
			{
				SetSelected(value);
			}
		}

		public UnityEngine.Animation Animation
		{
			get
			{
				return m_animation;
			}
		}

		public event Action<UIInventoryItemView> OnViewClicked;

		public event Action OnSwapEquipment;

		public void Init()
		{
			if (!m_initialized)
			{
				m_initialized = true;
				ResetValues();
				m_highlightTween.SetTweenToBeginning();
			}
		}

		public void UpdateView(InventoryItemBase _item, int _index)
		{
			m_currentItem = _item;
			m_index = _index;
			UpdateView();
		}

		public void UpdateView()
		{
			if (m_currentItem != null)
			{
				m_currentItem.Render(this);
			}
			m_contentRoot.gameObject.SetActive(m_currentItem != null);
			ToggleContent(m_currentItem != null);
		}

		public void Prepare(InventoryItemBase _item, int _index)
		{
			m_nextItem = _item;
			m_nextIndex = _index;
		}

		public void ToggleContent(bool _show)
		{
			m_contentRoot.alpha = ((!_show) ? 0f : 1f);
		}

		public void SetBlocked(bool _blocked, bool _instant = false)
		{
			if (_blocked)
			{
				if (_instant)
				{
					m_blockerTween.SetTweenToEnd();
				}
				else
				{
					m_blockerTween.StartTweenPlayForward();
				}
			}
			else if (_instant)
			{
				m_blockerTween.SetTweenToBeginning();
			}
			else
			{
				m_blockerTween.StartTweenPlayReverse();
			}
		}

		public void ShowNewsIcon(bool _show)
		{
			m_newIconSprite.gameObject.SetActive(_show);
		}

		public void ShowNonEquipableIcon(bool _show, bool _instant = false)
		{
			if (_show)
			{
				if (_instant)
				{
					m_nonEquipTween.SetTweenToEnd();
				}
				else
				{
					m_nonEquipTween.StartTweenPlayForward();
				}
			}
			else if (_instant)
			{
				m_nonEquipTween.SetTweenToBeginning();
			}
			else
			{
				m_nonEquipTween.StartTweenPlayReverse();
			}
		}

		public void UpdateView(InventoryElement _item)
		{
			ResetValues();
			if (_item != null)
			{
				if (_item.mysteryBoxData == null)
				{
					UIItemUtil.UpdateItem(_item, false, m_mainTexture, false, string.Empty, 0f);
				}
				else
				{
					UIItemUtil.UpdateMysteryBoxElement(_item, false, m_mainTexture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty);
				}
				m_backgroundIconFrame.gameObject.SetActive(false);
				m_textureContainer.gameObject.SetActive(true);
				CompareItemToEquipedItem(_item);
				m_amountLabel.text = LocalizationManager.Instance.GetInteger(_item.stackSize);
				m_itemAmountContainer.gameObject.SetActive(_item.stackSize > 1);
			}
		}

		public void UpdateView(Resource _resource, Quality.EQuality _quality)
		{
			ResetValues();
			UpdateResource(_resource, UIItemUtil.GetItemQualityColor(_quality));
		}

		public void UpdateView(Resource _resource)
		{
			ResetValues();
			UpdateResource(_resource, UIItemUtil.s_defaultShopItemColor);
		}

		public void UpdateView(ELockType _lock)
		{
			bool flag = m_lockedType != _lock;
			ResetValues();
			m_lockedType = _lock;
			m_mainTexture.gameObject.SetActive(_lock == ELockType.BuyBluePrint);
			m_textureContainer.gameObject.SetActive(true);
			m_button.gameObject.SetActive(_lock == ELockType.BuyInventory || _lock == ELockType.BuyBluePrint);
			m_backgroundIconFrame.gameObject.SetActive(_lock == ELockType.BuyInventory);
			m_backgroundIconSprite.gameObject.SetActive(false);
			if (_lock == ELockType.BuyBluePrint && flag)
			{
				ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintGlowColor);
				UIItemUtil.UpdateItemByColorDef(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_blueprintEmpty, colorDefForColorGuid.Color0, false, colorDefForColorGuid, m_mainTexture, false, UIItemUtil.ItemUVCoordType.ENone, false, 0f);
			}
		}

		public void UpdateFactor(float _factor)
		{
			_factor = Mathf.Clamp01(_factor);
			m_countDownSprite.enabled = _factor > 0f;
			m_countDownSprite.fillAmount = _factor;
		}

		public void EmptyItem(Item.EItemCategory _category)
		{
			ResetValues();
			ItemCategoryDescription value;
			if (_category == Item.EItemCategory.None)
			{
				m_backgroundIconFrame.gameObject.SetActive(false);
			}
			else if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions.TryGetValue((int)_category, out value))
			{
				m_backgroundIconSprite.spriteName = value.IconName;
				m_backgroundIconFrame.gameObject.SetActive(true);
				m_backgroundIconSprite.gameObject.SetActive(true);
				AdjustbackroundSprite(1f, Vector4.zero);
			}
		}

		public void SetEmptyItemIcon(Item.EItemCategory _category, float _alpha = 1f)
		{
			ItemCategoryDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions.TryGetValue((int)_category, out value))
			{
				m_backgroundIconSprite.spriteName = value.IconName;
				AdjustbackroundSprite(_alpha, Vector4.zero);
			}
		}

		public void EmptyModule()
		{
			ResetValues();
			m_backgroundIconSprite.spriteName = "aUI_icon_boost";
			AdjustbackroundSprite(1f, Vector4.zero);
			m_backgroundIconFrame.gameObject.SetActive(UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotModuleEntitlementGUID));
			m_backgroundIconSprite.gameObject.SetActive(UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotModuleEntitlementGUID));
		}

		public void SetModuleIcon()
		{
			m_backgroundIconSprite.spriteName = "aUI_icon_boost";
			AdjustbackroundSprite(1f, Vector4.zero);
		}

		public void EmptyIcon()
		{
			ResetValues();
			m_backgroundIconFrame.gameObject.SetActive(true);
			m_backgroundIconSprite.gameObject.SetActive(false);
		}

		public void ReservedSlotIcon()
		{
			m_backgroundIconSprite.spriteName = "aUI_icon_lock_S";
			AdjustbackroundSprite(0.5f, Vector4.zero);
			m_backgroundIconFrame.gameObject.SetActive(true);
			m_backgroundIconSprite.gameObject.SetActive(true);
		}

		private void AdjustbackroundSprite(float _alphaAdjust, Vector4 _anchor)
		{
			m_backgroundIconSprite.color = new Color(m_backgroundIconSprite.color.r, m_backgroundIconSprite.color.g, m_backgroundIconSprite.color.b, _alphaAdjust);
			m_backgroundIconSprite.leftAnchor.absolute = (int)_anchor.x;
			m_backgroundIconSprite.rightAnchor.absolute = (int)_anchor.y;
			m_backgroundIconSprite.bottomAnchor.absolute = (int)_anchor.z;
			m_backgroundIconSprite.topAnchor.absolute = (int)_anchor.w;
		}

		private void UpdateResource(Resource _resource, Color _color)
		{
			UIItemUtil.UpdateResourceItem(_resource.type, _color, false, m_mainTexture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty, 0f);
			m_itemAmountContainer.gameObject.SetActive(_resource.amount > 1);
			m_amountLabel.text = LocalizationManager.Instance.GetInteger(_resource.amount);
			m_textureContainer.gameObject.SetActive(true);
		}

		private void OnSwapItems()
		{
			m_currentItem = m_nextItem;
			m_index = m_nextIndex;
			UpdateView();
		}

		private void OnSwapEquipmentEvent()
		{
			if (this.OnSwapEquipment != null)
			{
				this.OnSwapEquipment();
			}
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_button.onClick, OnClick);
			m_swipeTrigger.OnActive += OnSwipeTrigger;
			m_swapEquipmentObserver.OnActive += OnSwapEquipmentEvent;
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onClick, OnClick);
			m_swipeTrigger.OnActive -= OnSwipeTrigger;
			m_swapEquipmentObserver.OnActive -= OnSwapEquipmentEvent;
			m_index = -1;
			m_nextIndex = -1;
			m_currentItem = null;
			m_nextItem = null;
		}

		private void OnClick()
		{
			if (this.OnViewClicked != null)
			{
				this.OnViewClicked(this);
			}
		}

		private void OnSwipeTrigger()
		{
			m_swipeEffect.gameObject.SetActive(true);
			m_swipeEffect.ActivateEffect();
		}

		private void ResetValues()
		{
			m_lockedType = ELockType.None;
			m_mainTexture.gameObject.SetActive(true);
			m_button.gameObject.SetActive(false);
			m_textureContainer.gameObject.SetActive(false);
			m_selected = false;
			m_newIconSprite.gameObject.SetActive(false);
			m_itemAmountContainer.gameObject.SetActive(false);
			m_backgroundIconFrame.gameObject.SetActive(false);
			m_backgroundIconSprite.gameObject.SetActive(false);
			m_compareItemHandler.State = CompareItemHandler.EState.Neutral;
			m_countDownSprite.enabled = false;
			m_blockerTween.SetTweenToBeginning();
			m_nonEquipTween.SetTweenToBeginning();
			m_lockedItemAnim.SampleAnimation(1f, string.Empty);
			m_lockedItemTween.gameObject.SetActive(false);
		}

		public void SetSelected(bool _selected, bool _instant = false)
		{
			m_selected = _selected;
			if (_selected)
			{
				if (_instant)
				{
					m_highlightTween.Sample(1f, true);
				}
				else
				{
					m_highlightTween.PlayForward();
				}
			}
			else if (_instant)
			{
				m_highlightTween.Sample(0f, true);
			}
			else
			{
				m_highlightTween.PlayReverse();
			}
		}

		private void CompareItemToEquipedItem(InventoryElement _item)
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (_item.IsItem())
			{
				InventoryElement elementForCategory = currentAssassin.GetElementForCategory(_item.itemData.category);
				if (elementForCategory == null)
				{
					m_compareItemHandler.State = CompareItemHandler.EState.Better;
					return;
				}
				int num = _item.CompareTo(elementForCategory);
				if (_item.category != elementForCategory.category)
				{
					m_compareItemHandler.State = CompareItemHandler.EState.Neutral;
				}
				else if (num == 0)
				{
					m_compareItemHandler.State = CompareItemHandler.EState.Neutral;
				}
				else
				{
					m_compareItemHandler.State = ((num > 0) ? CompareItemHandler.EState.Better : CompareItemHandler.EState.Worse);
				}
			}
			else
			{
				m_compareItemHandler.State = CompareItemHandler.EState.Neutral;
			}
		}

		public void PlaySlotUnlockAnimation(bool _playUnlockAnim)
		{
			if (_playUnlockAnim)
			{
				m_lockedItemAnim.SampleAnimation(0f, string.Empty);
				m_lockedItemAnim.PlayFirstClip();
				return;
			}
			BBTweenAlpha component = m_backgroundIconFrame.gameObject.GetComponent<BBTweenAlpha>();
			if (component != null && component.onFinished != null)
			{
				EventDelegate.Execute(component.onFinished);
			}
		}
	}
}
