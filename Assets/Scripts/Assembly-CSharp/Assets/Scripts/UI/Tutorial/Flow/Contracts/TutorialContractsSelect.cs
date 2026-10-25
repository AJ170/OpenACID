using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MissionScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Contracts
{
	public class TutorialContractsSelect : TutorialButtonClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		private readonly EMissionKind[] m_preferredMissionKinds = new EMissionKind[2]
		{
			EMissionKind.Kill_EMissionKind,
			EMissionKind.Deliver_EMissionKind
		};

		[SerializeField]
		protected UIMissionListContractMissions m_listContractMissions;

		public override string Name
		{
			get
			{
				return "TutorialContractsSelect";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26Contracts;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this);
		}

		protected override void OnSetupInternal()
		{
			for (int i = 0; i < m_preferredMissionKinds.Length && !m_listContractMissions.SelectMission(false, m_preferredMissionKinds[i]); i++)
			{
			}
			base.OnSetupInternal();
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.Mission;
		}

		public override int GetHeaderText()
		{
			return 7138;
		}

		public override int GetDescriptionText()
		{
			return 7139;
		}
	}
}
