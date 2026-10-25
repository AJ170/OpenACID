using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Ingame.Behaviours.MissionResult;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public static class UIInfoPopupHelper
	{
		private static int c_popupOffsetToItem = 20;

		public static void UpdateByLootTemplate(this UIInfoPopup _infoPopup, LootTemplate _template, Transform _target, string _colorOverride, bool _showCloseButton)
		{
			if (_template.itemTemplate != null || _template.mysteryBoxTemplate != null)
			{
				Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
				InventoryElement itemFromLootTemplate = _template.GetItemFromLootTemplate();
				int itemPowerOverride = _template.itemPowerOverride;
				ItemInfoPopupHelper.ShowInfoPopup(_infoPopup, currentAssassin, ItemInfoPopupHelper.EMode.None, itemFromLootTemplate, null, _target, null, _showCloseButton, itemPowerOverride, _colorOverride);
			}
			else if (_template.moduleTemplate != null)
			{
				InventoryElement itemFromLootTemplate2 = _template.GetItemFromLootTemplate();
				ModuleInfoPopupHelper.ShowInfoPopup(_infoPopup, ModuleInfoPopupHelper.EMode.None, itemFromLootTemplate2, _target, _showCloseButton, _colorOverride);
			}
			else if (_template.resourceTemplate != null)
			{
				LootTemplate.ResourceTemplate resourceTemplate = _template.resourceTemplate;
				int itemPowerOverride = _template.itemPowerOverride;
				ItemInfoPopupHelper.ShowInfoPopup(_infoPopup, ItemInfoPopupHelper.EMode.None, resourceTemplate.maxResource, _target, null, _showCloseButton, itemPowerOverride, _colorOverride);
			}
			else if (_template.blueprintTemplate != null)
			{
				InventoryElement itemFromLootTemplate3 = _template.GetItemFromLootTemplate();
				BlueprintInfoPopupHelper.ShowBlueprintInfoPopup(_infoPopup, itemFromLootTemplate3, new Button[0], _showCloseButton, _target, _colorOverride);
			}
			else if (_template.customTemplate != null)
			{
				InventoryElement itemFromLootTemplate4 = _template.GetItemFromLootTemplate();
				CustomLootInfoPopupHelper.ShowInfoPopup(_infoPopup, itemFromLootTemplate4, _showCloseButton, _target, _colorOverride);
			}
		}

		public static void UpdateByShopBundle(this UIInfoPopup _infoPopup, ShopBundle _bundle, Transform _target)
		{
			string text = (string.IsNullOrEmpty(_bundle.colorDefGuid) ? _bundle.shopPreviewColorDefGuid : _bundle.colorDefGuid);
			bool showCloseButton = !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatGuideShop);
			LootTemplate shopPreview = _bundle.shopPreview;
			LootTemplate[] lootTemplates = _bundle.payout.lootTemplates;
			if (lootTemplates.Length == 1 && lootTemplates[0].entitlementTemplate != null)
			{
				InventoryElement itemFromLootTemplate = shopPreview.GetItemFromLootTemplate();
				EntitlementInfoPopupHelper.ShowInfoPopup(_infoPopup, itemFromLootTemplate, _target, showCloseButton, text);
			}
			else
			{
				_infoPopup.UpdateByLootTemplate(shopPreview, _target, text, showCloseButton);
			}
		}

		public static UIMissionRewardItemBehaviour UpdateInfoPopupByRewardItem(this UIInfoPopup _infoPopup, UIMissionRewardItemBehaviour _currentSelectedItem, UIMissionRewardItemBehaviour _newRewardItem, bool _showIndicator)
		{
			if (_currentSelectedItem != null)
			{
				_currentSelectedItem.IsSelected = false;
			}
			if (_currentSelectedItem != _newRewardItem && _newRewardItem != null)
			{
				_currentSelectedItem = _newRewardItem;
				_currentSelectedItem.IsSelected = true;
				Transform transform = ((!_showIndicator) ? null : _newRewardItem.transform);
				Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
				InventoryElement inventoryElement = _newRewardItem.InventoryElement;
				if (inventoryElement != null)
				{
					if (inventoryElement.IsModule())
					{
						ModuleInfoPopupHelper.ShowInfoPopup(_infoPopup, ModuleInfoPopupHelper.EMode.None, inventoryElement, transform, true, string.Empty);
					}
					else if (inventoryElement.IsItem())
					{
						InventoryElement equipedEquivalent = currentAssassin.GetEquipedEquivalent(inventoryElement);
						ItemInfoPopupHelper.ShowInfoPopup(_infoPopup, currentAssassin, ItemInfoPopupHelper.EMode.None, inventoryElement, equipedEquivalent, transform, null, true, 0, string.Empty);
					}
					else if (inventoryElement.IsMysteryBox())
					{
						ItemInfoPopupHelper.ShowInfoPopup(_infoPopup, currentAssassin, ItemInfoPopupHelper.EMode.None, inventoryElement, null, transform, null, true, 0, string.Empty);
					}
					else if (inventoryElement.IsBlueprint())
					{
						BlueprintInfoPopupHelper.ShowBlueprintInfoPopup(_infoPopup, inventoryElement, new Button[0], true, transform, string.Empty);
					}
					else if (inventoryElement.IsCustomLoot())
					{
						CustomLootInfoPopupHelper.ShowInfoPopup(_infoPopup, inventoryElement, true, transform, string.Empty);
					}
				}
				else if (_newRewardItem.Resource != null)
				{
					ItemQualityDescription qualityDescription = null;
					if (_newRewardItem.UseItemQuality)
					{
						qualityDescription = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_newRewardItem.ItemQuality];
					}
					ItemInfoPopupHelper.ShowInfoPopup(_infoPopup, ItemInfoPopupHelper.EMode.None, _newRewardItem.Resource, transform, qualityDescription, true, 0, string.Empty);
				}
				else if (_newRewardItem.LootTemplate != null)
				{
					_infoPopup.UpdateByLootTemplate(_newRewardItem.LootTemplate, transform, string.Empty, true);
				}
				else
				{
					ItemQualityDescription qualityDescription2 = null;
					if (_newRewardItem.UseItemQuality)
					{
						qualityDescription2 = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_newRewardItem.ItemQuality];
					}
					ItemInfoPopupHelper.ShowInfoPopup(_infoPopup, ItemInfoPopupHelper.EMode.None, _newRewardItem.ItemDef, _newRewardItem.LootFilter, _newRewardItem.ItemCategory, transform, qualityDescription2, true, 0, string.Empty);
				}
			}
			else
			{
				_currentSelectedItem = null;
				_infoPopup.Close();
			}
			return _currentSelectedItem;
		}

		public static UIInfoPopup.EPlacementSide MovePopupToTarget(this UIInfoPopup _popup, GameObject _target, UIInfoPopup.EPlacementSide _targetSide = UIInfoPopup.EPlacementSide.Auto)
		{
			UIRect component = _popup.GetComponent<UIRect>();
			UIRect component2 = _target.GetComponent<UIRect>();
			Vector3 localPosition = component.transform.localPosition;
			localPosition.x = component.transform.parent.InverseTransformPoint(component2.transform.position).x;
			component.transform.localPosition = localPosition;
			UIInfoPopup.EPlacementSide ePlacementSide = _targetSide;
			if (ePlacementSide == UIInfoPopup.EPlacementSide.Auto)
			{
				Camera cam = NGUITools.FindCameraForLayer(component.root.gameObject.layer);
				Vector3[] sides = cam.GetSides(component.transform);
				float num = Mathf.Abs(sides[0].x);
				float num2 = Mathf.Abs(sides[2].x);
				ePlacementSide = ((!(num > num2)) ? UIInfoPopup.EPlacementSide.Right : UIInfoPopup.EPlacementSide.Left);
			}
			Vector3[] localCorners = component.localCorners;
			Vector3[] localCorners2 = component2.localCorners;
			float num3 = 0f;
			num3 = ((ePlacementSide != 0) ? (localCorners[3].x + localCorners2[3].x + (float)c_popupOffsetToItem) : (localCorners[1].x + localCorners2[1].x - (float)c_popupOffsetToItem));
			localPosition.x += num3;
			component.transform.localPosition = localPosition;
			return ePlacementSide;
		}
	}
}
