using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIRecivedItemManager : MonoBehaviour
	{
		[SerializeField]
		private UIRecivedItem m_reciveItemPrefab;

		[SerializeField]
		private UIRecivedCoins m_reciveCoinPrefab;

		[SerializeField]
		private UIRecivedFromMysteryBox m_recivedFromMysteryBoxToSlot;

		[SerializeField]
		private UIRecivedFromMysteryBox m_recivedItemFromMysteryBoxToInventory;

		[SerializeField]
		private UIRecivedResourceFromMysteryBox m_recivedResourceFromMysteryBoxToInventory;

		private readonly List<UIRecivedItem> m_itemPool = new List<UIRecivedItem>();

		private readonly List<UIRecivedCoins> m_coinPool = new List<UIRecivedCoins>();

		private readonly List<UIRecivedFromMysteryBox> m_mysteryBoxToSlotPool = new List<UIRecivedFromMysteryBox>();

		private readonly List<UIRecivedFromMysteryBox> m_mysteryBoxItemToInventoryPool = new List<UIRecivedFromMysteryBox>();

		private readonly List<UIRecivedResourceFromMysteryBox> m_mysteryBoxResourceToInventoryPool = new List<UIRecivedResourceFromMysteryBox>();

		private List<UIRecivedBase> m_currentObjects = new List<UIRecivedBase>();

		public bool IsCoinAnimationPlaying
		{
			get
			{
				for (int i = 0; i < m_coinPool.Count; i++)
				{
					if (m_coinPool[i].gameObject.activeSelf)
					{
						return true;
					}
				}
				for (int j = 0; j < m_itemPool.Count; j++)
				{
					if (m_itemPool[j].gameObject.activeSelf && m_itemPool[j].IsResourceReward)
					{
						return true;
					}
				}
				return false;
			}
		}

		public void StartReciveCoinAnimation(Resource _resource, GameObject _startTarget, UIWidget _effectWidget, bool _showLargeCoin, EventDelegate.Callback _animationFinished = null, EventDelegate.Callback _resourceMovedFinished = null, bool _sendToInventory = true)
		{
			if (_resource != null)
			{
				UIRecivedCoins freeRecivedCoinObject = GetFreeRecivedCoinObject();
				freeRecivedCoinObject.SetSpriteData(_resource.type, _showLargeCoin);
				StartReciveAnimation(freeRecivedCoinObject, _startTarget, _resource, _effectWidget, _animationFinished, _resourceMovedFinished, true, (!_sendToInventory) ? UIButtonHeader.EHeaderType.Social : UIButtonHeader.EHeaderType.Assassin);
			}
		}

		public void StartReciveItemCoinAnimation(Resource _resource, GameObject _startTarget, UIWidget _effectWidget, UIPortraitTexture _texture, EventDelegate.Callback _animationFinished = null, EventDelegate.Callback _resourceMovedFinished = null, bool _sendToInventory = true)
		{
			if (_resource != null)
			{
				UIRecivedItem freeRecivedItemObject = GetFreeRecivedItemObject();
				freeRecivedItemObject.SetTextureData(_texture);
				StartReciveAnimation(freeRecivedItemObject, _startTarget, _resource, _effectWidget, _animationFinished, _resourceMovedFinished, true, (!_sendToInventory) ? UIButtonHeader.EHeaderType.Social : UIButtonHeader.EHeaderType.Assassin);
			}
		}

		public void StartReciveMysteryBoxToSlotAnimation(UIPortraitTexture _texture, GameObject _startTarget, GameObject _endTarget, Quality.EQuality _quality, EventDelegate.Callback _animationFinished = null)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_animmovement);
			UIRecivedFromMysteryBox freeMysteryBoxToSlot = GetFreeMysteryBoxToSlot();
			freeMysteryBoxToSlot.SetTextureData(_texture);
			StartReciveAnimation(freeMysteryBoxToSlot, _startTarget, _endTarget, _quality, _animationFinished);
		}

		public void StartMysteryBoxItemToInventoryAnimation(UIPortraitTexture _texture, GameObject _startTarget, UIWidget _effectWidget, EventDelegate.Callback _animationFinished = null, EventDelegate.Callback _resourceMovedFinished = null, bool _sendToInventory = true)
		{
			UIRecivedFromMysteryBox freeMysteryBoxItemToInventory = GetFreeMysteryBoxItemToInventory();
			freeMysteryBoxItemToInventory.SetTextureData(_texture);
			StartReciveAnimation(freeMysteryBoxItemToInventory, _startTarget, null, _effectWidget, _animationFinished, _resourceMovedFinished, true, (!_sendToInventory) ? UIButtonHeader.EHeaderType.Social : UIButtonHeader.EHeaderType.Assassin, true);
		}

		public void StartMysteryBoxResourceToInventoryAnimation(Resource _resource, GameObject _startTarget, UIWidget _effectWidget, bool _showLargeCoin, EventDelegate.Callback _animationFinished = null, EventDelegate.Callback _resourceMovedFinished = null, bool _sendToInventory = true)
		{
			if (_resource != null)
			{
				UIRecivedResourceFromMysteryBox freeMysteryBoxResourceToInventory = GetFreeMysteryBoxResourceToInventory();
				freeMysteryBoxResourceToInventory.SetSpriteData(_resource.type, true);
				StartReciveAnimation(freeMysteryBoxResourceToInventory, _startTarget, _resource, _effectWidget, _animationFinished, _resourceMovedFinished, true, (!_sendToInventory) ? UIButtonHeader.EHeaderType.Social : UIButtonHeader.EHeaderType.Assassin, true);
			}
		}

		public void StartReciveTextureAnimation(UITexture _texture, GameObject _startTarget, UIWidget _effectWidget, UIButtonHeader.EHeaderType _target, EventDelegate.Callback _animationFinished = null, EventDelegate.Callback _resourceMovedFinished = null)
		{
			UIRecivedItem freeRecivedItemObject = GetFreeRecivedItemObject();
			freeRecivedItemObject.SetTextureData(_texture);
			StartReciveAnimation(freeRecivedItemObject, _startTarget, null, _effectWidget, _animationFinished, _resourceMovedFinished, true, _target);
		}

		public void StartReciveItemAnimation(ShopBundle _shopBundle, GameObject _startTarget, UIWidget _effectWidget, EventDelegate.Callback _animationFinished = null, bool _showReceiveEffect = true, EventDelegate.Callback _resourceMovedFinished = null, bool _sendToInventory = true)
		{
			UIRecivedItem freeRecivedItemObject = GetFreeRecivedItemObject();
			freeRecivedItemObject.SetTextureData(_shopBundle);
			LootTemplate.ResourceTemplate firstResourceTemplate = _shopBundle.payout.lootTemplates.GetFirstResourceTemplate();
			Resource resource = null;
			if (firstResourceTemplate != null && _sendToInventory)
			{
				resource = firstResourceTemplate.minResource;
			}
			UIButtonHeader.EHeaderType buttonTargetFromShopBundle = GetButtonTargetFromShopBundle(_sendToInventory, _shopBundle);
			StartReciveAnimation(freeRecivedItemObject, _startTarget, resource, _effectWidget, _animationFinished, _resourceMovedFinished, _showReceiveEffect, buttonTargetFromShopBundle);
		}

		public void StartReciveItemAnimation(ShopBundle _shopBundle, LootTemplate _lootTemplate, GameObject _startTarget, UIWidget _effectWidget, EventDelegate.Callback _animationFinished = null, bool _showReceiveEffect = true, EventDelegate.Callback _resourceMovedFinished = null, bool _sendToInventory = true)
		{
			UIRecivedItem freeRecivedItemObject = GetFreeRecivedItemObject();
			freeRecivedItemObject.SetTextureData(_lootTemplate);
			Resource resource = null;
			if (_lootTemplate.resourceTemplate != null && _sendToInventory)
			{
				resource = _lootTemplate.resourceTemplate.minResource;
			}
			UIButtonHeader.EHeaderType buttonTargetFromShopBundle = GetButtonTargetFromShopBundle(_sendToInventory, _shopBundle);
			StartReciveAnimation(freeRecivedItemObject, _startTarget, resource, _effectWidget, _animationFinished, _resourceMovedFinished, _showReceiveEffect, buttonTargetFromShopBundle);
		}

		public void StartReciveItemAnimation(UIPortraitTexture _texture, GameObject _startTarget, UIWidget _effectWidget, EventDelegate.Callback _animationFinished = null, EventDelegate.Callback _resourceMovedFinished = null, UIButtonHeader.EHeaderType _headerTarget = UIButtonHeader.EHeaderType.Assassin)
		{
			UIRecivedItem freeRecivedItemObject = GetFreeRecivedItemObject();
			freeRecivedItemObject.SetTextureData(_texture);
			StartReciveAnimation(freeRecivedItemObject, _startTarget, null, _effectWidget, _animationFinished, _resourceMovedFinished, true, _headerTarget);
		}

		public void StartReciveItemAnimation(UIPortraitTexture _texture, GameObject _startTarget, GameObject _endTarget, UIWidget _effectWidget, EventDelegate.Callback _animationFinished = null)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_animmovement);
			UIRecivedItem freeRecivedItemObject = GetFreeRecivedItemObject();
			freeRecivedItemObject.SetTextureData(_texture);
			StartReciveAnimation(freeRecivedItemObject, _startTarget, _endTarget, _effectWidget, _animationFinished, true);
		}

		public void ForceStopAnimation()
		{
			for (int i = 0; i < m_currentObjects.Count; i++)
			{
				m_currentObjects[i].ForceStopAnimation();
			}
		}

		private void Awake()
		{
			CreateNewRecivedItemObject();
			CreateNewRecivedCoinObject();
			CreateNewMysteryBoxToSlotObject();
			CreateNewMysteryBoxItemToInventoryObject();
			CreateNewMysteryBoxResourceToInventoryObject();
		}

		private void OnDestroy()
		{
			base.transform.ClearChildren();
			m_itemPool.Clear();
			m_coinPool.Clear();
			m_mysteryBoxToSlotPool.Clear();
			m_mysteryBoxItemToInventoryPool.Clear();
			m_mysteryBoxResourceToInventoryPool.Clear();
		}

		private void StartReciveAnimation(UIRecivedBase _baseObject, GameObject _startTarget, Resource _resource, UIWidget _effectWidget, EventDelegate.Callback _animationFinished, EventDelegate.Callback _resourceMovedFinished, bool _showReceiveEffect, UIButtonHeader.EHeaderType _headerTarget = UIButtonHeader.EHeaderType.Assassin, bool _meetInCenter = false)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_animmovement);
			ParticleEffectsToolDynamic effectTool = null;
			if (_effectWidget != null)
			{
				effectTool = _baseObject.GetComponent<ParticleEffectsToolDynamic>();
				if (effectTool != null)
				{
					effectTool.SetWidget(_effectWidget, _showReceiveEffect);
				}
			}
			EventDelegate.Callback onFinished = CreateAnimationCallbackOrder(_baseObject, _resource, _animationFinished, _resourceMovedFinished, !_meetInCenter);
			_baseObject.StartAnimation(_startTarget, GetEndTargetFromShopBundle(_resource, _headerTarget), delegate
			{
				if (effectTool != null)
				{
					effectTool.SetWidget(null);
				}
				if (onFinished != null)
				{
					onFinished();
				}
			}, _meetInCenter);
		}

		private void StartReciveAnimation(UIRecivedBase _baseObject, GameObject _startTarget, GameObject _endTarget, UIWidget _effectWidget, EventDelegate.Callback _animationFinished, bool _showReceiveEffect)
		{
			ParticleEffectsToolDynamic effectTool = null;
			if (_effectWidget != null)
			{
				effectTool = _baseObject.GetComponent<ParticleEffectsToolDynamic>();
				if (effectTool != null)
				{
					effectTool.SetWidget(_effectWidget, _showReceiveEffect);
				}
			}
			_baseObject.StartAnimation(_startTarget, _endTarget, delegate
			{
				if (effectTool != null)
				{
					effectTool.SetWidget(null);
				}
				if (_animationFinished != null)
				{
					_animationFinished();
				}
			});
		}

		private void StartReciveAnimation(UIRecivedBase _baseObject, GameObject _startTarget, GameObject _endTarget, Quality.EQuality _quality, EventDelegate.Callback _animationFinished)
		{
			GetFreeMysteryBoxToSlot().PlayMysteryBoxEffects(_quality);
			_baseObject.StartAnimation(_startTarget, _endTarget, delegate
			{
				if (_animationFinished != null)
				{
					_animationFinished();
				}
				m_currentObjects.Remove(_baseObject);
			});
			m_currentObjects.Add(_baseObject);
		}

		private static EventDelegate.Callback CreateAnimationCallbackOrder(UIRecivedBase _baseObject, Resource _resource, EventDelegate.Callback _animationFinished, EventDelegate.Callback _resourceMovedFinished, bool _animate)
		{
			EventDelegate.Callback result = _animationFinished;
			if (_resource != null && _animate)
			{
				switch (_resource.type)
				{
				case EResourceType.HardCurrency:
				case EResourceType.SoftCurrency:
				case EResourceType.SocialCurrency:
					_baseObject.IsResourceReward = true;
					Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.RegisterWaitForExternalAnimation();
					result = delegate
					{
						if (_resourceMovedFinished != null)
						{
							_resourceMovedFinished();
						}
						UIWalletHandler walletHandler = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler;
						walletHandler.UnregisterWaitForExternalAnimation();
						walletHandler.SetupTweenCurrencyCallback(_resource.type, _resource.amount, _animationFinished);
					};
					break;
				}
			}
			return result;
		}

		private static GameObject GetEndTargetFromShopBundle(Resource _resource, UIButtonHeader.EHeaderType _headerType = UIButtonHeader.EHeaderType.Assassin)
		{
			if (_resource != null)
			{
				switch (_resource.type)
				{
				case EResourceType.HardCurrency:
					return Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.HardIcon.gameObject;
				case EResourceType.SoftCurrency:
					return Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.SoftIcon.gameObject;
				case EResourceType.SocialCurrency:
					return Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.SocialIcon.gameObject;
				}
			}
			UIButtonHeaderManager buttonHeaderManager = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager;
			if (buttonHeaderManager != null)
			{
				UIButtonHeader buttonByHeaderType = buttonHeaderManager.GetButtonByHeaderType(_headerType);
				if (buttonByHeaderType != null)
				{
					return buttonByHeaderType.CenterPlaceHolder.gameObject;
				}
			}
			return null;
		}

		private UIButtonHeader.EHeaderType GetButtonTargetFromShopBundle(bool _sendToInventory, ShopBundle _bundle)
		{
			if (_sendToInventory)
			{
				if (_bundle.IsLocationEntitlementBundle() || _bundle.IsMissionTypeEntitlementBundle() || _bundle.HasHeroicRankPayout())
				{
					return UIButtonHeader.EHeaderType.MissionsContracts;
				}
				return UIButtonHeader.EHeaderType.Assassin;
			}
			return UIButtonHeader.EHeaderType.Social;
		}

		private UIRecivedItem GetFreeRecivedItemObject()
		{
			for (int i = 0; i < m_itemPool.Count; i++)
			{
				if (!m_itemPool[i].gameObject.activeSelf)
				{
					return m_itemPool[i];
				}
			}
			return CreateNewRecivedItemObject();
		}

		private UIRecivedCoins GetFreeRecivedCoinObject()
		{
			for (int i = 0; i < m_coinPool.Count; i++)
			{
				if (!m_coinPool[i].gameObject.activeSelf)
				{
					return m_coinPool[i];
				}
			}
			return CreateNewRecivedCoinObject();
		}

		private UIRecivedFromMysteryBox GetFreeMysteryBoxToSlot()
		{
			for (int i = 0; i < m_mysteryBoxToSlotPool.Count; i++)
			{
				if (!m_mysteryBoxToSlotPool[i].gameObject.activeSelf)
				{
					return m_mysteryBoxToSlotPool[i];
				}
			}
			return CreateNewMysteryBoxToSlotObject();
		}

		private UIRecivedFromMysteryBox GetFreeMysteryBoxItemToInventory()
		{
			for (int i = 0; i < m_mysteryBoxItemToInventoryPool.Count; i++)
			{
				if (!m_mysteryBoxItemToInventoryPool[i].gameObject.activeSelf)
				{
					return m_mysteryBoxItemToInventoryPool[i];
				}
			}
			return CreateNewMysteryBoxItemToInventoryObject();
		}

		private UIRecivedResourceFromMysteryBox GetFreeMysteryBoxResourceToInventory()
		{
			for (int i = 0; i < m_mysteryBoxResourceToInventoryPool.Count; i++)
			{
				if (!m_mysteryBoxResourceToInventoryPool[i].gameObject.activeSelf)
				{
					return m_mysteryBoxResourceToInventoryPool[i];
				}
			}
			return CreateNewMysteryBoxResourceToInventoryObject();
		}

		private UIRecivedItem CreateNewRecivedItemObject()
		{
			UIRecivedItem component = NGUITools.AddChild(base.gameObject, m_reciveItemPrefab.gameObject).GetComponent<UIRecivedItem>();
			m_itemPool.Add(component);
			return component;
		}

		private UIRecivedCoins CreateNewRecivedCoinObject()
		{
			UIRecivedCoins component = NGUITools.AddChild(base.gameObject, m_reciveCoinPrefab.gameObject).GetComponent<UIRecivedCoins>();
			m_coinPool.Add(component);
			return component;
		}

		private UIRecivedFromMysteryBox CreateNewMysteryBoxToSlotObject()
		{
			UIRecivedFromMysteryBox component = NGUITools.AddChild(base.gameObject, m_recivedFromMysteryBoxToSlot.gameObject).GetComponent<UIRecivedFromMysteryBox>();
			m_mysteryBoxToSlotPool.Add(component);
			return component;
		}

		private UIRecivedFromMysteryBox CreateNewMysteryBoxItemToInventoryObject()
		{
			UIRecivedFromMysteryBox component = NGUITools.AddChild(base.gameObject, m_recivedItemFromMysteryBoxToInventory.gameObject).GetComponent<UIRecivedFromMysteryBox>();
			m_mysteryBoxItemToInventoryPool.Add(component);
			return component;
		}

		private UIRecivedResourceFromMysteryBox CreateNewMysteryBoxResourceToInventoryObject()
		{
			UIRecivedResourceFromMysteryBox component = NGUITools.AddChild(base.gameObject, m_recivedResourceFromMysteryBoxToInventory.gameObject).GetComponent<UIRecivedResourceFromMysteryBox>();
			m_mysteryBoxResourceToInventoryPool.Add(component);
			return component;
		}
	}
}
