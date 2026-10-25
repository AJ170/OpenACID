using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	public class UIButtonHeaderEvaluator : MonoBehaviour
	{
		[CheckAssigned]
		[SerializeField]
		private UIButtonHeader m_buttonHeader;

		private bool m_isActive;

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			set
			{
				m_isActive = value;
				if (m_isActive)
				{
					OnEvaluateButton();
				}
			}
		}

		public void ForceEvaluat()
		{
			if (m_isActive)
			{
				OnEvaluateButton();
			}
		}

		private void Start()
		{
			IsActive = false;
			bool isUnlocked = UIButtonHeaderUtil.WasHeaderButtonUnlockedBeforeMission(m_buttonHeader.HeaderType);
			m_buttonHeader.ChangeLockedState(isUnlocked, true, true);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged += new SimpleDelegate.CallbackFunction(OnMissionStateChanged);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, OnEvaluateButton);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenPopped, OnEvaluateButton);
		}

		private void OnDestroy()
		{
			if (Singleton<StateManager>.Instance.ProfileManager != null)
			{
				Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate -= new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			}
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged -= new SimpleDelegate.CallbackFunction(OnMissionStateChanged);
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, OnEvaluateButton);
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenPopped, OnEvaluateButton);
		}

		private void OnMissionStateChanged(object[] _params)
		{
			OnEvaluateButton();
		}

		private void OnProfileUpdate(object[] _params)
		{
			OnEvaluateButton();
		}

		private void OnEvaluateButton()
		{
			if (IsActive)
			{
				EvaluateButtonUnlockedState();
				EvaluateButtonActiveStatus();
			}
			else if (m_buttonHeader.IsUnlocked)
			{
				IsActive = true;
			}
		}

		private void EvaluateButtonActiveStatus()
		{
			if (!m_buttonHeader.IsUnlocked)
			{
				return;
			}
			EScreenID currentScreenId = Singleton<UIScreenManager>.Instance.CurrentScreenId;
			bool flag = m_buttonHeader.ScreenID == currentScreenId;
			MissionSelectionManager.EMissionMenu subMenuType = Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType;
			switch (m_buttonHeader.HeaderType)
			{
			case UIButtonHeader.EHeaderType.MissionsCampaign:
				flag = flag && subMenuType == MissionSelectionManager.EMissionMenu.Campaigns;
				break;
			case UIButtonHeader.EHeaderType.MissionsContracts:
				flag = flag && subMenuType == MissionSelectionManager.EMissionMenu.ContractMissions;
				break;
			case UIButtonHeader.EHeaderType.Shop:
				if (flag)
				{
					UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(currentScreenId);
					if (screen != null)
					{
						flag = !screen.ShowBackButton;
					}
				}
				break;
			}
			m_buttonHeader.IsEnabled = !flag;
		}

		private void EvaluateButtonUnlockedState()
		{
			LastProfileData lastProfileDataBeforeMission = Singleton<StateManager>.Instance.ProfileManager.LastProfileDataBeforeMission;
			bool flag = UIButtonHeaderUtil.IsButtonHeaderTypeUnlocked(m_buttonHeader.HeaderType);
			bool flag2 = ((lastProfileDataBeforeMission.EntitlementInventory == null) ? flag : UIButtonHeaderUtil.WasHeaderButtonUnlockedBeforeMission(m_buttonHeader.HeaderType));
			m_buttonHeader.ChangeLockedState(flag, flag == flag2);
		}
	}
}
