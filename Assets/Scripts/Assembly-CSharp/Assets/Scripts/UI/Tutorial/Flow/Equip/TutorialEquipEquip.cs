using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Equip
{
	public class TutorialEquipEquip : TutorialInfoPopUpBase
	{
		private readonly STutorialFlowId m_index = EFlowId.DEPRECATEDGuideHirelingSelect.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Inventory Item Equip";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Equip;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		protected override void AddActionCallback()
		{
			ItemInfoPopupHelper.OnEquipedItemCallback += new SimpleDelegate.CallbackFunction(OnEquip);
		}

		protected override void RemoveActionCallback()
		{
			ItemInfoPopupHelper.OnEquipedItemCallback -= new SimpleDelegate.CallbackFunction(OnEquip);
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			ItemInfoPopupHelper.OnEquipedItemCallback -= new SimpleDelegate.CallbackFunction(OnEquip);
		}

		private void OnEquip(params object[] _params)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideEquip(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 4299;
		}

		public override int GetDescriptionText()
		{
			return 4303;
		}
	}
}
