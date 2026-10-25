using AcierProtocol;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.MVC.UIView.Misc;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Misc
{
	public class UIInfoPopupController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIInfoPopupViewPresenter m_infoPopupView;

		private GameObject m_target;

		public UIInfoPopup InfoPopup
		{
			get
			{
				return m_infoPopupView.InfoPopup;
			}
		}

		public void Initialize(InventoryElement _element, GameObject _target, AcierProtocol.Assassin _assassin, bool _moveToTarget = true)
		{
			m_target = _target;
			if (_element.IsModule())
			{
				ModuleInfoPopupHelper.ShowInfoPopup(m_infoPopupView.InfoPopup, ModuleInfoPopupHelper.EMode.None, _element, m_target.transform, true, string.Empty);
			}
			else if (_element.IsItem())
			{
				InventoryElement compareToItem = ((_assassin == null) ? null : _assassin.GetEquipedEquivalent(_element));
				ItemInfoPopupHelper.ShowInfoPopup(m_infoPopupView.InfoPopup, _assassin, ItemInfoPopupHelper.EMode.None, _element, compareToItem, m_target.transform, null, true, 0, string.Empty);
			}
			else if (_element.IsMysteryBox())
			{
				ItemInfoPopupHelper.ShowInfoPopup(m_infoPopupView.InfoPopup, _assassin, ItemInfoPopupHelper.EMode.None, _element, null, m_target.transform, null, true, 0, string.Empty);
			}
			else if (_element.IsBlueprint())
			{
				BlueprintInfoPopupHelper.ShowBlueprintInfoPopup(m_infoPopupView.InfoPopup, _element, new Button[0], true, m_target.transform, string.Empty);
			}
			else if (_element.IsCustomLoot())
			{
				CustomLootInfoPopupHelper.ShowInfoPopup(m_infoPopupView.InfoPopup, _element, true, m_target.transform, string.Empty);
			}
			InitializeTarget(_target, _moveToTarget);
		}

		public void Initialize(Resource _resource, Quality.EQuality? _itemQuality, GameObject _target, bool _moveToTarget = true)
		{
			m_target = _target;
			ItemQualityDescription qualityDescription = null;
			if (_itemQuality.HasValue)
			{
				qualityDescription = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_itemQuality.Value];
			}
			ItemInfoPopupHelper.ShowInfoPopup(m_infoPopupView.InfoPopup, ItemInfoPopupHelper.EMode.None, _resource, _target.transform, qualityDescription, true, 0, string.Empty);
			InitializeTarget(_target, _moveToTarget);
		}

		public void Initialize(LootFilter _lootFilter, ItemDef _itemDef, Quality.EQuality? _itemQuality, GameObject _target, bool _moveToTarget = true)
		{
			ItemQualityDescription qualityDescription = null;
			if (_itemQuality.HasValue)
			{
				qualityDescription = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_itemQuality.Value];
			}
			ItemInfoPopupHelper.ShowInfoPopup(m_infoPopupView.InfoPopup, ItemInfoPopupHelper.EMode.None, _itemDef, _lootFilter, _lootFilter.GetItemCategory(), _target.transform, qualityDescription, true, 0, string.Empty);
			InitializeTarget(_target, _moveToTarget);
		}

		public void Initialize(ShopBundle _shopBundle, GameObject _target, bool _moveToTarget = true)
		{
			m_infoPopupView.InfoPopup.UpdateByShopBundle(_shopBundle, _target.transform);
			InitializeTarget(_target, _moveToTarget);
		}

		public void Initialize(ECustomItemType _customItemType, int _customItemCount, GameObject _target, bool _moveToTarget = true)
		{
			m_target = _target;
			CustomItemTypeInfoPopupHelper.ShowInfoPopup(m_infoPopupView.InfoPopup, _customItemType, _customItemCount, true, _target.transform);
			InitializeTarget(_target, _moveToTarget);
		}

		public void OnBeforePopupCloseCallback()
		{
			base.IsInitialized = false;
		}

		public void Close()
		{
			m_infoPopupView.Close();
			base.IsInitialized = false;
			m_target = null;
		}

		private void InitializeTarget(GameObject _target, bool _moveToTarget = true)
		{
			m_target = _target;
			if (_moveToTarget && m_target != null)
			{
				m_infoPopupView.InfoPopup.MovePopupToTarget(m_target);
			}
			base.IsInitialized = true;
		}
	}
}
