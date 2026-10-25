using AcierProtocol;

namespace Assets.Scripts.UI.Tutorial.Flow
{
	public static class FlowIdHelper
	{
		private static readonly EFlowId[] m_minFlowIds = new EFlowId[17];

		private static readonly EFlowId[] m_maxFlowIds = new EFlowId[17]
		{
			EFlowId.ProfileCreation,
			EFlowId.IntroMission,
			EFlowId.GuideLoadout,
			EFlowId.IntroMission,
			EFlowId.IntroMission,
			EFlowId.DEPRECATEDGuideHirelingSelect,
			EFlowId.GuideMysteryBoxInventoryReturn,
			EFlowId.GuideMysteryBoxLootAgain,
			EFlowId.DEPRECATEDGuideHirelingSelect,
			EFlowId.GuideForgeClaim,
			EFlowId.GuideShopSelectItem,
			EFlowId.GuideMysteryBoxClaim,
			EFlowId.GuideShopSelectItem,
			EFlowId.DEPRECATEDGuideHirelingSelect,
			EFlowId.ProfileCreation,
			EFlowId.IntroMission,
			EFlowId.IntroMission
		};

		public static STutorialFlowId ToTutorialFlowId(this EFlowId _flowId)
		{
			return new STutorialFlowId((int)_flowId);
		}

		public static EFlowId GetMinOfCategory(ETutorialCategory _category)
		{
			return m_minFlowIds[(int)_category];
		}

		public static EFlowId GetMaxOfCategory(ETutorialCategory _category)
		{
			return m_maxFlowIds[(int)_category];
		}
	}
}
