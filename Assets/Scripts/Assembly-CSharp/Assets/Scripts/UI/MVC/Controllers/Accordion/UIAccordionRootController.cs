using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Behaviours.AccordionTab;
using Assets.Scripts.UI.MVC.Models.Accordion;
using Assets.Scripts.UI.MVC.UIView.Accordion;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Accordion
{
	public class UIAccordionRootController : UIControllerBase
	{
		private const float c_layoutUpdateTimer = 0.8f;

		[SerializeField]
		[CheckAssigned]
		private UIAccordionRootView m_accordionRootView;

		public Action<UIAccordionBaseTab> OnSelectionChanged;

		private float m_delayLayoutUpdateTimer;

		private UIAccordionCategoryBaseModel m_rootModel;

		private readonly List<UIAccordionBaseTab> m_children = new List<UIAccordionBaseTab>();

		private readonly List<UIAccordionBaseTab> m_firstLayerOfCategory = new List<UIAccordionBaseTab>();

		private readonly List<UITable> m_tables = new List<UITable>();

		public UIAccordionBaseTab CurrentSelectedTab { get; set; }

		public void PreInitialize(UIAccordionCategoryBaseModel _rootModel)
		{
			m_rootModel = _rootModel;
			UpdateAccordionn();
			PrepareAccordionBeforeUpdate();
		}

		public void Initialize(UIAccordionCategoryBaseModel _rootModel)
		{
			m_rootModel = _rootModel;
			UpdateAccordionn();
		}

		public void SelectFirstAvailableCategoryTab()
		{
			for (int i = 0; i < m_children.Count; i++)
			{
				UIAccordionBaseTab uIAccordionBaseTab = m_children[i];
				UIAccordionCategoryTab uIAccordionCategoryTab = uIAccordionBaseTab as UIAccordionCategoryTab;
				if (uIAccordionBaseTab.gameObject.activeSelf && uIAccordionCategoryTab != null && !uIAccordionCategoryTab.IsLocked)
				{
					SelectTab(uIAccordionCategoryTab);
					break;
				}
			}
		}

		protected override void OnEnableUnity()
		{
			base.OnEnableUnity();
			m_delayLayoutUpdateTimer = 0f;
			StartAlignment();
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			CurrentSelectedTab = null;
			for (int i = 0; i < m_children.Count; i++)
			{
				UIAccordionBaseTab uIAccordionBaseTab = m_children[i];
				if (!(uIAccordionBaseTab == null))
				{
					UIButton component = uIAccordionBaseTab.GetComponent<UIButton>();
					if (component != null)
					{
						EventDelegate.Remove(component.GetComponent<UIButton>().onClick, OnClickedTab);
					}
				}
			}
			m_children.Clear();
			m_firstLayerOfCategory.Clear();
			m_accordionRootView.ClearTable();
			m_tables.Clear();
		}

		private void UpdateAccordionn()
		{
			PrepareAccordionBeforeUpdate();
			m_tables.Clear();
			m_tables.Add(m_accordionRootView.RootTable);
			for (int i = 0; i < m_rootModel.SubCategories.Count; i++)
			{
				CreateOrUpdateAccordionn(null, m_accordionRootView.ViewRoot, m_rootModel.SubCategories[i], 0);
			}
			m_accordionRootView.UpdateTable();
		}

		private void PrepareAccordionBeforeUpdate()
		{
			for (int i = 0; i < m_children.Count; i++)
			{
				m_children[i].gameObject.SetActive(false);
			}
		}

		private void CreateOrUpdateAccordionn(UIAccordionCategoryTab _categoryTab, UIRect _anchorTarget, UIAccordionCategoryBaseModel _dataNode, int _layer)
		{
			UIAccordionBaseTab uIAccordionBaseTab = GetChildByLayer(_categoryTab, _layer);
			bool flag = uIAccordionBaseTab == null;
			if (flag)
			{
				uIAccordionBaseTab = CreateAndSetupAccordionByLayer(_layer, _categoryTab, _anchorTarget);
				if (_categoryTab == null)
				{
					m_firstLayerOfCategory.Add(uIAccordionBaseTab);
				}
			}
			if (_dataNode.SubCategories.Count > 0)
			{
				int layer = _layer + 1;
				UIAccordionCategoryTab component = uIAccordionBaseTab.GetComponent<UIAccordionCategoryTab>();
				for (int i = 0; i < _dataNode.SubCategories.Count; i++)
				{
					CreateOrUpdateAccordionn(component, uIAccordionBaseTab.GetComponent<UIRect>(), _dataNode.SubCategories[i], layer);
				}
				m_tables.Add(component.ChildTable);
			}
			if (flag)
			{
				uIAccordionBaseTab.SetDisplayData(_dataNode);
				uIAccordionBaseTab.Init(false);
				m_children.Add(uIAccordionBaseTab);
			}
			else
			{
				uIAccordionBaseTab.SetDisplayData(_dataNode);
				uIAccordionBaseTab.gameObject.SetActive(true);
			}
		}

		private UIAccordionBaseTab GetChildByLayer(UIAccordionCategoryTab _categoryTab, int _layer)
		{
			if (_categoryTab != null)
			{
				Transform transform = _categoryTab.ChildTable.transform;
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					UIAccordionBaseTab notActiveAccordionTabByLayer = GetNotActiveAccordionTabByLayer(child.gameObject, _layer);
					if (notActiveAccordionTabByLayer != null)
					{
						return notActiveAccordionTabByLayer;
					}
				}
			}
			else
			{
				for (int j = 0; j < m_firstLayerOfCategory.Count; j++)
				{
					UIAccordionBaseTab notActiveAccordionTabByLayer2 = GetNotActiveAccordionTabByLayer(m_firstLayerOfCategory[j].gameObject, _layer);
					if (notActiveAccordionTabByLayer2 != null)
					{
						return notActiveAccordionTabByLayer2;
					}
				}
			}
			return null;
		}

		private UIAccordionBaseTab GetNotActiveAccordionTabByLayer(GameObject _object, int _layer)
		{
			if (!_object.activeSelf)
			{
				if (_layer == 0)
				{
					return _object.GetComponent<UIAccordionCategoryTab>();
				}
				return _object.GetComponent<UIAccordionEntryTab>();
			}
			return null;
		}

		private UIAccordionBaseTab CreateAndSetupAccordionByLayer(int _layer, UIAccordionCategoryTab _parentTab, UIRect _anchorTarget)
		{
			UITable uITable = ((!(_parentTab == null)) ? _parentTab.ChildTable : m_accordionRootView.RootTable);
			UIAccordionBaseTab uIAccordionBaseTab = m_accordionRootView.CategoryTabPrefab;
			if (_layer > 0)
			{
				uIAccordionBaseTab = m_accordionRootView.EntryTabPrefab;
			}
			GameObject gameObject = NGUITools.AddChild(uITable.gameObject, uIAccordionBaseTab.gameObject);
			UIAccordionBaseTab component = gameObject.GetComponent<UIAccordionBaseTab>();
			component.UpdateTabAnchors(_anchorTarget);
			EventDelegate.Add(component.GetComponent<UIButton>().onClick, OnClickedTab);
			return component;
		}

		private void OnClickedTab()
		{
			UIAccordionBaseTab component = UIButton.current.GetComponent<UIAccordionBaseTab>();
			SelectTab(component);
		}

		private void SelectTab(UIAccordionBaseTab _tabToBeSelected)
		{
			UIAccordionCategoryTab uIAccordionCategoryTab = CurrentSelectedTab as UIAccordionCategoryTab;
			if (_tabToBeSelected != CurrentSelectedTab)
			{
				if (CurrentSelectedTab != null)
				{
					if (uIAccordionCategoryTab != null)
					{
						m_accordionRootView.SetTabHighlighterVisibility(false);
					}
					else
					{
						m_accordionRootView.PlayTabHighlighterTween(false);
					}
				}
				UIAccordionCategoryTab uIAccordionCategoryTab2 = _tabToBeSelected as UIAccordionCategoryTab;
				if ((bool)uIAccordionCategoryTab2)
				{
					uIAccordionCategoryTab2.IsOpend = !uIAccordionCategoryTab2.IsOpend;
				}
				else
				{
					m_accordionRootView.SetTabHighlighterVisibility(true);
					m_accordionRootView.UpdateTabHighlighterLocation(_tabToBeSelected.transform.position.y);
					m_accordionRootView.PlayTabHighlighterTween(true);
				}
				_tabToBeSelected.IsActivated = true;
				if (CurrentSelectedTab != null)
				{
					CurrentSelectedTab.IsActivated = false;
				}
			}
			else if (CurrentSelectedTab != null && (bool)uIAccordionCategoryTab)
			{
				uIAccordionCategoryTab.IsOpend = !uIAccordionCategoryTab.IsOpend;
			}
			StartAlignment();
			CurrentSelectedTab = _tabToBeSelected;
			Action<UIAccordionBaseTab> onSelectionChanged = OnSelectionChanged;
			if (onSelectionChanged != null)
			{
				onSelectionChanged(CurrentSelectedTab);
			}
		}

		private void StartAlignment()
		{
			if (m_delayLayoutUpdateTimer <= 0f)
			{
				StartCoroutine(AlignElements());
			}
			else
			{
				m_delayLayoutUpdateTimer = 0f;
			}
		}

		private IEnumerator AlignElements()
		{
			m_delayLayoutUpdateTimer = 0f;
			do
			{
				yield return new WaitForEndOfFrame();
				m_delayLayoutUpdateTimer += Time.deltaTime;
				for (int i = 0; i < m_tables.Count; i++)
				{
					m_tables[i].Reposition();
				}
				m_accordionRootView.RepositionScrollViewIfOutofBounds();
			}
			while (!(m_delayLayoutUpdateTimer > 0.8f));
			m_delayLayoutUpdateTimer = 0f;
		}
	}
}
