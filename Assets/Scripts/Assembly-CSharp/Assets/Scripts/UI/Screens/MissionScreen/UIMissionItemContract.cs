using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public class UIMissionItemContract : UIMissionItemBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_level;

		[SerializeField]
		[CheckAssigned]
		private BBTweenColor m_levelColorTween;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_objctiveIconContainer;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_objctiveIcon;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_objectiveEliteFrame;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_headerBackground;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_lootableContainer;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_lootableItemsBox;

		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture[] m_lootableItems;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_lockedContainer;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_lockedLabel;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_refreshAnimation;

		[CheckAssigned]
		[SerializeField]
		private BBTweenPosition m_itemTweenPosition;

		private LootFilter[] m_lootFilters;

		public AcierProtocol.Mission MissionData { get; private set; }

		public UnityEngine.Animation RefreshAnimation
		{
			get
			{
				return m_refreshAnimation;
			}
		}

		private bool UpdateEntryDate { get; set; }

		public override bool IsMissionValid()
		{
			MissionCollection.RankMissions currrentRankMissions = Singleton<StateManager>.Instance.ProfileManager.CurrrentRankMissions;
			return currrentRankMissions != null && currrentRankMissions.GetMissionByID(base.ItemId) != null;
		}

		public void UpdateContractItem(AcierProtocol.Mission _mission, int _index)
		{
			base.Index = _index;
			base.ItemId = _mission.id;
			MissionData = _mission;
			base.RegionData = _mission.GetRegionDefinition();
			IsUnlocked = true;
			base.IsElite = MissionData.eliteMission;
			m_headerBackground.color = (base.IsElite ? new Color(0.5f, 0.5f, 0.5f) : Color.white);
			m_lootFilters = MissionData.missionKind.GetLootFiltersForContractMissionKind();
			UpdateEntryDate = true;
		}

		public void SetupTweenersForAnimation(bool _fadeIn)
		{
			UIWidget component = GetComponent<UIWidget>();
			float x = UIUtil.GetOffscreenPositionX(component, UIUtil.ESide.Right) * 1.2f;
			if (_fadeIn)
			{
				m_itemTweenPosition.to = Vector3.zero;
				m_itemTweenPosition.from = new Vector3(x, 0f);
			}
			else
			{
				m_itemTweenPosition.from = Vector3.zero;
				m_itemTweenPosition.to = new Vector3(x, 0f);
			}
			m_itemTweenPosition.SetTweenToBeginning();
		}

		protected override void OnDestroyItem()
		{
			MissionData = null;
		}

		protected override void SetActivated(bool _completeInstantly)
		{
			base.SetActivated(_completeInstantly);
			if (_completeInstantly)
			{
				m_levelColorTween.SetTweenToEnd();
			}
			else
			{
				m_levelColorTween.StartTweenPlayForward();
			}
		}

		protected override void SetDeActivated(bool _completeInstantly)
		{
			base.SetDeActivated(_completeInstantly);
			if (_completeInstantly)
			{
				m_levelColorTween.SetTweenToEnd();
			}
			else
			{
				m_levelColorTween.StartTweenPlayForward();
			}
		}

		protected override void UpdateTweenColors()
		{
			base.UpdateTweenColors();
			if (base.IsSelected)
			{
				m_levelColorTween.to = UIDataMapper.UITextColors.SelectedHeaderText;
				m_levelColorTween.from = ((!base.IsElite) ? UIDataMapper.UITextColors.InactiveHeaderText : UIDataMapper.UITextColors.EliteMissionTitleColor);
			}
			else
			{
				m_levelColorTween.to = ((!base.IsElite) ? UIDataMapper.UITextColors.InactiveHeaderText : UIDataMapper.UITextColors.EliteMissionTitleColor);
				m_levelColorTween.from = UIDataMapper.UITextColors.SelectedHeaderText;
			}
		}

		private void LateUpdate()
		{
			if (UpdateEntryDate)
			{
				LateUpdateContractItem();
				UpdateEntryDate = false;
			}
		}

		private void LateUpdateContractItem()
		{
			string text = MissionData.LocaObjectiveLang(base.RegionData);
			if (base.IsElite)
			{
				text = LocalizationManager.Instance.Get(8029, new StringMapper { { "elite_mission_name", text } });
			}
			m_title.text = text;
			m_level.text = string.Empty;
			UIMissionUtil.UpdateMissionIconByMissionObjectiveType(m_objctiveIcon, MissionData.missionKind, false);
			m_objctiveIconContainer.enabled = !base.IsElite;
			m_objectiveEliteFrame.enabled = base.IsElite;
			m_lockedContainer.SetActive(false);
			UpdateRewardItems();
			UpdateTexture();
		}

		private void UpdateRewardItems()
		{
			int num = 0;
			for (int i = 0; i < m_lootFilters.Length; i++)
			{
				LootFilter lootFilter = m_lootFilters[i];
				if (lootFilter.lootTypes.itemCategories == null || lootFilter.lootTypes.itemCategories.Length <= 1)
				{
					if (num >= m_lootableItems.Length)
					{
						break;
					}
					GameID itemDefPreview = lootFilter.GetItemDefPreview();
					int heroicMissionRank = Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank;
					Quality.EQuality minQualityForHeroicRank = Singleton<StateManager>.Instance.TemplateManager.GetMinQualityForHeroicRank(heroicMissionRank, base.IsElite);
					ColorDef colorDefForQuality = UIItemUtil.GetColorDefForQuality(minQualityForHeroicRank);
					UIItemUtil.UpdateItemByColorDef(itemDefPreview, colorDefForQuality.Color0, false, null, m_lootableItems[num], false, UIItemUtil.ItemUVCoordType.ENone, true, 0f);
					m_lootableItems[num].gameObject.SetActive(true);
					num++;
				}
			}
			for (int j = num; j < m_lootableItems.Length; j++)
			{
				m_lootableItems[j].gameObject.SetActive(false);
			}
			m_lootableItemsBox.RepositionNow = true;
			m_lootableContainer.gameObject.SetActive(num > 0);
		}

		private void UpdateTexture()
		{
			MissionKindDescription value = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.MissionKindDescriptions.TryGetValue((int)MissionData.missionKind, out value))
			{
				Texture2D mainTexture = (Texture2D)UnityEngine.Resources.Load(value.BackgrounPath, typeof(Texture2D));
				m_texture.mainTexture = mainTexture;
			}
		}
	}
}
