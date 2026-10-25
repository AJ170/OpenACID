using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Achievements;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Behaviours.AccordionTab;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AchievementsScreen
{
	public class UIAchievementsCategoriesDetails : UIScreenChild
	{
		private const float c_layoutUpdateTimer = 0.8f;

		[SerializeField]
		private UIAchievementsDetails m_achievementsDetails;

		[SerializeField]
		private UIRect m_anchorTarget;

		[SerializeField]
		private UITable m_rootTable;

		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		private BBTweenAlpha m_selectedTabHighlighterTween;

		[SerializeField]
		private UIAccordionCategoryTab m_categoryTabPrefab;

		[SerializeField]
		private UIAccordionEntryTab m_entryTabPrefab;

		private UIAccordionBaseTab m_lastPressedTab;

		private float m_delayLayoutUpdateTimer;

		private UIAccordionBaseTab m_prevTab;

		private readonly List<UIAccordionBaseTab> m_children = new List<UIAccordionBaseTab>();

		private readonly List<UITable> m_tables = new List<UITable>();

		private UIAccordionCategoryTab m_summeryTab;

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeactivate);
			CreateCategoryAccordion();
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
			}
			for (int i = 0; i < m_children.Count; i++)
			{
				UIAccordionBaseTab uIAccordionBaseTab = m_children[i];
				if (uIAccordionBaseTab != null)
				{
					UIButton component = uIAccordionBaseTab.GetComponent<UIButton>();
					if (component != null)
					{
						EventDelegate.Remove(component.GetComponent<UIButton>().onClick, OnClickedTab);
					}
				}
			}
			m_children.Clear();
			if (m_rootTable != null && m_rootTable.transform != null)
			{
				m_rootTable.transform.ClearChildren();
			}
			m_lastPressedTab = null;
			m_tables.Clear();
			m_prevTab = null;
		}

		private void OnScreenActivate()
		{
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			achievementsManager.AchievementDataUpdated += OnAchievementDataUpdated;
			UpdateCategories();
			m_delayLayoutUpdateTimer = 0f;
			StartAlignment();
		}

		private void OnScreenDeactivate()
		{
			Singleton<StateManager>.Instance.AchievementsManager.AchievementDataUpdated -= OnAchievementDataUpdated;
		}

		private void Start()
		{
			UITable rootTable = m_rootTable;
			rootTable.onReposition = (UITable.OnReposition)Delegate.Combine(rootTable.onReposition, new UITable.OnReposition(OnFirstReposition));
			m_rootTable.repositionNow = true;
			if (m_lastPressedTab == null)
			{
				m_achievementsDetails.UpdateAchievementDetails(Singleton<StateManager>.Instance.AchievementsManager.AchievementCategoryData);
			}
		}

		private void OnDestroy()
		{
			UITable rootTable = m_rootTable;
			rootTable.onReposition = (UITable.OnReposition)Delegate.Remove(rootTable.onReposition, new UITable.OnReposition(OnFirstReposition));
			m_prevTab = null;
		}

		private void OnFirstReposition()
		{
			UITable rootTable = m_rootTable;
			rootTable.onReposition = (UITable.OnReposition)Delegate.Remove(rootTable.onReposition, new UITable.OnReposition(OnFirstReposition));
			if (m_lastPressedTab == null)
			{
				m_summeryTab.Init(true);
				m_lastPressedTab = m_summeryTab;
				m_scrollView.ResetPosition();
			}
		}

		private void OnAchievementDataUpdated()
		{
			UpdateCategories();
		}

		private void CreateCategoryAccordion()
		{
			AchievementCategoryData achievementCategoryData = Singleton<StateManager>.Instance.AchievementsManager.AchievementCategoryData;
			GameObject gameObject = NGUITools.AddChild(m_rootTable.gameObject, m_categoryTabPrefab.gameObject);
			m_summeryTab = gameObject.GetComponent<UIAccordionCategoryTab>();
			m_summeryTab.UpdateTabAnchors(m_anchorTarget);
			m_summeryTab.SetDisplayData(achievementCategoryData.CategoryId, 5627, false);
			EventDelegate.Add(m_summeryTab.GetComponent<UIButton>().onClick, OnClickedTab);
			m_children.Add(m_summeryTab);
			for (int i = 0; i < achievementCategoryData.SubCategories.Count; i++)
			{
				CreateCategoryAccordion(m_rootTable, m_anchorTarget, achievementCategoryData.SubCategories[i], 0);
			}
		}

		private void CreateCategoryAccordion(UITable _parentTable, UIRect _anchorTarget, AchievementCategoryData _dataNode, int _layer)
		{
			AchievementCategoryDefinition achievementCategoryDefinition = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementCategoryDefinition(_dataNode.CategoryId);
			UIAccordionBaseTab uIAccordionBaseTab = m_categoryTabPrefab;
			if (_layer > 0)
			{
				uIAccordionBaseTab = m_entryTabPrefab;
			}
			GameObject gameObject = NGUITools.AddChild(_parentTable.gameObject, uIAccordionBaseTab.gameObject);
			UIAccordionBaseTab component = gameObject.GetComponent<UIAccordionBaseTab>();
			component.UpdateTabAnchors(_anchorTarget);
			if (_dataNode.SubCategories.Count > 0)
			{
				int layer = _layer + 1;
				UIAccordionCategoryTab component2 = gameObject.GetComponent<UIAccordionCategoryTab>();
				for (int i = 0; i < _dataNode.SubCategories.Count; i++)
				{
					CreateCategoryAccordion(component2.ChildTable, gameObject.GetComponent<UIRect>(), _dataNode.SubCategories[i], layer);
				}
			}
			EventDelegate.Add(component.GetComponent<UIButton>().onClick, OnClickedTab);
			component.SetDisplayData(_dataNode.CategoryId, achievementCategoryDefinition.NameLocaKey.Key, _dataNode.HasUnread);
			component.Init(false);
			m_children.Add(component);
			m_tables.Add(_parentTable);
		}

		private void UpdateCategories()
		{
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			for (int i = 0; i < m_children.Count; i++)
			{
				UIAccordionBaseTab uIAccordionBaseTab = m_children[i];
				if (!(uIAccordionBaseTab != null))
				{
					continue;
				}
				AchievementCategoryData achievementCategoryData = achievementsManager.GetAchievementCategoryData(uIAccordionBaseTab.Id);
				UIAccordionCategoryTab uIAccordionCategoryTab = uIAccordionBaseTab as UIAccordionCategoryTab;
				if (string.IsNullOrEmpty(uIAccordionBaseTab.Id))
				{
					uIAccordionBaseTab.NumberOfEntriesLabel.text = string.Empty;
					uIAccordionCategoryTab.HasChildren = false;
					continue;
				}
				bool flag = achievementCategoryData.TotalCount > 0;
				uIAccordionBaseTab.gameObject.SetActive(flag);
				if (flag)
				{
					uIAccordionBaseTab.HasUnread = achievementCategoryData.HasUnread;
					StringMapper stringMapper = new StringMapper();
					stringMapper.Add("value", achievementCategoryData.TotalClaimedCount);
					stringMapper.Add("max_value", achievementCategoryData.TotalCount);
					StringMapper variables = stringMapper;
					uIAccordionBaseTab.NumberOfEntriesLabel.SetLocalizationKey(5630, variables);
					if (uIAccordionCategoryTab != null)
					{
						uIAccordionCategoryTab.HasChildren = achievementCategoryData.SubCategories.Count > 0;
					}
				}
			}
			if (m_lastPressedTab != null)
			{
				AchievementCategoryData achievementCategoryData2 = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementCategoryData(m_lastPressedTab.Id);
				m_achievementsDetails.UpdateAchievementDetails(achievementCategoryData2, false);
			}
		}

		private void OnClickedTab()
		{
			UIAccordionBaseTab component = UIButton.current.GetComponent<UIAccordionBaseTab>();
			UIAccordionCategoryTab uIAccordionCategoryTab = m_lastPressedTab as UIAccordionCategoryTab;
			if (component != m_lastPressedTab)
			{
				if (m_lastPressedTab != null)
				{
					if (uIAccordionCategoryTab != null)
					{
						m_selectedTabHighlighterTween.gameObject.SetActive(false);
					}
					else
					{
						m_selectedTabHighlighterTween.StartTweenPlayReverse();
					}
				}
				UIAccordionCategoryTab uIAccordionCategoryTab2 = component as UIAccordionCategoryTab;
				if ((bool)uIAccordionCategoryTab2)
				{
					uIAccordionCategoryTab2.IsOpend = !uIAccordionCategoryTab2.IsOpend;
				}
				else
				{
					Vector3 position = m_selectedTabHighlighterTween.transform.position;
					position.y = component.transform.position.y;
					m_selectedTabHighlighterTween.transform.position = position;
					m_selectedTabHighlighterTween.gameObject.SetActive(true);
					m_selectedTabHighlighterTween.StartTweenPlayForward();
				}
				component.IsActivated = true;
				if (m_prevTab != null)
				{
					m_prevTab.IsActivated = false;
				}
				m_lastPressedTab = component;
				AchievementCategoryData achievementCategoryData = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementCategoryData(component.Id);
				m_achievementsDetails.UpdateAchievementDetails(achievementCategoryData);
			}
			else if (m_lastPressedTab != null && (bool)uIAccordionCategoryTab)
			{
				uIAccordionCategoryTab.IsOpend = !uIAccordionCategoryTab.IsOpend;
			}
			StartAlignment();
			m_prevTab = component;
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
				for (int j = 0; j < m_children.Count; j++)
				{
					Transform childTrans = m_children[j].transform;
					childTrans.localPosition = new Vector3(Mathf.Round(childTrans.localPosition.x), Mathf.Round(childTrans.localPosition.y), Mathf.Round(childTrans.localPosition.z));
				}
			}
			while (!(m_delayLayoutUpdateTimer > 0.8f));
			m_delayLayoutUpdateTimer = 0f;
		}
	}
}
