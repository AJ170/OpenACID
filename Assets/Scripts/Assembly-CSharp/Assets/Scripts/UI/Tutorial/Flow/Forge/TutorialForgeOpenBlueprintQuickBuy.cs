using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Tutorial.Flow.InventoryScreenBases;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	public class TutorialForgeOpenBlueprintQuickBuy : TutorialForgeSelectItemBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideMysteryBoxClaim.ToTutorialFlowId();

		private long m_addedVirtualHardCurrency;

		public override string Name
		{
			get
			{
				return "TutorialForgeOpenBlueprintQuickBuy";
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
			return TutorialHelper.ConditionAllowShowGuideForge(this);
		}

		protected override void OnSetupInternal()
		{
			Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnPreviewOpenCallback += OnPreviewOpen;
			base.OnSetupInternal();
		}

		private void OnPreviewOpen()
		{
			Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnPreviewOpenCallback -= OnPreviewOpen;
			bool flag = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnPreviewOpenCallback -= OnPreviewOpen;
				base.Deactivate();
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnPreviewOpenCallback -= OnPreviewOpen;
			}
		}

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
		}

		protected override void RemoveButtonFromEventDelegate()
		{
		}

		protected override void OnClickInternal()
		{
		}

		public override int GetHeaderText()
		{
			return 7768;
		}

		public override int GetDescriptionText()
		{
			return (int)StringId.Invalid.Value;
		}

		public override string GetDescription()
		{
			return TutorialHelper.ExchangeCurrencyInString(7767, EResourceType.HardCurrency);
		}

		protected override UIInventoryItemView GetItemViewToSelect()
		{
			int itemIndexOfBuyBluePrintSlot = m_inventoryHandler.GetItemIndexOfBuyBluePrintSlot();
			List<Transform> children = m_itemContainer.Children;
			return TutorialHelper.GetInventoryItemView(children, itemIndexOfBuyBluePrintSlot);
		}
	}
}
