using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Models.Accordion
{
	public class UIAccordionCategoryBaseModel : UIModelBase
	{
		private static readonly Color s_selectedTextColor = new Color32(6, 88, 102, byte.MaxValue);

		private static readonly Color s_deselectedTextColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public string Id;

		public int SortId;

		public StringId NameId = StringId.Invalid;

		public string NameIconName = string.Empty;

		public bool IsUnread;

		public bool IsLocked;

		public int OwnCount;

		public int TotalCount;

		private readonly List<UIAccordionCategoryBaseModel> m_subCategories = new List<UIAccordionCategoryBaseModel>();

		public ReadOnlyCollection<UIAccordionCategoryBaseModel> SubCategories
		{
			get
			{
				return m_subCategories.AsReadOnly();
			}
		}

		public virtual Color SelectedTextColor
		{
			get
			{
				return s_selectedTextColor;
			}
		}

		public virtual Color DeselectedTextColor
		{
			get
			{
				return s_deselectedTextColor;
			}
		}

		public void Sort()
		{
			m_subCategories.Sort(SortItems);
			for (int i = 0; i < m_subCategories.Count; i++)
			{
				m_subCategories[i].Sort();
			}
		}

		public virtual void Add(UIAccordionCategoryBaseModel _achievement)
		{
			m_subCategories.Add(_achievement);
		}

		public virtual void UpdateProgress()
		{
			OwnCount = m_subCategories.Count;
			TotalCount = OwnCount;
			for (int i = 0; i < m_subCategories.Count; i++)
			{
				UIAccordionCategoryBaseModel uIAccordionCategoryBaseModel = m_subCategories[i];
				uIAccordionCategoryBaseModel.UpdateProgress();
				TotalCount += uIAccordionCategoryBaseModel.TotalCount;
			}
		}

		public virtual void ClearDynamicData()
		{
			IsUnread = false;
			for (int i = 0; i < m_subCategories.Count; i++)
			{
				m_subCategories[i].ClearDynamicData();
			}
			OwnCount = 0;
			TotalCount = 0;
		}

		private static int SortItems(UIAccordionCategoryBaseModel _entryA, UIAccordionCategoryBaseModel _entryB)
		{
			return _entryA.SortId.CompareTo(_entryB.SortId);
		}
	}
}
