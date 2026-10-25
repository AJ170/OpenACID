using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MysteryBoxScreen;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.EarlyAccess;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MysteryBox
{
	public class UIMysteryBoxScreen : UIScreenChild
	{
		private const string k_screenAnimation = "ScreenOpen";

		[SerializeField]
		private UnityEngine.Animation m_mainAnimaton;

		[SerializeField]
		private ChainManager m_chainManager;

		[SerializeField]
		private MysteryBoxPreviewManager m_previewManager;

		[SerializeField]
		private UIExtendedButton m_claimButton;

		[SerializeField]
		private UIExtendedButton m_lootButton;

		[SerializeField]
		private UIMysteryBoxBehaviour m_mysteryBoxBehaviour;

		[SerializeField]
		private UIExtendedLabel m_mysterBoxesLeftLabel;

		[SerializeField]
		private ParticleSystem m_starGlow;

		private MysteryBoxData m_data;

		private bool m_eventsAdded;

		private static bool IsTutorialActive
		{
			get
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25MysteryBox);
			}
		}

		public Resource OverspendingCost
		{
			get
			{
				return (m_data == null) ? null : m_data.OverspendingCost;
			}
		}

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
			EventDelegate.Add(m_claimButton.onClick, OnClickedClaimButton);
			EventDelegate.Add(m_lootButton.onClick, OnClickedLootAgain);
		}

		private void Awake()
		{
			m_eventsAdded = false;
		}

		protected override void OnScreenDestroy()
		{
			EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			EventDelegate.Remove(m_claimButton.onClick, OnClickedClaimButton);
			EventDelegate.Remove(m_lootButton.onClick, OnClickedLootAgain);
			ClearScreen();
		}

		private void OnScreenActivate()
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.DimmingBehaviour.SetDimming(true);
			}
			m_data = new MysteryBoxData();
			m_data.OnLootedAgain += new SimpleDelegate.CallbackFunction(OnLootedAgain);
			m_data.OnReset += new SimpleDelegate.CallbackFunction(OnResetData);
			m_data.OnClaimedChain += new SimpleDelegate.CallbackFunction(OnClaimed);
			m_eventsAdded = true;
			m_mysteryBoxBehaviour.Activate(m_data.Chain.mysteryBoxType, m_data.Chain.quality);
			SetupElements();
			m_chainManager.OnAllAnimationsDone += new SimpleDelegate.CallbackFunction(CheckClaimButton);
			m_chainManager.OnAllAnimationsDone += new SimpleDelegate.CallbackFunction(CheckLootMoreButton);
			if (m_mainAnimaton != null)
			{
				StartCoroutine(co_StartAnimation());
			}
		}

		private void OnScreenDeActivate()
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.DimmingBehaviour.SetDimming(false);
			}
			ClearScreen();
			m_mysteryBoxBehaviour.Deactivate();
			if (m_mainAnimaton != null)
			{
				m_mainAnimaton.Stop("ScreenOpen");
			}
			m_chainManager.OnAllAnimationsDone -= new SimpleDelegate.CallbackFunction(CheckClaimButton);
			m_chainManager.OnAllAnimationsDone -= new SimpleDelegate.CallbackFunction(CheckLootMoreButton);
			m_chainManager.CheckForRunningAnimations();
			Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr.ForceStopAnimation();
		}

		private void SetupElements()
		{
			m_chainManager.Setup(m_data, m_mysteryBoxBehaviour);
			m_previewManager.gameObject.SetActive(!IsTutorialActive);
			if (!IsTutorialActive)
			{
				m_previewManager.Setup(m_data);
			}
			UpdateOwnedMysteryBoxesLabel();
			CheckLootMoreButton();
			CheckClaimButton();
		}

		private void ClearScreen()
		{
			if (m_data != null)
			{
				m_data.OnLootedAgain -= new SimpleDelegate.CallbackFunction(OnLootedAgain);
				m_data.OnReset -= new SimpleDelegate.CallbackFunction(OnResetData);
				m_data.OnClaimedChain -= new SimpleDelegate.CallbackFunction(OnClaimed);
				m_data = null;
				m_eventsAdded = false;
			}
		}

		private void OnClickedClaimButton()
		{
			if (m_data.Chain != null)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate);
				m_data.ClaimChain();
				m_claimButton.isEnabled = false;
				m_lootButton.isEnabled = false;
			}
			else
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_mysterybox_lock);
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.InventoryScreen, new UITweenHandle(null, null));
			}
		}

		private void OnClickedLootAgain()
		{
			if (!m_data.CanReciveMysteryBoxRewards())
			{
				MysteryBoxChain chain = ((m_data.Chain == null) ? Singleton<StateManager>.Instance.CurrentChain : m_data.Chain);
				ShowInventoryAndMailIsFullPopup(OnLootAgainAllowed, chain);
			}
			else
			{
				OnLootAgainAllowed();
			}
		}

		private void OnLootAgainAllowed()
		{
			if (IsTutorialActive || m_data.Overspending)
			{
				ShowOverspendingPopup();
			}
			else
			{
				StartLootAgain();
			}
		}

		private void StartLootAgain()
		{
			m_data.LootAgain();
			m_lootButton.isEnabled = false;
			m_claimButton.isEnabled = false;
		}

		private void UpdateOwnedMysteryBoxesLabel()
		{
			BBTweenScale component = m_mysterBoxesLeftLabel.gameObject.GetComponent<BBTweenScale>();
			bool flag = IsTutorialActive || m_data.Overspending;
			if (!flag)
			{
				component.StartTweenPlayForward();
			}
			m_mysterBoxesLeftLabel.color = ((!flag) ? Color.white : Color.red);
			m_mysterBoxesLeftLabel.text = LocalizationManager.Instance.Get(1797, new StringMapper { 
			{
				"Decryption_Amount_Current",
				(!flag) ? m_data.MysteryBoxesLeft.ToString() : "0"
			} });
		}

		private void OnLootedAgain(object _param)
		{
			CheckLootMoreButton();
			UpdateOwnedMysteryBoxesLabel();
			m_chainManager.CheckForRunningAnimations();
		}

		private void OnResetData(object _param)
		{
			CheckClaimButton();
			CheckLootMoreButton();
		}

		private void OnClaimed(object _param)
		{
			m_starGlow.Play();
		}

		private void CheckLootMoreButton(object _param = null)
		{
			UIButtonLabelHandler component = m_lootButton.gameObject.GetComponent<UIButtonLabelHandler>();
			component.ResourceType = Singleton<StateManager>.Instance.TemplateManager.MysteryBoxMapping.GetEntryByQualityAndType(m_data.BoxQuality, m_data.BoxType).overspendingCost.type;
			component.ShowResource = IsTutorialActive || m_data.Overspending;
			m_lootButton.isEnabled = m_data.RolledElements.Count + 1 < m_data.OverallChainLenght || m_data.Chain == null;
		}

		private void CheckClaimButton(object _param = null)
		{
			m_claimButton.Type = ((m_data.RolledElements.Count + 1 >= m_data.OverallChainLenght && m_data.Chain != null) ? EButtonTypes.Action : EButtonTypes.Normal);
			m_claimButton.isEnabled = true;
			if (m_data.Chain == null)
			{
				m_claimButton.GetComponent<UIButtonLabelHandler>().SetText(6689);
			}
			else
			{
				m_claimButton.GetComponent<UIButtonLabelHandler>().SetText(1808);
			}
		}

		private IEnumerator co_StartAnimation()
		{
			m_mainAnimaton.SampleAnimation(0f, string.Empty);
			yield return new WaitForEndOfFrame();
			m_mainAnimaton.Play("ScreenOpen");
			m_mainAnimaton.Sample();
		}

		private void ShowOverspendingPopup()
		{
			Resource resources = m_data.OverspendingCost;
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>();
			list.Add(new UIPopupResourcesContent.ResourceConfiguration
			{
				HasEnoughResources = (IsTutorialActive || (bool)profile.HasEnoughResources(resources)),
				Resource = resources
			});
			List<UIPopupResourcesContent.ResourceConfiguration> list2 = list;
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1] { UIBuyHelper.CreateResourceCostButton(list2[0], 1780, delegate
			{
				OnLootAgain(resources);
			}) };
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = !IsTutorialActive;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new EarlyAccessContent.EarlyAccessData
			{
				ShowResources = false,
				ShowModifier = false,
				RewardIncreaseFactor = 0f,
				ResourcesCost = list2,
				ResourceSymbolAdditive = false,
				Description = LocalizationManager.Instance.Get(6688)
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void OnLootAgain(Resource _cost)
		{
			if (IsTutorialActive)
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				StartLootAgain();
			}
			else
			{
				UIBuyHelper.DetermineAllowPurches(_cost, StartLootAgain, null);
			}
		}

		public static void ShowInventoryAndMailIsFullPopup(Action _onContinueAction, EMysteryBoxType _mysteryBoxType, Quality.EQuality _quality)
		{
			UIBuyHelper.ShowInventoryAndMailIsFullPopup(_onContinueAction, _mysteryBoxType.GetInventorySpaceRequiredForClaim(_quality), true, 7803);
		}

		public static void ShowInventoryAndMailIsFullPopup(Action _onContinueAction, MysteryBoxChain _chain)
		{
			UIBuyHelper.ShowInventoryAndMailIsFullPopup(_onContinueAction, _chain.GetInventorySpaceRequiredForClaim(), true, 7803);
		}
	}
}
