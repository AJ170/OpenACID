using AcierProtocol;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.UIView.Assassin;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Assassin
{
	public class UIAssassinAndEquipmentPreviewController : UIControllerBase
	{
		protected const string c_openAnimationName = "ItemBundleOpen";

		protected const string c_closeAnimationName = "ItemBundleClose";

		[CheckAssigned]
		[SerializeField]
		private UIAssassinAndEquipmentPreviewView m_preview;

		[CheckAssigned]
		[SerializeField]
		private UIAssassinSkillSummeryController m_assassinSkillSummery;

		[SerializeField]
		[CheckAssigned]
		private UIAssassinEquipmentController m_assassinEquipment;

		[SerializeField]
		[CheckAssigned]
		private UIPanel m_ownPanel;

		[SerializeField]
		[CheckAssigned]
		private UIPanel m_overlayPanel;

		public void OpenPreview(string _playerName, int _heroicRank, int _leaderboardRank, AcierProtocol.Assassin _assassin)
		{
			m_assassinEquipment.InitializeController(_assassin);
			m_assassinSkillSummery.UpdateSummery(_assassin.GetSummary());
			UpdatePreview(_playerName, _heroicRank, _leaderboardRank, _assassin);
			m_preview.BackgroundColor = UIDataMapper.UITextColors.AssassinPreviewPopupBackgroundColor;
			base.gameObject.SetActive(true);
			m_preview.StartAnimation("ItemBundleOpen");
		}

		public override void OnControllerClicked()
		{
			base.OnControllerClicked();
			OnClosePopup();
		}

		protected override void OnEnableUnity()
		{
			base.OnEnableUnity();
			m_overlayPanel.gameObject.SetActive(true);
			m_overlayPanel.alpha = m_ownPanel.alpha;
			BehaviourSingleton<EnvironmentBridgeMain>.Instance.FloorMatteShadow.gameObject.SetActive(true);
			BehaviourSingleton<EnvironmentBridgeMain>.Instance.FloorStaticObject.gameObject.SetActive(false);
			CameraMenuManager cameraMenuManager = BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager;
			if (cameraMenuManager.AssassinCamera != null)
			{
				cameraMenuManager.AssassinCamera.gameObject.SetActive(true);
			}
		}

		protected override void OnDisableUnity()
		{
			base.OnDisableUnity();
			if (m_overlayPanel != null)
			{
				m_overlayPanel.gameObject.SetActive(false);
			}
			BehaviourSingleton<EnvironmentBridgeMain>.Instance.FloorMatteShadow.gameObject.SetActive(false);
			BehaviourSingleton<EnvironmentBridgeMain>.Instance.FloorStaticObject.gameObject.SetActive(true);
			CameraMenuManager cameraMenuManager = BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager;
			if (cameraMenuManager.AssassinCamera != null)
			{
				cameraMenuManager.AssassinCamera.gameObject.SetActive(false);
			}
		}

		protected override void OnUpdateUnity()
		{
			base.OnUpdateUnity();
			m_overlayPanel.alpha = m_ownPanel.alpha;
		}

		protected void OnClosePopup()
		{
			if (base.gameObject.activeSelf)
			{
				m_assassinEquipment.OnClosePopup();
				m_preview.StartAnimation("ItemBundleClose", delegate
				{
					base.gameObject.SetActive(false);
				});
			}
		}

		private void UpdatePreview(string _playerName, int _heroicRank, int _leaderboardRank, AcierProtocol.Assassin _assassin)
		{
			m_preview.PlayerName = _playerName;
			m_preview.AssassinName = _assassin.GetAssassinName();
			m_preview.AssassinClass = _assassin.GetAssassinClassDisplayNames();
			m_preview.LeaderboardRank = ((_leaderboardRank <= 0) ? string.Empty : LocalizationManager.Instance.GetInteger(_leaderboardRank));
			m_preview.HeroicRank = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription[_heroicRank].NameId);
		}
	}
}
