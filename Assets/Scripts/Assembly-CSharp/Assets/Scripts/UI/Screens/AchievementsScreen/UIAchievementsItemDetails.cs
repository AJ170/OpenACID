using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Achievements;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AchievementsScreen
{
	public class UIAchievementsItemDetails : MonoBehaviour
	{
		public enum EAchimentRank
		{
			Gold = 0,
			Silver = 1,
			Bronze = 2
		}

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_headerLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_infoText;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_extraAwardText;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_rewardIcon;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_rewardValue;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_rewardContainer;

		[CheckAssigned]
		[SerializeField]
		private ParticleSystem m_rewardParticle;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_awardType;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_awardProgressThumb;

		[SerializeField]
		[CheckAssigned]
		private UIProgressBar m_awardProgress;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_progressValue;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_goldFrame;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_awardContainer;

		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture m_awardItem;

		[CheckAssigned]
		[SerializeField]
		private UITexture m_awardSimpleTexture;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_newIndicator;

		[CheckAssigned]
		[SerializeField]
		private UIAchievementTexture m_achievementTexture;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_achievementUpgradeEffect;

		[CheckAssigned]
		[SerializeField]
		private ParticleEffectsToolDynamic m_claimEffect;

		[SerializeField]
		[CheckAssigned]
		private AnimateAchievementHighlight m_highLight;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_alignmentBox;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_buttonContainer;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_progressbarContainer;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_highlightFrame;

		[CheckAssigned]
		[SerializeField]
		private BBTweenScale m_selectScaleTween;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_selectHighlightTween;

		[CheckAssigned]
		[SerializeField]
		private Texture[] m_awardRankFrames;

		[CheckAssigned]
		[SerializeField]
		private UIButtonProxy m_claimButton;

		public Action<AchievementMapping.Definition> OnItemClaimed;

		private EAchimentRank m_currentRank;

		private AchievementMapping.Definition m_achievementDef;

		private AchievementDefinition m_achievementDBDef;

		private AchievementMapping.RankDefinition m_rankDef;

		private EAchievementState m_achievementState;

		private long m_unlockTimeInSeconds;

		private bool m_isUnRead;

		private AchievementMapping.Definition m_nextAchievementDef;

		private bool m_nextShowLastClaimed;

		private bool m_nextShowNewAnimation;

		private AchievementConfig m_achievementConfig;

		private IAudioHandle m_audioClaim;

		private int m_waitForAnimationCounter;

		public string Id { get; set; }

		public int Index { get; set; }

		public bool IsWaitingForClaimAnimation
		{
			get
			{
				return WaitForAnimationCounter > 0;
			}
		}

		private bool IsWaitingForClaimCallback { get; set; }

		private int WaitForAnimationCounter
		{
			get
			{
				return m_waitForAnimationCounter;
			}
			set
			{
				if (m_waitForAnimationCounter != value && value >= 0)
				{
					m_waitForAnimationCounter = value;
					if (m_waitForAnimationCounter == 0)
					{
						UpdateChildAfterAnimations(true);
					}
				}
			}
		}

		public void ShowNewAchievementAnimation()
		{
			if (m_nextAchievementDef != null || m_achievementDef == null)
			{
				m_nextShowNewAnimation = true;
				return;
			}
			m_nextShowNewAnimation = false;
			if (!UbiCore.Utils.Singleton<LocalStorage>.Instance.IsAchievementRead(m_achievementDef.id, m_rankDef.idx))
			{
				UbiCore.Utils.Singleton<LocalStorage>.Instance.SetAchievementRead(m_achievementDef.id, m_rankDef.idx);
				m_achievementUpgradeEffect.gameObject.SetActive(true);
				m_achievementUpgradeEffect.Play("AchievementUnlocked");
				m_highLight.gameObject.SetActive(true);
				StartCoroutine(m_highLight.AnimateHighlight(m_achievementConfig.EffectHighlightColor));
			}
		}

		public void UpdateChild(AchievementMapping.Definition _achievementDef, bool _showLastClaimed, bool _showNewAnimation, int _index)
		{
			Index = _index;
			m_nextAchievementDef = _achievementDef;
			m_nextShowLastClaimed = _showLastClaimed;
			m_nextShowNewAnimation = _showNewAnimation;
		}

		public void OnClaimTap()
		{
			if (m_claimButton.isEnabled)
			{
				IsWaitingForClaimCallback = true;
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = false;
				UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
				{
					Singleton<StateManager>.Instance.ProfileManager.ClaimAchievement(m_achievementDef.id, m_rankDef.idx, _asyncCallback);
				}, null, OnClaimAchievement);
			}
			else if (!IsWaitingForClaimAnimation)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4625));
			}
		}

		private void Awake()
		{
			EventDelegate.Add(m_awardProgress.onChange, OnProgressChanged);
		}

		private void OnDestroy()
		{
			if (m_awardProgress != null)
			{
				EventDelegate.Remove(m_awardProgress.onChange, OnProgressChanged);
			}
		}

		private void OnEnable()
		{
			Singleton<StateManager>.Instance.AchievementsManager.CountersUpdated += OnCounterUpdated;
			m_audioClaim = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_dailyrewards_claim, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
			m_audioClaim.Load();
		}

		private void OnDisable()
		{
			m_rankDef = null;
			m_achievementDBDef = null;
			m_achievementDef = null;
			m_nextAchievementDef = null;
			m_nextShowLastClaimed = false;
			m_nextShowNewAnimation = false;
			m_achievementConfig = null;
			m_waitForAnimationCounter = 0;
			m_awardSimpleTexture.mainTexture = null;
			m_audioClaim.Unload();
			Singleton<StateManager>.Instance.AchievementsManager.CountersUpdated -= OnCounterUpdated;
		}

		private void OnCounterUpdated()
		{
			SetProgress();
		}

		private void OnProgressChanged()
		{
			if (m_achievementState != EAchievementState.AchievementClaimed)
			{
				m_awardProgressThumb.alpha = ((m_awardProgress.value > 0f) ? 1 : 0);
			}
		}

		private void OnClaimAchievement(IAsyncResult _ar)
		{
			IsWaitingForClaimCallback = false;
			if (_ar.IsError())
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = true;
				return;
			}
			WaitForAnimationCounter = 0;
			m_claimEffect.createParticlesDynamically();
			m_audioClaim.Play();
			UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
			bool resoAnimationStarted = false;
			m_rankDef.lootTemplates.DoForResourceTemplates(delegate(LootTemplate.ResourceTemplate _template)
			{
				if (_template.minResource.amount > 0)
				{
					WaitForAnimationCounter++;
					recivedItemMgr.StartReciveCoinAnimation(_template.minResource, m_rewardIcon.gameObject, null, false, null, delegate
					{
						WaitForAnimationCounter--;
					});
					resoAnimationStarted = true;
				}
			});
			if (!resoAnimationStarted)
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = true;
			}
			if (m_awardItem.gameObject.activeInHierarchy)
			{
				WaitForAnimationCounter++;
				recivedItemMgr.StartReciveItemAnimation(m_awardItem, m_awardItem.gameObject, null, delegate
				{
					WaitForAnimationCounter--;
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
				});
			}
			else if (m_awardSimpleTexture.gameObject.activeInHierarchy)
			{
				WaitForAnimationCounter++;
				recivedItemMgr.StartReciveTextureAnimation(m_awardSimpleTexture, m_awardSimpleTexture.gameObject, null, UIButtonHeader.EHeaderType.Codex, delegate
				{
					WaitForAnimationCounter--;
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
				});
			}
			Action<AchievementMapping.Definition> onItemClaimed = OnItemClaimed;
			if (onItemClaimed != null)
			{
				onItemClaimed(m_achievementDef);
			}
		}

		private void LateUpdate()
		{
			UpdateChild();
		}

		private void UpdateChild()
		{
			if (m_nextAchievementDef == null || IsWaitingForClaimCallback)
			{
				return;
			}
			m_achievementUpgradeEffect.gameObject.SetActive(false);
			m_highLight.gameObject.SetActive(false);
			if (m_achievementDef == null || m_achievementDef.id != m_nextAchievementDef.id)
			{
				m_waitForAnimationCounter = 0;
			}
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			m_achievementDef = m_nextAchievementDef;
			m_achievementDBDef = achievementsManager.GetAchievementDefinition(m_achievementDef.id);
			SetAchievementData(m_nextShowLastClaimed);
			m_currentRank = (EAchimentRank)(m_achievementDef.ranks.Length - m_rankDef.idx);
			m_achievementConfig = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.AchievementConfigurations.TryGetValue((int)m_currentRank, out m_achievementConfig))
			{
				if (m_currentRank == EAchimentRank.Gold && m_achievementDef.ranks.Length > 1)
				{
					m_awardType.spriteName = m_achievementConfig.SingleRankSpriteName;
				}
				else
				{
					m_awardType.spriteName = m_achievementConfig.MultiRankSpriteName;
				}
			}
			SetAwardItem();
			SetProgress();
			m_headerLabel.SetLocalizationKey(m_achievementDBDef.NameLocaKey.Key);
			m_newIndicator.gameObject.SetActive(m_isUnRead);
			SetContainerState();
			if (Id != m_nextAchievementDef.id)
			{
				StartCoroutine(UpdateAwardTexture());
			}
			m_nextAchievementDef = null;
			m_nextShowLastClaimed = false;
			if (m_nextShowNewAnimation)
			{
				ShowNewAchievementAnimation();
			}
			UpdateChildAfterAnimations(false);
		}

		private void UpdateChildAfterAnimations(bool _playEffect)
		{
			m_claimButton.isEnabled = m_rankDef.lootTemplates.HasInventorySpaceForLootTemplates() && !IsWaitingForClaimAnimation;
			SetRewards(_playEffect);
		}

		private void SetAchievementData(bool _showLastClaimed)
		{
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			int id = 1;
			m_rankDef = null;
			m_isUnRead = false;
			m_unlockTimeInSeconds = 0L;
			m_achievementState = EAchievementState.AchievementNotReached;
			Achievement value = null;
			AchievementCategoryData achievementCategoryData = achievementsManager.GetAchievementCategoryData(m_achievementDef.categoryId);
			if (achievementCategoryData.IdToAchievements.TryGetValue(m_achievementDef.id, out value))
			{
				AchievementRank achievementRank = null;
				achievementRank = ((!_showLastClaimed) ? value.GetNextAvailableAchievementRank() : value.GetLastClaimedAchievementRank());
				if (achievementRank != null)
				{
					if (achievementRank.idx == m_achievementDef.GetHighestRankIdx() || _showLastClaimed || achievementRank.state != EAchievementState.AchievementClaimed)
					{
						m_achievementState = achievementRank.state;
						m_unlockTimeInSeconds = achievementRank.unlockDate;
						m_isUnRead = achievementRank.state == EAchievementState.AchievementClaimable;
						m_rankDef = m_achievementDef.GetRankDefinitionByIdx(achievementRank.idx);
					}
					else
					{
						id = achievementRank.idx + 1;
					}
				}
			}
			if (m_rankDef == null)
			{
				m_rankDef = m_achievementDef.GetRankDefinitionByIdx(id);
			}
		}

		private void SetProgress()
		{
			if (m_achievementDef != null)
			{
				AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
				long num = 0L;
				long num2 = 1L;
				ECounterType counterType = ECounterType.InvalidCounter;
				if (m_rankDef.conditions.Length > 0)
				{
					AchievementMapping.Condition condition = m_rankDef.conditions[0];
					num2 = condition.goal;
					num = (long)Mathf.Min(achievementsManager.GetCounterValue(condition.counterType), num2);
					counterType = condition.counterType;
				}
				UIAchievementGoal value = new UIAchievementGoal(num, counterType);
				UIAchievementGoal value2 = new UIAchievementGoal(num2, counterType);
				m_infoText.SetLocalizationKey(m_achievementDBDef.DescriptionLocaKey.Key, new StringMapper { { "goal", value2 } });
				if (m_achievementState == EAchievementState.AchievementClaimed)
				{
					DateTime time = ServerTimeManager.ToLocalTime(m_unlockTimeInSeconds);
					m_progressValue.SetLocalizationKey(5629, new StringMapper { 
					{
						"date",
						new UIDate(time, UIDate.EDisplayType.DateTime)
					} });
					m_awardProgress.value = 1f;
					m_awardProgressThumb.alpha = 0f;
					m_awardProgress.gameObject.SetActive(true);
					m_goldFrame.SetActive(true);
				}
				else
				{
					m_progressValue.SetLocalizationKey(5630, new StringMapper
					{
						{ "value", value },
						{ "max_value", value2 }
					});
					m_awardProgress.value = (float)num / (float)num2;
					m_awardProgressThumb.alpha = ((m_awardProgress.value > 0f) ? 1 : 0);
					m_goldFrame.SetActive(false);
				}
				m_awardProgress.gameObject.SetActive(m_awardProgress.value > 0f);
				m_awardProgress.ForceUpdate();
			}
		}

		private void SetRewards(bool _playEffect)
		{
			if (!IsWaitingForClaimAnimation && m_achievementState != EAchievementState.AchievementClaimed && m_rankDef.lootTemplates.DoForResourceTemplates(delegate(LootTemplate.ResourceTemplate _template)
			{
				CurrencyDescription value;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)_template.minResource.type, out value))
				{
					m_rewardIcon.spriteName = value.SmallIconName;
				}
				m_rewardValue.text = LocalizationManager.Instance.GetInteger(_template.minResource.amount);
			}) > 0)
			{
				m_rewardContainer.SetActive(true);
				if (_playEffect)
				{
					m_rewardParticle.Play(true);
				}
			}
			else
			{
				m_rewardContainer.SetActive(false);
			}
		}

		private void SetAwardItem()
		{
			LocalizationManager locaMgr = LocalizationManager.Instance;
			string itemName = string.Empty;
			m_awardContainer.gameObject.SetActive(false);
			m_awardItem.gameObject.SetActive(false);
			m_awardSimpleTexture.gameObject.SetActive(false);
			m_extraAwardText.UseLocalizedText = false;
			m_extraAwardText.text = string.Empty;
			if (m_rankDef.lootTemplates.DoForItemTemplateWrappers(delegate(LootTemplate _template)
			{
				int num2 = ((_template.nameKeys.Length <= 0) ? (-1) : _template.nameKeys[0]);
				itemName = locaMgr.Get(num2);
				UIItemUtil.UpdateItemByLootTemplate(_template, false, m_awardItem, 0f);
			}) > 0)
			{
				m_awardItem.gameObject.SetActive(true);
				m_awardContainer.gameObject.SetActive(true);
			}
			else if (m_rankDef.lootTemplates.DoForModuleTemplateWrappers(delegate(LootTemplate _template)
			{
				int num = ((_template.nameKeys.Length <= 0) ? (-1) : _template.nameKeys[0]);
				StringMapper _stringMapper = new StringMapper();
				_template.FillLootTemplatetringBuilder(ref _stringMapper);
				itemName = locaMgr.Get(num, _stringMapper);
				UIItemUtil.UpdateItemByLootTemplate(_template, false, m_awardItem, 0f);
			}) > 0)
			{
				m_awardItem.gameObject.SetActive(true);
				m_awardContainer.gameObject.SetActive(true);
			}
			else if (m_rankDef.lootTemplates.DoForEntitlementTemplateWrappers(delegate(LootTemplate _template)
			{
				StartCoroutine(SetupCodexTexture(_template));
			}) > 0)
			{
				itemName = LocalizationManager.Instance.Get(3804);
			}
			if (!string.IsNullOrEmpty(itemName) && m_achievementState == EAchievementState.AchievementNotReached)
			{
				m_extraAwardText.SetLocalizationKey(5663, new StringMapper { { "name", itemName } });
			}
		}

		private IEnumerator SetupCodexTexture(LootTemplate _template)
		{
			if (_template.IsCodexEntitlement())
			{
				CodexDefinition codexDef = Singleton<StateManager>.Instance.CodexManager.GetCodexDefinitionByEntitlementGuid(_template.entitlementTemplate.gameIdGuid);
				yield return Main.Instance.StartCoroutine(codexDef.Thumbnail.PrepareAsync());
				Texture texture = codexDef.Thumbnail.AccessObject<Texture>();
				m_awardSimpleTexture.mainTexture = texture;
				m_awardSimpleTexture.gameObject.SetActive(true);
				m_awardContainer.gameObject.SetActive(true);
			}
		}

		private void SetContainerState()
		{
			m_progressbarContainer.SetActive(false);
			m_extraAwardText.gameObject.SetActive(false);
			m_buttonContainer.SetActive(false);
			switch (m_achievementState)
			{
			case EAchievementState.AchievementNotReached:
			case EAchievementState.AchievementClaimed:
				m_progressbarContainer.SetActive(true);
				m_extraAwardText.gameObject.SetActive(true);
				break;
			case EAchievementState.AchievementClaimable:
				m_buttonContainer.SetActive(true);
				break;
			}
			m_alignmentBox.RepositionNow = true;
		}

		private IEnumerator UpdateAwardTexture()
		{
			int rank = (int)m_currentRank;
			LoadAnimationManager loadManager = Singleton<UIScreenManager>.Instance.SceneData.LoadAnimationManager;
			yield return StartCoroutine(loadManager.BeginLoadUnityResource(m_achievementDBDef.Icon, new LoadAnimationManager.SLoadObjectInfo(m_achievementTexture)));
			if (m_achievementDBDef == null)
			{
				yield break;
			}
			float desaturaion = 0f;
			if (m_achievementState == EAchievementState.AchievementNotReached)
			{
				if (m_rankDef.idx == 1)
				{
					rank = 1;
					desaturaion = 1f;
				}
				else
				{
					rank = Math.Min(Math.Max(rank + 1, 0), 2);
				}
			}
			m_achievementTexture.SetMaterialData(m_achievementDBDef.Icon.AccessObject<Texture>(), m_awardRankFrames[rank], desaturaion);
		}
	}
}
