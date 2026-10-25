using System.Collections;
using Assets.Scripts.Database;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	internal class UITutorialResultBehaviour : UIBaseResultBehaviour
	{
		[SerializeField]
		private UITexture m_TutorialContextImage;

		[SerializeField]
		private UILabel m_TutorialCongratsText;

		[SerializeField]
		private UILabel m_continueButtonText;

		[SerializeField]
		private UILabel m_retryButtonText;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.TutorialResult;
			}
		}

		protected override string GetAnimationName()
		{
			return "TutorialResultMain";
		}

		protected override void ValidateFields()
		{
			base.ValidateFields();
		}

		protected override void UpdateMissionRelatedData()
		{
			RegionDefinition regionDefinition = m_missionDescription.GetRegionDefinition();
			m_missionName.text = m_missionDescription.LocaName(regionDefinition);
			m_TutorialCongratsText.text = m_missionDescription.LocaDebriefing();
		}

		public override void Init()
		{
			ValidateFields();
			base.Init();
			if (!Singleton<StateManager>.Instance.MissionSelectionManager.HaveMissionAfterTutorial)
			{
				EventDelegate.Remove(m_nextMissionButton.onClick, base.OnTapNextMission);
				EventDelegate.Add(m_nextMissionButton.onClick, OnTapReturnToAnimus);
			}
		}

		public override void SetMissionData(bool _missionCompleted, MissionRootData _missionDescription)
		{
			base.SetMissionData(_missionCompleted, _missionDescription);
			m_missionCompleted = _missionCompleted;
			m_missionDescription = _missionDescription;
			m_retryButton.gameObject.SetActive(true);
			m_nextMissionButton.gameObject.SetActive(true);
			int num = 6543;
			if (Singleton<StateManager>.Instance.MissionSelectionManager.CampaignMissionAfterTutorial != null)
			{
				num = 677;
			}
			else if (Singleton<StateManager>.Instance.MissionSelectionManager.ContractMissionAfterTutorial != null)
			{
				num = 677;
			}
			m_continueButtonText.text = LocalizationManager.Instance.Get(num);
			m_retryButtonText.text = LocalizationManager.Instance.Get(7199);
			UpdateMissionRelatedData();
		}

		public override void SetVisibility(bool _visible)
		{
			base.SetVisibility(_visible);
			if (_visible)
			{
				Show();
			}
		}

		public void Show()
		{
			StartCoroutine(PlayMainAnimation());
			StartCoroutine(UpdateTexture());
		}

		private IEnumerator UpdateTexture()
		{
			m_TutorialContextImage.mainTexture = null;
			yield return null;
			string imagePath = string.Empty;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.TutorialMissionDescription.TryGetValue(m_missionDescription.BaseName, out imagePath) && !string.IsNullOrEmpty(imagePath))
			{
				Texture2D textureAtlas = (Texture2D)Resources.Load(imagePath, typeof(Texture2D));
				m_TutorialContextImage.mainTexture = textureAtlas;
			}
		}

		private void Destroy()
		{
			OnDestroy();
		}

		private void OnTapReturnToAnimus()
		{
			if (OnReturnToAnimusTapped != null)
			{
				OnReturnToAnimusTapped();
			}
		}
	}
}
