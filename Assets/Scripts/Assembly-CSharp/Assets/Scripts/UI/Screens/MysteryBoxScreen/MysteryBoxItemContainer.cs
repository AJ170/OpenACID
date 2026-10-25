using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.UI.Debriefing;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MysteryBoxScreen
{
	public class MysteryBoxItemContainer : MonoBehaviour
	{
		private const int k_standardSize = 196;

		private const int k_specialSize = 256;

		[SerializeField]
		private UISprite m_arrowSprite;

		[SerializeField]
		private UIWidget m_thisWidget;

		[SerializeField]
		private BBTweenAlpha m_highlightTween;

		[SerializeField]
		private GameObject m_itemTextureContainer;

		[SerializeField]
		private UIPortraitTexture m_texture;

		[SerializeField]
		private UIPortraitTexture m_rewardItemTexture;

		[SerializeField]
		private ParticleSystem m_rewardImpact;

		[SerializeField]
		private GameObject m_impactEffect;

		[SerializeField]
		private MysteryBoxEffects m_mysteryBoxEffects;

		[SerializeField]
		private UIExtendedLabel m_itemAmountLabel;

		private MysteryBoxChain.RolledLoot m_carriedLoot;

		private Assets.Scripts.UI.Screens.MysteryBox.MysteryBoxData m_data;

		private int m_index;

		private bool m_rewardItem;

		private GameObject m_startTarget;

		private bool m_animationPlayed;

		private bool m_animationRunning;

		private UIMysteryBoxBehaviour m_mysteryBoxBehaviour;

		private MysteryBoxItemContainer m_nextContainer;

		public SimpleDelegate ClickDelegate;

		public SimpleDelegate OnAnimationFinished;

		private bool m_selected;

		private bool m_containsItem;

		public bool IsRewardItem
		{
			get
			{
				return m_rewardItem;
			}
		}

		public InventoryElement CarriedElement
		{
			get
			{
				if (m_carriedLoot != null)
				{
					return m_carriedLoot.element;
				}
				return null;
			}
		}

		public Resource CarriedResource
		{
			get
			{
				if (m_carriedLoot != null)
				{
					return m_carriedLoot.resources;
				}
				return null;
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
				if (ContainsItem)
				{
					m_selected = value;
					if (value)
					{
						m_highlightTween.PlayForward();
					}
					else
					{
						m_highlightTween.PlayReverse();
					}
				}
				else
				{
					m_highlightTween.value = 0f;
				}
			}
		}

		public bool AnimationPlayed
		{
			get
			{
				return m_animationPlayed;
			}
		}

		public bool AnimationRunning
		{
			get
			{
				return m_animationRunning;
			}
		}

		private bool ContainsItem
		{
			get
			{
				return m_containsItem;
			}
			set
			{
				m_itemTextureContainer.SetActive(value);
				m_containsItem = value;
				ArrowSpriteEnabled = !value && !m_rewardItem;
				if (m_rewardItem)
				{
					m_rewardItemTexture.gameObject.SetActive(!value);
				}
			}
		}

		private bool ArrowSpriteEnabled
		{
			get
			{
				return m_arrowSprite.enabled;
			}
			set
			{
				m_arrowSprite.enabled = value;
			}
		}

		private MysteryBoxItemContainer()
		{
			ClickDelegate = new SimpleDelegate(this, new Type[1] { typeof(MysteryBoxItemContainer) });
			OnAnimationFinished = new SimpleDelegate(this, new Type[1] { typeof(int) });
		}

		private void Awake()
		{
		}

		public void OnItemClicked()
		{
			if (ContainsItem && Singleton<StateManager>.Instance.ProfileManager.Profile.IsMysteryBoxOpen())
			{
				ClickDelegate.Trigger(base.gameObject.GetComponent<MysteryBoxItemContainer>());
			}
		}

		public void Setup(Assets.Scripts.UI.Screens.MysteryBox.MysteryBoxData _data, int _index, bool _rewardItem, GameObject _startTarget, UIMysteryBoxBehaviour _behaviour, MysteryBoxItemContainer _next)
		{
			m_data = _data;
			m_index = _index;
			m_rewardItem = _rewardItem;
			m_startTarget = _startTarget;
			m_mysteryBoxBehaviour = _behaviour;
			m_nextContainer = _next;
			int num = (IsRewardItem ? 1 : 0);
			if (m_animationRunning)
			{
				OnLootAnimationFinished();
			}
			m_animationPlayed = m_index <= UbiCore.Utils.Singleton<LocalStorage>.Instance.MysteryBoxSlotCount + num;
			m_data.OnGeneralUpdate += new SimpleDelegate.CallbackFunction(GeneralUpdate);
			m_data.OnReset += new SimpleDelegate.CallbackFunction(Reset);
			m_data.OnClaimedChain += new SimpleDelegate.CallbackFunction(ClaimedChain);
			m_data.OnLootedAgain += new SimpleDelegate.CallbackFunction(LootAgain);
			if (_rewardItem)
			{
				UIWidget thisWidget = m_thisWidget;
				int num2 = 256;
				m_thisWidget.height = num2;
				thisWidget.width = num2;
			}
			else
			{
				UIWidget thisWidget2 = m_thisWidget;
				int num2 = 196;
				m_thisWidget.height = num2;
				thisWidget2.width = num2;
			}
			ContainsItem = false;
			FillContainer();
			ArrowSpriteEnabled = !_rewardItem && !ContainsItem;
			m_rewardItemTexture.gameObject.SetActive(_rewardItem && !ContainsItem && !m_animationPlayed);
			ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)m_data.RewardQuality].ColorGUID);
			m_rewardItemTexture.RarityColor = colorDefForColorGuid.Color1;
			Selected = false;
		}

		private void GeneralUpdate(object _param)
		{
			FillContainer();
		}

		private void LootAgain(object _param)
		{
			FillContainer();
		}

		private void Reset(object _param)
		{
			ContainsItem = false;
			m_animationPlayed = false;
		}

		private void ClaimedChain(object _param)
		{
			Selected = false;
			StartCoroutine(ClaimedChainDelayed());
		}

		private IEnumerator ClaimedChainDelayed()
		{
			if (!IsRewardItem)
			{
				while (m_animationRunning)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			else
			{
				ContainsItem = true;
				m_animationPlayed = true;
				m_animationRunning = false;
				StopAllCoroutines();
				m_rewardImpact.Stop();
			}
			UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
			if (!ContainsItem)
			{
				yield break;
			}
			if (m_index < m_data.Chain.slots.Length + 1)
			{
				MysteryBoxChain.RolledLoot loot = ((!m_rewardItem) ? m_data.Chain.slots[m_index].rolledLoot[0] : m_data.Chain.bonus.rolledLoot[0]);
				if (loot.element == null)
				{
					recivedItemMgr.StartMysteryBoxResourceToInventoryAnimation(_sendToInventory: loot.inInventory, _resource: loot.resources, _startTarget: base.gameObject, _effectWidget: null, _showLargeCoin: false, _animationFinished: OnClaimAnimationFinished);
				}
				else
				{
					recivedItemMgr.StartMysteryBoxItemToInventoryAnimation(_sendToInventory: loot.inInventory, _texture: m_texture, _startTarget: base.gameObject, _effectWidget: null, _animationFinished: OnClaimAnimationFinished);
				}
			}
			ContainsItem = false;
		}

		private void OnClaimAnimationFinished()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_missionreward_currency_gained);
			m_data.ResetData();
			if (m_carriedLoot != null && !m_carriedLoot.inInventory && m_carriedLoot.resources == null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(6972), true);
			}
			m_carriedLoot = null;
		}

		private void FillContainer()
		{
			MysteryBoxChain.RolledLoot rolledLoot = null;
			m_carriedLoot = null;
			if (!m_rewardItem)
			{
				if (m_index < m_data.Chain.slots.Length)
				{
					rolledLoot = m_data.Chain.slots[m_index].rolledLoot[0];
				}
			}
			else if (m_data.Chain.bonus.rolledLoot.Length > 0)
			{
				rolledLoot = m_data.Chain.bonus.rolledLoot[0];
			}
			if (rolledLoot == null)
			{
				return;
			}
			m_carriedLoot = rolledLoot;
			if (!m_rewardItem)
			{
				if (!m_animationPlayed && !m_animationRunning)
				{
					StartCoroutine(StartItemLootEffect());
					ChooseElementAndUpdateTexture(rolledLoot);
				}
				else if (!m_animationRunning)
				{
					ChooseElementAndUpdateTexture(rolledLoot);
					ContainsItem = true;
					if (m_nextContainer.IsRewardItem)
					{
						m_nextContainer.ContainsItem = ContainsItem;
					}
				}
			}
			else
			{
				ChooseElementAndUpdateTexture(rolledLoot);
				m_carriedLoot = rolledLoot;
				ContainsItem = AnimationPlayed;
			}
		}

		private void ChooseElementAndUpdateTexture(MysteryBoxChain.RolledLoot _loot)
		{
			if (_loot.element != null)
			{
				UIItemUtil.UpdateItem(_loot.element, false, m_texture, false, string.Empty, 0f);
				m_itemAmountLabel.transform.parent.gameObject.SetActive(false);
				return;
			}
			m_itemAmountLabel.transform.parent.gameObject.SetActive(_loot.resources.amount > 0);
			m_itemAmountLabel.text = LocalizationManager.Instance.Get(5402, new StringMapper { 
			{
				"Amount",
				_loot.resources.amount
			} });
			UIItemUtil.UpdateResourceItem(_loot.resources.type, false, m_texture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty, 0f);
		}

		public void RewardItemLootEffect()
		{
			if (IsRewardItem)
			{
				StartCoroutine(DelayedRewardItemLootEffect());
			}
		}

		private IEnumerator DelayedRewardItemLootEffect()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_mysterybox_unlockfinalreward);
			m_rewardImpact.Play();
			yield return new WaitForSeconds(0.5f);
			m_mysteryBoxEffects.PlayEffect(m_data.RewardQuality);
			yield return new WaitForSeconds(0.5f);
			m_animationRunning = true;
			ContainsItem = true;
			m_animationPlayed = true;
			m_animationRunning = false;
		}

		private IEnumerator StartItemLootEffect()
		{
			m_animationRunning = true;
			yield return new WaitForEndOfFrame();
			while (m_mysteryBoxBehaviour.IsAppearing)
			{
				yield return new WaitForEndOfFrame();
			}
			m_mysteryBoxBehaviour.OpenBox(m_data.Chain.mysteryBoxType == EMysteryBoxType.MysteryBoxRessourceType, (CarriedElement == null) ? m_data.Chain.quality : CarriedElement.quality);
			yield return new WaitForEndOfFrame();
			while (m_mysteryBoxBehaviour.IsOpening)
			{
				yield return new WaitForEndOfFrame();
			}
			UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
			Quality.EQuality tempQuality = Quality.EQuality.Mundane;
			if (m_carriedLoot != null)
			{
				tempQuality = ((m_carriedLoot.element == null) ? Quality.EQuality.Mundane : m_carriedLoot.element.quality);
			}
			recivedItemMgr.StartReciveMysteryBoxToSlotAnimation(m_texture, m_startTarget, base.gameObject, tempQuality, OnLootAnimationFinished);
		}

		private void OnLootAnimationFinished()
		{
			ContainsItem = true;
			m_animationPlayed = true;
			m_animationRunning = false;
			m_mysteryBoxBehaviour.CloseBox();
			m_impactEffect.SetActive(true);
			m_impactEffect.GetComponent<TextureOffsetAnimation>().ActivateEffect();
			if (m_nextContainer != null && m_nextContainer.IsRewardItem)
			{
				m_nextContainer.RewardItemLootEffect();
			}
			UbiCore.Utils.Singleton<LocalStorage>.Instance.MysteryBoxSlotCount = m_index;
			OnAnimationFinished.Trigger(m_index);
			if (CarriedResource != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.SetupTweenCurrencyCallback(CarriedResource.type, CarriedResource.amount, null);
			}
		}

		private void RemoveListeners()
		{
			m_data.OnGeneralUpdate -= new SimpleDelegate.CallbackFunction(GeneralUpdate);
			m_data.OnReset -= new SimpleDelegate.CallbackFunction(Reset);
			m_data.OnClaimedChain -= new SimpleDelegate.CallbackFunction(ClaimedChain);
			m_data.OnLootedAgain -= new SimpleDelegate.CallbackFunction(LootAgain);
		}

		private void PlayAnimation()
		{
		}

		private void OnDestroy()
		{
			RemoveListeners();
		}
	}
}
