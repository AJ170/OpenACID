using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Ingame.Behaviours.MissionResult;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MissionDetailsScreen
{
	public class MissionDetailsNavigation : MonoBehaviour
	{
		[SerializeField]
		private UIMissionRewardItemBehaviour[] m_rewardItems;

		[SerializeField]
		private UIScrollBar m_scrollBar;

		[SerializeField]
		private UIInfoPopup m_infoPopup;

		private UIMissionRewardItemBehaviour m_currentSelectedItem;

		public void Show()
		{
			m_scrollBar.value = 0f;
			if (m_currentSelectedItem != null)
			{
				m_currentSelectedItem.IsSelected = false;
			}
			m_infoPopup.CloseNow();
		}

		public void Hide()
		{
			m_infoPopup.CloseNow();
		}

		private void Awake()
		{
		}

		private void Start()
		{
			for (int i = 0; i < m_rewardItems.Length; i++)
			{
				UIMissionRewardItemBehaviour uIMissionRewardItemBehaviour = m_rewardItems[i];
				uIMissionRewardItemBehaviour.OnClickCallback += OnRewardItemClicked;
			}
			m_infoPopup.CloseNow();
			m_infoPopup.OnPopupClosedCallback += OnClosePopup;
		}

		private void OnDestroy()
		{
			for (int i = 0; i < m_rewardItems.Length; i++)
			{
				UIMissionRewardItemBehaviour uIMissionRewardItemBehaviour = m_rewardItems[i];
				uIMissionRewardItemBehaviour.OnClickCallback -= OnRewardItemClicked;
			}
			m_infoPopup.OnPopupClosedCallback -= OnClosePopup;
		}

		private void OnRewardItemClicked(UIMissionRewardItemBehaviour _rewardItem)
		{
			if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatGuideEraSelect) && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs26Contracts))
			{
				m_currentSelectedItem = m_infoPopup.UpdateInfoPopupByRewardItem(m_currentSelectedItem, _rewardItem, true);
				if (m_currentSelectedItem != null)
				{
					m_infoPopup.MovePopupToTarget(_rewardItem.gameObject, UIInfoPopup.EPlacementSide.Left);
				}
			}
		}

		private void OnClosePopup()
		{
			if (m_currentSelectedItem != null)
			{
				m_currentSelectedItem.IsSelected = false;
			}
			m_currentSelectedItem = null;
		}
	}
}
