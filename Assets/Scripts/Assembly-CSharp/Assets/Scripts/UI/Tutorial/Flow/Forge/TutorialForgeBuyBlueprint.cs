using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	public class TutorialForgeBuyBlueprint : TutorialButtonProxyClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideForgeBuyBlueprint.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeBuyBlueprint";
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

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			OnScreenDestroy();
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				UIInventoryScreen componentInChildren = Singleton<UIScreenManager>.Instance.CurrentScreen.GetComponentInChildren<UIInventoryScreen>();
				ShopBundle forgeBlueprintShopBundle = TutorialHelper.GetForgeBlueprintShopBundle();
				LootTemplate firstBlueprintTemplate = forgeBlueprintShopBundle.payout.lootTemplates.GetFirstBlueprintTemplate();
				InventoryElement itemFromLootTemplate = firstBlueprintTemplate.GetItemFromLootTemplate();
				componentInChildren.ForgeHandler.TutorialAddlInventoryElement(itemFromLootTemplate);
				componentInChildren.InventoryHandler.SetTutorialItem(itemFromLootTemplate);
				base.Deactivate();
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
			return 7770;
		}
	}
}
