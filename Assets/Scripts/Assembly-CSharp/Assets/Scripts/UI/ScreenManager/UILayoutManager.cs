using System;
using System.Collections.Generic;
using Assets.Scripts.UI.ScreenManager.Layout;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	[Serializable]
	public class UILayoutManager : MonoBehaviour
	{
		public List<EventDelegate> OnLayoutOptionsChanged = new List<EventDelegate>();

		[SerializeField]
		private List<TagFamilyLayoutData> m_layouts = new List<TagFamilyLayoutData>();

		private EFamilyType m_selectedFamilyType;

		public EFamilyType SelectedFamilyType
		{
			get
			{
				return m_selectedFamilyType;
			}
			set
			{
				if (m_selectedFamilyType != value)
				{
					m_selectedFamilyType = value;
					RefreshLayout();
				}
			}
		}

		public List<TagFamilyLayoutData> LayoutData
		{
			get
			{
				return m_layouts;
			}
		}

		public FamilyLayoutData GetFamilyLayoutData(string _tagName)
		{
			for (int i = 0; i < m_layouts.Count; i++)
			{
				TagFamilyLayoutData tagFamilyLayoutData = m_layouts[i];
				if (tagFamilyLayoutData.TagNameId.ToLower() == _tagName.ToLower())
				{
					return tagFamilyLayoutData.GetFamilyLayoutData(m_selectedFamilyType);
				}
			}
			return null;
		}

		public void RefreshLayout()
		{
			EventDelegate.Execute(OnLayoutOptionsChanged);
		}

		private void Awake()
		{
			float layoutDPI = UIUtil.GetLayoutDPI();
			float screenInches = UIUtil.GetScreenInches();
			if (screenInches < 6.5f)
			{
				m_selectedFamilyType = EFamilyType.IphoneDPI326;
			}
			else if (screenInches < 11f)
			{
				if (layoutDPI < 200f)
				{
					m_selectedFamilyType = EFamilyType.IPadDPI132;
				}
				else
				{
					m_selectedFamilyType = EFamilyType.IPadRetinaDPI264;
				}
			}
			else
			{
				m_selectedFamilyType = EFamilyType.IPadRetinaDPI264;
			}
		}
	}
}
