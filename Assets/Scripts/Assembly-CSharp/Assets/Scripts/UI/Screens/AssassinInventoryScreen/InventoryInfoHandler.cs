using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinInventoryScreen
{
	public class InventoryInfoHandler : MonoBehaviour
	{
		public struct Data
		{
			public int TotalSpace;

			public int UsedSpace;

			public int FreeSpace
			{
				get
				{
					return TotalSpace - UsedSpace;
				}
			}
		}

		public delegate void SenderDelegate(InventoryInfoHandler _sender);

		[SerializeField]
		private UIButtonProxy m_buyButton;

		private bool m_filterActive;

		[SerializeField]
		private UIExtendedLabel m_label;

		[SerializeField]
		private UIButtonProxy m_showAllButton;

		public bool Enable { get; set; }

		public bool FilterActive
		{
			get
			{
				return m_filterActive;
			}
			set
			{
				m_filterActive = value;
				UpdateCategoryButton();
			}
		}

		public event SenderDelegate BuyInventory;

		public event SenderDelegate ShowAll;

		public void Init(Data _data)
		{
			m_label.text = LocalizationManager.Instance.Get(6540, new StringMapper
			{
				{
					"used_space",
					LocalizationManager.Instance.GetInteger(_data.FreeSpace)
				},
				{
					"total_space",
					LocalizationManager.Instance.GetInteger(_data.TotalSpace)
				}
			});
			if (_data.TotalSpace - _data.UsedSpace <= 0)
			{
				m_label.color = UIDataMapper.UITextColors.NegativeColor;
			}
			else
			{
				m_label.color = Color.white;
			}
			UpdateCategoryButton();
		}

		private void InvokeBuyInventory()
		{
			SenderDelegate buyInventory = this.BuyInventory;
			if (buyInventory != null)
			{
				buyInventory(this);
			}
		}

		private void InvokeShowAll()
		{
			SenderDelegate showAll = this.ShowAll;
			if (showAll != null)
			{
				showAll(this);
			}
		}

		private void Awake()
		{
		}

		private void OnBuyButtonClick()
		{
			InvokeBuyInventory();
		}

		private void OnDisable()
		{
			Enable = false;
			EventDelegate.Remove(m_buyButton.onClick, OnBuyButtonClick);
			EventDelegate.Remove(m_showAllButton.onClick, OnShowAllButtonClick);
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_buyButton.onClick, OnBuyButtonClick);
			EventDelegate.Add(m_showAllButton.onClick, OnShowAllButtonClick);
			m_showAllButton.Button.isEnabled = false;
			UpdateCategoryButton();
		}

		private void OnShowAllButtonClick()
		{
			FilterActive = !FilterActive;
			InvokeShowAll();
		}

		private void UpdateCategoryButton()
		{
			m_buyButton.gameObject.SetActive(Singleton<StateManager>.Instance.ProfileManager.CanExpandInventory);
			m_showAllButton.ShowIcon = FilterActive;
			m_showAllButton.TranslationKey = ((!FilterActive) ? 6451 : 1577);
			if (FilterActive)
			{
				m_showAllButton.Button.isEnabled = true;
			}
		}
	}
}
