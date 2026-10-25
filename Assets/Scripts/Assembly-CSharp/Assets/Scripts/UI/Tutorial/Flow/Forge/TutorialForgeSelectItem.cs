using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Tutorial.Flow.InventoryScreenBases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	public class TutorialForgeSelectItem : TutorialForgeSelectItemBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeSelectItem";
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

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideForge(this);
		}

		protected override UIInventoryItemView GetItemViewToSelect()
		{
			string itemToForge = GetItemToForge();
			int itemIndexByTechnicalName = m_inventoryHandler.GetItemIndexByTechnicalName(itemToForge);
			List<Transform> children = m_itemContainer.Children;
			return TutorialHelper.GetInventoryItemView(children, itemIndexByTechnicalName);
		}

		public override int GetHeaderText()
		{
			return 6923;
		}

		public override int GetDescriptionText()
		{
			return 6927;
		}

		public virtual string GetItemToForge()
		{
			return TutorialHelper.TutorialForgeItemOne;
		}
	}
}
