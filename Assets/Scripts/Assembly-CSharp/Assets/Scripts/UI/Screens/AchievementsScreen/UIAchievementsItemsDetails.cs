using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Achievements;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AchievementsScreen
{
	public class UIAchievementsItemsDetails : MonoBehaviour
	{
		private const float c_animationNewAchievementWaitTime = 1.5f;

		[SerializeField]
		private UIWidget m_anchorTarget;

		[SerializeField]
		private UIExtendedWrapContent m_wraperContainer;

		[SerializeField]
		private UIShareOnSocialMediaBehaviour m_shareOnSocialMedia;

		private string m_lastCategoryId = string.Empty;

		private float m_animateNewTimeLeft;

		private int m_newItemAnimationIndex = -1;

		private readonly List<int> m_newAchievementsIndex = new List<int>();

		private readonly List<AchievementMapping.Definition> m_claimableAchievementsDefList = new List<AchievementMapping.Definition>();

		private List<AchievementMapping.Definition> m_achievementsDefList = new List<AchievementMapping.Definition>();

		private void Awake()
		{
			m_wraperContainer.OnUpdateItem += OnUpdateItem;
			m_wraperContainer.OnItemCreated += OnItemCreated;
			m_wraperContainer.Init();
		}

		public void UpdateAchievementList(AchievementCategoryData _categoryDate, bool _shouldReposition = true)
		{
			if (m_lastCategoryId != _categoryDate.CategoryId)
			{
				m_animateNewTimeLeft = 0f;
				m_newItemAnimationIndex = -1;
				m_newAchievementsIndex.Clear();
				m_claimableAchievementsDefList.Clear();
				m_lastCategoryId = _categoryDate.CategoryId;
			}
			m_achievementsDefList.Clear();
			_categoryDate.GetAllAchievements(ref m_achievementsDefList);
			SortAndCollectDataFromAchievementsDefList();
			if (_shouldReposition)
			{
				m_wraperContainer.ItemCount = m_achievementsDefList.Count + m_claimableAchievementsDefList.Count;
				if (m_wraperContainer.ItemCount > 0)
				{
					m_wraperContainer.WrapScrollView.ResetPosition();
				}
			}
			else
			{
				for (int i = 0; i < m_wraperContainer.Children.Count; i++)
				{
					UIAchievementsItemDetails component = m_wraperContainer.Children[i].GetComponent<UIAchievementsItemDetails>();
					if (component.Index >= 0)
					{
						OnUpdateItem(m_wraperContainer.Children[i], component.Index);
					}
				}
			}
			Update();
		}

		private void SortAndCollectDataFromAchievementsDefList()
		{
			m_achievementsDefList = m_achievementsDefList.Except(m_claimableAchievementsDefList);
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			int num = 0;
			while (num < m_achievementsDefList.Count)
			{
				AchievementMapping.Definition definition = m_achievementsDefList[num];
				AchievementCategoryData achievementCategoryData = achievementsManager.GetAchievementCategoryData(definition.categoryId);
				Achievement value = null;
				if (achievementCategoryData.IdToAchievements.TryGetValue(definition.id, out value))
				{
					AchievementRank nextAvailableAchievementRank = value.GetNextAvailableAchievementRank();
					if (nextAvailableAchievementRank.state == EAchievementState.AchievementClaimable)
					{
						m_claimableAchievementsDefList.Add(m_achievementsDefList[num]);
						if (!UbiCore.Utils.Singleton<LocalStorage>.Instance.IsAchievementRead(definition.id, nextAvailableAchievementRank.idx))
						{
							m_newAchievementsIndex.Add(m_claimableAchievementsDefList.Count - 1);
						}
						m_achievementsDefList.RemoveAt(num);
						continue;
					}
				}
				num++;
			}
		}

		private void Update()
		{
			if (m_newAchievementsIndex.Count > 0)
			{
				m_animateNewTimeLeft -= RealTime.deltaTime;
				if (m_animateNewTimeLeft < 0f)
				{
					m_newItemAnimationIndex = m_newAchievementsIndex[0];
					m_animateNewTimeLeft = 1.5f;
					EventDelegate.Add(m_wraperContainer.OnFocusFinishedEvent, OnItemFocused, true);
					m_wraperContainer.SetFocusOnChildByIndex(m_newItemAnimationIndex);
					m_newAchievementsIndex.RemoveAt(0);
				}
			}
		}

		private void OnItemFocused()
		{
			for (int i = 0; i < m_wraperContainer.Children.Count; i++)
			{
				Transform transform = m_wraperContainer.Children[i];
				UIAchievementsItemDetails component = transform.GetComponent<UIAchievementsItemDetails>();
				if (component.Index == m_newItemAnimationIndex)
				{
					m_newItemAnimationIndex = -1;
					component.ShowNewAchievementAnimation();
					break;
				}
			}
		}

		private void OnDisable()
		{
			m_lastCategoryId = string.Empty;
			m_animateNewTimeLeft = 0f;
			m_newItemAnimationIndex = -1;
			m_newAchievementsIndex.Clear();
			m_claimableAchievementsDefList.Clear();
			m_achievementsDefList.Clear();
		}

		private void OnDestroy()
		{
			if (!(m_wraperContainer != null))
			{
				return;
			}
			m_wraperContainer.OnUpdateItem -= OnUpdateItem;
			m_wraperContainer.OnItemCreated -= OnItemCreated;
			if (m_wraperContainer.Children == null)
			{
				return;
			}
			for (int i = 0; i < m_wraperContainer.Children.Count; i++)
			{
				Transform transform = m_wraperContainer.Children[i];
				if (transform != null)
				{
					UIAchievementsItemDetails component = transform.GetComponent<UIAchievementsItemDetails>();
					if (component != null)
					{
						component.OnItemClaimed = (Action<AchievementMapping.Definition>)Delegate.Remove(component.OnItemClaimed, new Action<AchievementMapping.Definition>(OnAchievementItemClaimed));
					}
				}
			}
		}

		private void OnItemCreated(Transform _transform)
		{
			UIAchievementsItemDetails component = _transform.GetComponent<UIAchievementsItemDetails>();
			if (component != null)
			{
				UIWidget component2 = component.GetComponent<UIWidget>();
				UIUtil.UpdateLeftAndRightAnchorsForTarget(component2, m_anchorTarget);
				component.OnItemClaimed = (Action<AchievementMapping.Definition>)Delegate.Combine(component.OnItemClaimed, new Action<AchievementMapping.Definition>(OnAchievementItemClaimed));
			}
		}

		private void OnUpdateItem(Transform _transform, int _index)
		{
			UIAchievementsItemDetails component = _transform.GetComponent<UIAchievementsItemDetails>();
			AchievementMapping.Definition definition = null;
			if (_index < m_claimableAchievementsDefList.Count)
			{
				definition = m_claimableAchievementsDefList[_index];
			}
			else
			{
				int num = _index - m_claimableAchievementsDefList.Count;
				if (num >= 0 && num < m_achievementsDefList.Count)
				{
					definition = m_achievementsDefList[num];
				}
			}
			if (definition != null)
			{
				bool flag = _index == m_newItemAnimationIndex;
				component.UpdateChild(definition, false, flag, _index);
				if (flag)
				{
					m_newItemAnimationIndex = -1;
				}
			}
		}

		private void OnAchievementItemClaimed(AchievementMapping.Definition _claimedItem)
		{
			m_shareOnSocialMedia.OnAchievementClaimed(_claimedItem);
		}
	}
}
