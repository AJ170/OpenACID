using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager.Popup.Widgets;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.ItemPopup
{
	public class ItemPopupContent : PopUpContentView<ItemPopupContent.ItemPopupData>
	{
		public class PopupItemConfiguration
		{
			public readonly InventoryElement Item;

			public readonly ShopBundle ShopBundle;

			public readonly long Amount = 1L;

			public PopupItemConfiguration(ShopBundle _shopBundle, long _amount)
			{
				ShopBundle = _shopBundle;
				Amount = _amount;
			}

			public PopupItemConfiguration(InventoryElement _item, long _amount)
			{
				Item = _item;
				Amount = _amount;
			}
		}

		public class ItemPopupData : PopUpContentData
		{
			public PopupItemConfiguration[] Items;

			public List<UIPopupResourcesContent.ResourceConfiguration> ResourcesCost;

			public bool ResourceSymbolAdditive;

			public string Description;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.Item;
				}
			}

			public override bool HasResourceContentDisplay
			{
				get
				{
					return true;
				}
			}
		}

		[SerializeField]
		private UIAlignmentBox m_itemsBox;

		[SerializeField]
		private GameObject m_itemPrefab;

		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private UIPopupResourcesContent m_uiPopupResources;

		public override Vector2 Size
		{
			get
			{
				return m_itemsBox.Size;
			}
		}

		private void Awake()
		{
			m_description.supportEncoding = true;
		}

		protected override void Render(ItemPopupData _data)
		{
			m_description.text = _data.Description;
			m_uiPopupResources.ConfigurateItemsCost(_data.ResourcesCost, _data.ResourceSymbolAdditive);
			ConfigurateItems(_data.Items);
		}

		public override void UpdateLayout()
		{
			if (m_itemsBox.gameObject.activeSelf)
			{
				EventDelegate.Add(m_itemsBox.OnReposition, OnAlignmentUpdate, true);
				m_itemsBox.RepositionNow = true;
			}
			else
			{
				OnAlignmentUpdate();
			}
		}

		public override void Purge()
		{
			base.Purge();
			if (m_itemsBox != null)
			{
				m_itemsBox.transform.ClearChildren();
			}
		}

		private void OnAlignmentUpdate()
		{
			EventDelegate.Execute(onLayoutFinished);
		}

		private void ConfigurateItems(PopupItemConfiguration[] _items)
		{
			m_itemsBox.gameObject.SetActive(_items.Length > 0);
			for (int i = 0; i < _items.Length; i++)
			{
				UIPopupItemImageWidget uIPopupItemImageWidget = null;
				uIPopupItemImageWidget = ((i >= m_itemsBox.Children.Count) ? NGUITools.AddChild(m_itemsBox.gameObject, m_itemPrefab).GetComponent<UIPopupItemImageWidget>() : m_itemsBox.Children[i].GetComponent<UIPopupItemImageWidget>());
				uIPopupItemImageWidget.Configure(_items[i], false);
				uIPopupItemImageWidget.gameObject.SetActive(true);
			}
			for (int j = _items.Length; j < m_itemsBox.Children.Count; j++)
			{
				m_itemsBox.Children[j].gameObject.SetActive(false);
			}
		}
	}
}
