using System.Collections;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public class UIMissionItemCampaigns : UIMissionItemBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_lockedText;

		[CheckAssigned]
		[SerializeField]
		private UnityEngine.Animation m_unlockMissionAnimation;

		private bool m_waitignForTextureLoad;

		private bool m_unlockAnimationStarted;

		public CampaignDefinition CampaignDefinition { get; private set; }

		public CampaignCatalog.Campaign Campaign { get; private set; }

		private bool UpdateEntryDate { get; set; }

		public override bool IsUnlocked
		{
			get
			{
				return m_isUnlocked;
			}
			protected set
			{
				m_isUnlocked = value;
			}
		}

		protected override void OnAwake()
		{
		}

		public void UpdateCampaignData(CampaignCatalog.Campaign _campaign, int _index)
		{
			base.Index = _index;
			base.ItemId = _campaign.id;
			Campaign = _campaign;
			CampaignDefinition = _campaign.GetCampaignDefinition();
			base.RegionData = _campaign.GetRegionDefinitionForCampaign();
			IsUnlocked = Campaign.IsCampaignUnlocked() && Campaign.HasUnlockAnimationBeenPlayed();
			m_unlockAnimationStarted = false;
			UpdateEntryDate = true;
			m_unlockMissionAnimation.SampleAnimation(IsUnlocked ? 1 : 0, string.Empty);
		}

		public void ScheduleUnlockStatusChanged()
		{
			if (Campaign.IsCampaignUnlocked() && !Campaign.HasUnlockAnimationBeenPlayed() && !m_unlockAnimationStarted)
			{
				StartCoroutine(OnPlayUnlockAnimation());
			}
		}

		protected override void OnDestroyItem()
		{
			base.RegionData = null;
		}

		private void OnEnable()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
		}

		private void OnDisable()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate -= new SimpleDelegate.CallbackFunction(OnProfileUpdate);
		}

		private void LateUpdate()
		{
			if (UpdateEntryDate)
			{
				LateUpdateCampaignData();
				UpdateEntryDate = false;
			}
		}

		private bool IsAllowedToStartUnlockAnimation()
		{
			return !m_waitignForTextureLoad && UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.HasCompleted() && !Singleton<UIScreenManager>.Instance.SceneData.UnlockMissionBundlePreview.gameObject.activeInHierarchy;
		}

		private void LateUpdateCampaignData()
		{
			m_title.SetLocalizationKey(CampaignDefinition.NameLocaKey.Key);
			if (!IsUnlocked)
			{
				bool active = Campaign.GetShopItemsByCampaignId().Count > 0;
				m_lockedText.gameObject.SetActive(active);
				m_lockedText.SetLocalizationKey(8369);
			}
			StopAllCoroutines();
			StartCoroutine(UpdateTexture());
		}

		private void OnProfileUpdate(object[] _params)
		{
			ScheduleUnlockStatusChanged();
		}

		private IEnumerator OnPlayUnlockAnimation()
		{
			m_unlockAnimationStarted = true;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			if (!IsAllowedToStartUnlockAnimation())
			{
				yield return wait;
			}
			m_unlockMissionAnimation.Play();
			for (int i = 0; i < Campaign.reqEntitlements.Length; i++)
			{
				UbiCore.Utils.Singleton<LocalStorage>.Instance.SetEntitlementShown(Campaign.reqEntitlements[i]);
			}
			IsUnlocked = Campaign.IsCampaignUnlocked();
			m_waitignForTextureLoad = false;
		}

		private IEnumerator UpdateTexture()
		{
			m_waitignForTextureLoad = true;
			UnityResource resource = CampaignDefinition.Banner;
			LoadAnimationManager loadManager = Singleton<UIScreenManager>.Instance.SceneData.LoadAnimationManager;
			yield return StartCoroutine(loadManager.BeginLoadUnityResource(resource, new LoadAnimationManager.SLoadObjectInfo(m_texture)));
			m_texture.mainTexture = CampaignDefinition.Banner.AccessObject<Texture>();
			m_waitignForTextureLoad = false;
			ScheduleUnlockStatusChanged();
		}
	}
}
