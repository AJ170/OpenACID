using AcierProtocol;
using Assets.Scripts.UI.Ingame.Behaviours.MissionResult;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	public class TutorialForgeSelectBlueprint : TutorialClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideMysteryBoxInventoryReturn.ToTutorialFlowId();

		private UIMissionRewardItemBehaviour m_rewardItem;

		public override string Name
		{
			get
			{
				return "TutorialForgeSelectBlueprint";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Forge;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		protected void Start()
		{
			OnScreenInit();
		}

		protected override void OnSetupInternal()
		{
			string forgeTutorialInputBlueprintId = Singleton<StateManager>.Instance.TemplateManager.Globals.forgeTutorialInputBlueprintId;
			m_rewardItem = Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.GetRewardItemByBundleId(forgeTutorialInputBlueprintId);
			m_buttonWidget = m_rewardItem.GetComponent<UIWidget>();
			base.OnSetupInternal();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_rewardItem = null;
			m_buttonWidget = null;
			OnScreenDestroy();
		}

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
			m_rewardItem.OnClickCallback += OnClickWrapper;
		}

		private void OnClickWrapper(UIMissionRewardItemBehaviour _obj)
		{
			OnClick();
		}

		protected override void RemoveButtonFromEventDelegate()
		{
			if (m_rewardItem != null)
			{
				m_rewardItem.OnClickCallback -= OnClickWrapper;
			}
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideForge(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 7768;
		}

		public override int GetDescriptionText()
		{
			return 7769;
		}
	}
}
