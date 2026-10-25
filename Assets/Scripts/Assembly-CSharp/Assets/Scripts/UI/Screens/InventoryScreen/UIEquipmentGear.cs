using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public abstract class UIEquipmentGear : MonoBehaviour, IInitialized<UIInventoryScreenData, UIAssassinSelectionHelper>
	{
		[SerializeField]
		private UIInventoryItemView m_itemView;

		public abstract bool Empty { get; }

		public abstract bool Selectable { get; }

		public abstract InventoryElement InventoryElement { get; }

		public bool Selected
		{
			get
			{
				return m_itemView.Selected;
			}
			set
			{
				m_itemView.Selected = value;
			}
		}

		protected UIInventoryScreenData ScreenData { get; private set; }

		protected UIAssassinSelectionHelper AssassinSelection { get; private set; }

		public UIInventoryItemView IconItem
		{
			get
			{
				return m_itemView;
			}
		}

		public event Action<UIEquipmentGear> OnClickCallback;

		public void SetLock(bool _lockState)
		{
			m_itemView.IsLocked = _lockState;
		}

		public void Initialize(UIInventoryScreenData _data, UIAssassinSelectionHelper _helper)
		{
			m_itemView.Init();
			AssassinSelection = _helper;
			ScreenData = _data;
			DoInitialize(_data);
			m_itemView.OnViewClicked += OnIconClickCallback;
		}

		public void Clear()
		{
			m_itemView.OnViewClicked -= OnIconClickCallback;
			DoClear();
		}

		private void OnIconClickCallback(UIInventoryItemView _view)
		{
			DoClicked();
			if (this.OnClickCallback != null && Selectable)
			{
				this.OnClickCallback(this);
			}
		}

		private void Awake()
		{
			DoAwake();
		}

		private void OnEnable()
		{
			DoEnable();
		}

		private void OnDisable()
		{
			DoDisable();
		}

		protected virtual void DoClicked()
		{
		}

		protected virtual void DoInitialize(UIInventoryScreenData _data)
		{
		}

		protected virtual void DoClear()
		{
		}

		protected virtual void DoAwake()
		{
		}

		protected virtual void DoEnable()
		{
		}

		protected virtual void DoDisable()
		{
		}

		internal void PlaySlotUnlockAnim(bool _playUnlockAnim)
		{
			if (InventoryElement != null)
			{
				OnUnlockAnimationFinished();
				_playUnlockAnim = false;
			}
			else if (_playUnlockAnim)
			{
				EventDelegate.Add(m_itemView.BackgroundIconTween.onFinished, OnUnlockAnimationFinished, true);
			}
			m_itemView.PlaySlotUnlockAnimation(_playUnlockAnim);
		}

		protected void OnUnlockAnimationFinished()
		{
			if (this is UIEquipmentModule)
			{
				UIEquipmentModule uIEquipmentModule = (UIEquipmentModule)this;
				if (uIEquipmentModule.HasModuleEntitlement)
				{
					UbiCore.Utils.Singleton<LocalStorage>.Instance.SetEntitlementShown(StaticEntitlementIds.EquipSlotModuleEntitlementGUID);
				}
			}
			else
			{
				if (!(this is UIEquipmentItem))
				{
					return;
				}
				UIEquipmentItem uIEquipmentItem = (UIEquipmentItem)this;
				if (!uIEquipmentItem.Category.EntitledToEquip())
				{
					return;
				}
				switch (uIEquipmentItem.Category)
				{
				case Item.EItemCategory.Insignia:
					if (uIEquipmentItem.HasInsigniaEntitlement())
					{
						UbiCore.Utils.Singleton<LocalStorage>.Instance.SetEntitlementShown(StaticEntitlementIds.EquipSlotInsigniaEntitlementGUID);
					}
					break;
				case Item.EItemCategory.Outfit:
					if (uIEquipmentItem.HasOutfitEntitlement())
					{
						UbiCore.Utils.Singleton<LocalStorage>.Instance.SetEntitlementShown(StaticEntitlementIds.EquipSlotOutfitEntitlementGUID);
					}
					break;
				}
			}
		}
	}
}
