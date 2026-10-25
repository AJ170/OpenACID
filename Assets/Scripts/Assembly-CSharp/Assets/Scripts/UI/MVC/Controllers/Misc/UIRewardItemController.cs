using System;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.MVC.UIView.Misc;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.AssassinInventoryScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Misc
{
	public class UIRewardItemController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIRewardItemViewPresenter m_rewardItemView;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private UIInfoPopupController m_infoPopupController;

		private bool m_isHighlighted;

		private bool m_moveInfoPopupToTarget = true;

		private AcierProtocol.Assassin m_compareToAssassin;

		private Action<UIRewardItemController> m_onItemClicked;

		public ECustomItemType CustomItemType { get; private set; }

		public int CustomItemCount { get; private set; }

		public LootTemplate LootTemplate { get; private set; }

		public InventoryElement InventoryElement { get; private set; }

		public Resource Resource { get; private set; }

		public ShopBundle ShopBundle { get; private set; }

		public Quality.EQuality? ItemQuality { get; private set; }

		public ItemDef ItemDef { get; set; }

		public LootFilter LootFilter { get; set; }

		public EventProgressionChallenges.EventProgression.EventProgressionReward EventProgressionReward { get; set; }

		public MissionGoalLoot MissionGoalLoot { get; private set; }

		public bool UseLargeCoinItemTextureAsClaimReward { get; set; }

		public Action<UIRewardItemController> OnClicked
		{
			get
			{
				return m_onItemClicked;
			}
			set
			{
				m_onItemClicked = value;
			}
		}

		public bool IsHighlighted
		{
			get
			{
				return m_isHighlighted;
			}
			set
			{
				if (m_isHighlighted != value)
				{
					m_isHighlighted = value;
					m_rewardItemView.IsHighlighted = m_isHighlighted;
				}
			}
		}

		public bool MoveInfoPopupToTarget
		{
			get
			{
				return m_moveInfoPopupToTarget;
			}
			set
			{
				m_moveInfoPopupToTarget = value;
			}
		}

		public bool IsCheckMarkVisible
		{
			get
			{
				return m_rewardItemView.IsCheckMarkVisible;
			}
			set
			{
				m_rewardItemView.IsCheckMarkVisible = value;
			}
		}

		public void Close()
		{
			if (m_infoPopupController != null)
			{
				m_infoPopupController.Close();
			}
		}

		public void Initialize(int _stackSize)
		{
			ResetValues();
			base.IsInitialized = true;
			GameDB gameDB = Main.Instance.GameDB;
			SetRewardStackCount(_stackSize, string.Empty);
			ColorDef item = gameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[1].ColorGUID);
			UIItemUtil.UpdateItemByColorDef(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_itemMailGroup, item.Color0, false, null, m_rewardItemView.RewardItemTexture, false, UIItemUtil.ItemUVCoordType.ENone, false, 0f);
			Show();
		}

		public void Initialize(string _spriteName)
		{
			ResetValues();
			SetRewardStackCount(0L, string.Empty);
			if (!string.IsNullOrEmpty(_spriteName))
			{
				m_rewardItemView.RewardItemTexture.gameObject.SetActive(false);
				m_rewardItemView.BackgroundSprite.gameObject.SetActive(true);
				m_rewardItemView.BackgroundSprite.spriteName = _spriteName;
			}
			Show();
		}

		public void Initialize(InventoryElement _element, bool _showStackSize = true, bool _compareItemToCurrentAssassin = true)
		{
			ResetValues();
			base.IsInitialized = true;
			if (_compareItemToCurrentAssassin)
			{
				m_compareToAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			}
			InventoryElement = _element;
			SetRewardStackCount(_showStackSize ? InventoryElement.stackSize : 0, string.Empty);
			UIItemUtil.UpdateItem(InventoryElement, false, m_rewardItemView.RewardItemTexture, false, string.Empty, 0f);
			CompareItemToEquipedItem();
			Show();
		}

		public void Initialize(ShopBundle _shopBundle)
		{
			ResetValues();
			base.IsInitialized = true;
			ShopBundle = _shopBundle;
			SetRewardStackCount(0L, string.Empty);
			Color32 _qualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
			Color32 _textQualityColor = UIDataMapper.UITextColors.ShopMundaneOverideColor;
			UIUtil.UpdateShopItemQuality(_shopBundle, ref _qualityColor, ref _textQualityColor);
			UIItemUtil.UpdateShopItem(_shopBundle, _qualityColor, true, m_rewardItemView.RewardItemTexture, UIItemUtil.ItemUVCoordType.EShopItem, 0f);
			Show();
		}

		public void Initialize(EResourceType _resourceType, long _amount, Quality.EQuality? _quality = null)
		{
			Initialize(new Resource
			{
				amount = _amount,
				type = _resourceType
			}, _quality);
		}

		public void Initialize(Resource _resource, Quality.EQuality? _quality = null)
		{
			ResetValues();
			base.IsInitialized = true;
			Resource = _resource;
			ItemQuality = _quality;
			SetRewardStackCount(Resource.amount, string.Empty);
			Color quality = UIItemUtil.s_defaultShopItemColor;
			if (ItemQuality.HasValue)
			{
				quality = UIItemUtil.GetItemQualityColor(ItemQuality.Value);
			}
			UIItemUtil.UpdateResourceItem(_resource.type, quality, false, m_rewardItemView.RewardItemTexture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty, 0f);
			Show();
		}

		public void Initialize(EventProgressionChallenges.EventProgression.EventProgressionReward _progressionReward)
		{
			ResetValues();
			base.IsInitialized = true;
			if (_progressionReward.RewardItem.resourceTemplate != null)
			{
				Resource maxResource = _progressionReward.RewardItem.resourceTemplate.maxResource;
				Initialize(maxResource.type, maxResource.amount * _progressionReward.RewardMultiplier);
			}
			else if (_progressionReward.RewardItem.entitlementTemplate == null)
			{
				Initialize(_progressionReward.RewardItem.GetItemFromLootTemplate(_progressionReward.RewardMultiplier));
			}
			EventProgressionReward = _progressionReward;
			UseLargeCoinItemTextureAsClaimReward = true;
			Show();
		}

		public void Initialize(MissionGoalLoot _missionGoalLoot)
		{
			ResetValues();
			base.IsInitialized = true;
			if (_missionGoalLoot.resources != null && _missionGoalLoot.resources.Length > 0)
			{
				Initialize(_missionGoalLoot.resources[0].resource);
			}
			else if (_missionGoalLoot.elements != null && _missionGoalLoot.elements.elements != null && _missionGoalLoot.elements.elements.Length > 0)
			{
				Initialize(_missionGoalLoot.elements.elements[0]);
			}
			MissionGoalLoot = _missionGoalLoot;
			Show();
		}

		public void Initialize(LootTemplate _template)
		{
			ResetValues();
			base.IsInitialized = true;
			if (_template.resourceTemplate != null)
			{
				Initialize(_template.resourceTemplate.maxResource);
			}
			else if (_template.entitlementTemplate == null)
			{
				Initialize(_template.GetItemFromLootTemplate());
			}
			LootTemplate = _template;
			Show();
		}

		public void Initialize(GameID _gameId, Color32 _quality)
		{
			ResetValues();
			base.IsInitialized = true;
			ItemDef = Main.Instance.GameDB.GetItem<ItemDef>(_gameId);
			SetRewardStackCount(0L, string.Empty);
			UIItemUtil.UpdateItemByColorDef(_gameId, _quality, false, null, m_rewardItemView.RewardItemTexture, false, UIItemUtil.ItemUVCoordType.ENone, false, 0f);
			Show();
		}

		public void Initialize(ECustomItemType _customItemType, int _itemCount)
		{
			ResetValues();
			base.IsInitialized = true;
			CustomItemType = _customItemType;
			CustomItemCount = _itemCount;
			CustomItemDescription value = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CustomItemDescriptions.TryGetValue((int)_customItemType, out value))
			{
				SetRewardStackCount(_itemCount, value.SmallUnitIcon);
				Color32 shopNoQualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
				UIItemUtil.UpdateCustomItem(CustomItemType, shopNoQualityColor, false, m_rewardItemView.RewardItemTexture, 0f);
				Show();
			}
		}

		public void ClaimReward(bool _isSentToInventory, GameObject _optionalTarget = null, EventDelegate.Callback _animationFinished = null, EventDelegate.Callback _resourceMovedFinished = null)
		{
			UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
			if (Resource != null)
			{
				if (UseLargeCoinItemTextureAsClaimReward)
				{
					bool sendToInventory = _isSentToInventory;
					recivedItemMgr.StartReciveItemCoinAnimation(Resource, m_rewardItemView.gameObject, null, m_rewardItemView.RewardItemTexture, _animationFinished, _resourceMovedFinished, sendToInventory);
				}
				else
				{
					bool sendToInventory = _isSentToInventory;
					recivedItemMgr.StartReciveCoinAnimation(Resource, m_rewardItemView.gameObject, null, false, _animationFinished, _resourceMovedFinished, sendToInventory);
				}
				return;
			}
			if (_optionalTarget != null)
			{
				recivedItemMgr.StartReciveItemAnimation(m_rewardItemView.RewardItemTexture, m_rewardItemView.gameObject, _optionalTarget, null, delegate
				{
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
					if (_animationFinished != null)
					{
						_animationFinished();
					}
				});
				return;
			}
			recivedItemMgr.StartReciveItemAnimation(m_rewardItemView.RewardItemTexture, m_rewardItemView.gameObject, null, delegate
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
				if (_animationFinished != null)
				{
					_animationFinished();
				}
			}, _resourceMovedFinished, (!_isSentToInventory) ? UIButtonHeader.EHeaderType.Social : UIButtonHeader.EHeaderType.Assassin);
		}

		public void OnClickRewardItem()
		{
			if (IsHighlighted)
			{
				if (m_infoPopupController != null)
				{
					m_infoPopupController.Close();
				}
				else
				{
					IsHighlighted = false;
				}
			}
			else
			{
				IsHighlighted = true;
				if (m_infoPopupController != null)
				{
					GameObject target = m_rewardItemView.RewardItemTexture.gameObject;
					if (InventoryElement != null)
					{
						m_infoPopupController.Initialize(InventoryElement, target, m_compareToAssassin, m_moveInfoPopupToTarget);
					}
					else if (Resource != null)
					{
						m_infoPopupController.Initialize(Resource, ItemQuality, target, m_moveInfoPopupToTarget);
					}
					else if (ShopBundle != null)
					{
						m_infoPopupController.Initialize(ShopBundle, target, m_moveInfoPopupToTarget);
					}
					else if (LootFilter != null)
					{
						m_infoPopupController.Initialize(LootFilter, ItemDef, ItemQuality, target, m_moveInfoPopupToTarget);
					}
					else if (CustomItemType != 0)
					{
						m_infoPopupController.Initialize(CustomItemType, CustomItemCount, target, m_moveInfoPopupToTarget);
					}
					else
					{
						IsHighlighted = false;
					}
				}
			}
			Action<UIRewardItemController> onItemClicked = m_onItemClicked;
			if (onItemClicked != null)
			{
				m_onItemClicked(this);
			}
		}

		public override void Show()
		{
			base.Show();
			m_rewardItemView.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			m_rewardItemView.gameObject.SetActive(false);
		}

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
			if (m_infoPopupController != null)
			{
				m_infoPopupController.InfoPopup.OnBeforePopupCloseCallback += OnBeforePopupCloseCallback;
			}
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			if (m_infoPopupController != null && m_infoPopupController.InfoPopup != null)
			{
				m_infoPopupController.InfoPopup.OnBeforePopupCloseCallback -= OnBeforePopupCloseCallback;
			}
		}

		private void OnBeforePopupCloseCallback()
		{
			IsHighlighted = false;
		}

		private void SetRewardStackCount(long _count, string _spriteName = "")
		{
			m_rewardItemView.IsAmountContainerVisible = _count > 1;
			if (_count > 1)
			{
				m_rewardItemView.SetupAmount(_count, _spriteName);
			}
		}

		private void CompareItemToEquipedItem()
		{
			if (m_compareToAssassin != null && InventoryElement != null && InventoryElement.IsItem())
			{
				InventoryElement elementForCategory = m_compareToAssassin.GetElementForCategory(InventoryElement.itemData.category);
				if (elementForCategory == null)
				{
					m_rewardItemView.CompareState = CompareItemHandler.EState.Better;
					return;
				}
				int num = InventoryElement.CompareTo(elementForCategory);
				if (InventoryElement.itemData.category != elementForCategory.itemData.category)
				{
					m_rewardItemView.CompareState = CompareItemHandler.EState.Neutral;
				}
				else if (num == 0)
				{
					m_rewardItemView.CompareState = CompareItemHandler.EState.Neutral;
				}
				else
				{
					m_rewardItemView.CompareState = ((num > 0) ? CompareItemHandler.EState.Better : CompareItemHandler.EState.Worse);
				}
			}
			else
			{
				m_rewardItemView.CompareState = CompareItemHandler.EState.Neutral;
			}
		}

		private void ResetValues()
		{
			m_compareToAssassin = null;
			InventoryElement = null;
			Resource = null;
			LootTemplate = null;
			LootFilter = null;
			ItemDef = null;
			ItemQuality = null;
			EventProgressionReward = null;
			MissionGoalLoot = null;
			CustomItemType = ECustomItemType.None;
			CustomItemCount = 0;
			IsCheckMarkVisible = false;
			IsHighlighted = false;
			m_rewardItemView.CompareState = CompareItemHandler.EState.Neutral;
			m_rewardItemView.IsHighlighted = false;
			m_rewardItemView.RewardItemTexture.gameObject.SetActive(true);
			if (m_rewardItemView.BackgroundSprite != null)
			{
				m_rewardItemView.BackgroundSprite.gameObject.SetActive(false);
			}
			if (m_infoPopupController != null)
			{
				m_infoPopupController.Close();
			}
		}
	}
}
