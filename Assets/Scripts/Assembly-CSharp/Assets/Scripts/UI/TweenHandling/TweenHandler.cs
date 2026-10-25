using System;
using System.Collections.Generic;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UnityEngine;

namespace Assets.Scripts.UI.TweenHandling
{
	[Serializable]
	public class TweenHandler : MonoBehaviour
	{
		[Serializable]
		public class TweenData
		{
			[SerializeField]
			public int GroupId;

			[SerializeField]
			public bool IgnoreOnFinish;

			[SerializeField]
			public BBTargetedTween Tween;
		}

		private enum EPlayDirection
		{
			None = 0,
			Forward = 1,
			Reverse = 2
		}

		[SerializeField]
		private float m_startTimeDelay;

		[SerializeField]
		private List<TweenData> m_tweenRefContainer = new List<TweenData>();

		private readonly List<TweenData> m_allBBTweensOnTweenTarget = new List<TweenData>();

		private readonly List<int> m_tweenGroups = new List<int>();

		private float m_startTime;

		private int m_currentTweenGroupIndex = -1;

		private int m_activeTweens;

		private bool m_tweeningPausedAtGroupFinished;

		private bool m_startNextTweenPhase;

		private EPlayDirection m_playingDirection;

		private List<EventDelegate> m_onTweeningFinished = new List<EventDelegate>();

		private bool m_isStartingTweenGroup;

		private bool IsStarting { get; set; }

		public float StartTimeDelay
		{
			get
			{
				return m_startTimeDelay;
			}
			set
			{
				m_startTimeDelay = value;
			}
		}

		public bool IsTweening
		{
			get
			{
				return m_currentTweenGroupIndex >= 0 || IsStarting;
			}
		}

		public bool IsTweeningPaused
		{
			get
			{
				return m_tweeningPausedAtGroupFinished;
			}
			set
			{
				m_tweeningPausedAtGroupFinished = value;
				if (m_currentTweenGroupIndex >= 0)
				{
					TryStartNextTweenGroup();
				}
			}
		}

		public List<EventDelegate> OnTweeningFinished
		{
			get
			{
				return m_onTweeningFinished;
			}
		}

		public List<TweenData> TweenRefContainer
		{
			get
			{
				return m_tweenRefContainer;
			}
		}

		public List<TweenData> AllBBTweensOnTweensContainer
		{
			get
			{
				return m_allBBTweensOnTweenTarget;
			}
		}

		public bool IgnoreInactive { get; set; }

		[ContextMenu("SortList by GroupId")]
		public void SortListByGroupId()
		{
			TweenRefContainer.Sort(CompareGroupId);
			ValidateTweens();
		}

		public void StartTweeningForward()
		{
			m_playingDirection = EPlayDirection.Forward;
			m_tweenGroups.Sort(SortAscending);
			for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
			{
				TweenData tweenData = AllBBTweensOnTweensContainer[i];
				if (tweenData.Tween != null)
				{
					tweenData.Tween.PrepareTweenForStart();
				}
			}
			ResetTweenHandler();
			if (m_startTimeDelay <= 0f)
			{
				UpdateTweenPhase();
				return;
			}
			IsStarting = true;
			m_startTime = RealTime.time + m_startTimeDelay;
		}

		public void StartTweeningReverse()
		{
			m_playingDirection = EPlayDirection.Reverse;
			m_tweenGroups.Sort(SortDescending);
			ResetTweenHandler();
			if (m_startTimeDelay <= 0f)
			{
				UpdateTweenPhase();
				return;
			}
			IsStarting = true;
			m_startTime = RealTime.time + m_startTimeDelay;
		}

		public void ResetToBeginning()
		{
			ResetTweenHandler();
			for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
			{
				TweenData tweenData = AllBBTweensOnTweensContainer[i];
				if (tweenData.Tween != null)
				{
					tweenData.Tween.SetTweenToBeginning();
				}
			}
		}

		public void ResetToEnd()
		{
			ResetTweenHandler();
			for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
			{
				TweenData tweenData = AllBBTweensOnTweensContainer[i];
				if (tweenData.Tween != null)
				{
					tweenData.Tween.SetTweenToEnd();
				}
			}
		}

		public void RemoveAllTweendReferences()
		{
			m_onTweeningFinished.Clear();
			m_tweenGroups.Clear();
			AllBBTweensOnTweensContainer.Clear();
			TweenRefContainer.Clear();
		}

		public void DestroyAllTweenedReferences()
		{
			for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
			{
				TweenData tweenData = AllBBTweensOnTweensContainer[i];
				if (tweenData.Tween != null)
				{
					UnityEngine.Object.Destroy(tweenData.Tween);
				}
			}
			RemoveAllTweendReferences();
		}

		public void AddAllSiblingsAttachedToTween(BBTargetedTween _checkingTween, int _groupId, bool _ignoreOnFinish)
		{
			TweenData tweenData = new TweenData();
			tweenData.GroupId = _groupId;
			tweenData.IgnoreOnFinish = _ignoreOnFinish;
			tweenData.Tween = _checkingTween;
			TweenData tweenData2 = tweenData;
			TweenRefContainer.Add(tweenData2);
			if (tweenData2.Tween != null)
			{
				BBTargetedTween[] components = tweenData2.Tween.GetComponents<BBTargetedTween>();
				for (int i = 0; i < components.Length; i++)
				{
					tweenData = new TweenData();
					tweenData.Tween = components[i];
					tweenData.GroupId = tweenData2.GroupId;
					tweenData.IgnoreOnFinish = tweenData2.IgnoreOnFinish;
					TweenData tweenData3 = tweenData;
					tweenData3.Tween.ValidateTween();
					tweenData3.Tween.enabled = false;
					AddNewTweenGroupIfNotFound(tweenData3.GroupId);
					AllBBTweensOnTweensContainer.Add(tweenData3);
				}
			}
		}

		public void AddTweens(BBTargetedTween[] _connectedTweens, int _groupId, bool _ignoreOnFinish)
		{
			if (_connectedTweens == null || _connectedTweens.Length <= 0)
			{
				return;
			}
			for (int i = 0; i < _connectedTweens.Length; i++)
			{
				TweenData tweenData = new TweenData();
				tweenData.Tween = _connectedTweens[i];
				tweenData.GroupId = _groupId;
				tweenData.IgnoreOnFinish = _ignoreOnFinish;
				TweenData tweenData2 = tweenData;
				tweenData2.Tween.ValidateTween();
				tweenData2.Tween.enabled = false;
				if (i == 0)
				{
					TweenRefContainer.Add(tweenData2);
				}
				AddNewTweenGroupIfNotFound(tweenData2.GroupId);
				AllBBTweensOnTweensContainer.Add(tweenData2);
			}
		}

		public void ValidateAllTweens()
		{
			if (AllBBTweensOnTweensContainer == null)
			{
				return;
			}
			for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
			{
				TweenData tweenData = AllBBTweensOnTweensContainer[i];
				if (tweenData != null && tweenData.Tween != null)
				{
					tweenData.Tween.ValidateTween();
				}
			}
		}

		public void SetStartToCurrentValue()
		{
			if (AllBBTweensOnTweensContainer == null)
			{
				return;
			}
			for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
			{
				TweenData tweenData = AllBBTweensOnTweensContainer[i];
				if (tweenData != null && tweenData.Tween != null)
				{
					tweenData.Tween.SetStartToCurrentValue();
				}
			}
		}

		protected static int SortAscending(int a, int b)
		{
			return a.CompareTo(b);
		}

		protected static int SortDescending(int a, int b)
		{
			return -1 * a.CompareTo(b);
		}

		private void Awake()
		{
			ValidateTweens();
		}

		private void OnDisable()
		{
			ResetTweenHandler();
		}

		private void OnDestroy()
		{
			AllBBTweensOnTweensContainer.Clear();
			m_tweenGroups.Clear();
		}

		private int CompareGroupId(TweenData _tweenDataA, TweenData _tweenDataB)
		{
			return _tweenDataA.GroupId.CompareTo(_tweenDataB.GroupId);
		}

		private void ValidateTweens()
		{
			m_tweenGroups.Clear();
			AllBBTweensOnTweensContainer.Clear();
			for (int i = 0; i < TweenRefContainer.Count; i++)
			{
				TweenData tweenData = TweenRefContainer[i];
				if (tweenData.Tween != null)
				{
					BBTargetedTween[] components = tweenData.Tween.GetComponents<BBTargetedTween>();
					for (int j = 0; j < components.Length; j++)
					{
						TweenData tweenData2 = new TweenData();
						tweenData2.Tween = components[j];
						tweenData2.GroupId = tweenData.GroupId;
						tweenData2.IgnoreOnFinish = tweenData.IgnoreOnFinish;
						TweenData tweenData3 = tweenData2;
						tweenData3.Tween.ValidateTween();
						tweenData3.Tween.enabled = false;
						AddNewTweenGroupIfNotFound(tweenData3.GroupId);
						AllBBTweensOnTweensContainer.Add(tweenData3);
					}
				}
			}
			m_tweenGroups.Sort();
		}

		private void ResetTweenHandler()
		{
			if (m_currentTweenGroupIndex >= 0)
			{
				for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
				{
					TweenData tweenData = AllBBTweensOnTweensContainer[i];
					if (tweenData.Tween != null)
					{
						EventDelegate.Remove(tweenData.Tween.onFinished, OnFinished);
					}
				}
			}
			IsStarting = false;
			m_startTime = 0f;
			m_activeTweens = 0;
			m_currentTweenGroupIndex = -1;
			m_startNextTweenPhase = false;
			m_tweeningPausedAtGroupFinished = false;
		}

		private void AddNewTweenGroupIfNotFound(int _id)
		{
			for (int i = 0; i < m_tweenGroups.Count; i++)
			{
				if (m_tweenGroups[i] == _id)
				{
					return;
				}
			}
			m_tweenGroups.Add(_id);
		}

		private void OnFinished()
		{
			m_activeTweens--;
			if (m_activeTweens <= 0 && !m_isStartingTweenGroup)
			{
				TryStartNextTweenGroup();
			}
		}

		private void TryStartNextTweenGroup()
		{
			if (m_activeTweens <= 0 && !IsTweeningPaused)
			{
				m_startNextTweenPhase = true;
			}
		}

		private bool StartNextTweenGroup()
		{
			if (RealTime.time < m_startTime)
			{
				return true;
			}
			m_currentTweenGroupIndex++;
			if (m_currentTweenGroupIndex >= m_tweenGroups.Count)
			{
				return false;
			}
			m_isStartingTweenGroup = true;
			m_activeTweens = 0;
			int num = m_tweenGroups[m_currentTweenGroupIndex];
			for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
			{
				TweenData tweenData = AllBBTweensOnTweensContainer[i];
				if (!(tweenData.Tween != null) || (IgnoreInactive && (!IgnoreInactive || !tweenData.Tween.gameObject.activeInHierarchy)))
				{
					continue;
				}
				BBTargetedTween tween = tweenData.Tween;
				if (tweenData.GroupId == num && !tween.IsDeactivated)
				{
					if (!tweenData.IgnoreOnFinish)
					{
						m_activeTweens++;
						EventDelegate.Add(tween.onFinished, OnFinished, true);
					}
					if (m_playingDirection == EPlayDirection.Forward)
					{
						tween.StartTweenPlayForward();
					}
					else
					{
						tween.StartTweenPlayReverse();
					}
				}
			}
			m_isStartingTweenGroup = false;
			if (m_activeTweens <= 0)
			{
				return StartNextTweenGroup();
			}
			return true;
		}

		private void ResetFinnishedTweensStartState()
		{
			if (m_playingDirection != EPlayDirection.Reverse || m_currentTweenGroupIndex < 0 || m_currentTweenGroupIndex >= m_tweenGroups.Count)
			{
				return;
			}
			int num = m_tweenGroups[m_currentTweenGroupIndex];
			for (int i = 0; i < AllBBTweensOnTweensContainer.Count; i++)
			{
				TweenData tweenData = AllBBTweensOnTweensContainer[i];
				if (tweenData.Tween != null)
				{
					BBTargetedTween tween = tweenData.Tween;
					if (tweenData.GroupId == num && !tween.IsDeactivated && tween != null)
					{
						tween.PrepareTweenForStart();
					}
				}
			}
		}

		private void UpdateTweenPhase()
		{
			ResetFinnishedTweensStartState();
			m_startNextTweenPhase = false;
			if (StartNextTweenGroup())
			{
				return;
			}
			m_currentTweenGroupIndex = -1;
			if (m_onTweeningFinished != null)
			{
				List<EventDelegate> onTweeningFinished = m_onTweeningFinished;
				m_onTweeningFinished = new List<EventDelegate>();
				EventDelegate.Execute(onTweeningFinished);
				for (int i = 0; i < onTweeningFinished.Count; i++)
				{
					EventDelegate.Add(m_onTweeningFinished, onTweeningFinished[i]);
				}
			}
		}

		private void LateUpdate()
		{
			if (IsStarting && RealTime.time > m_startTime)
			{
				IsStarting = false;
				m_startNextTweenPhase = true;
			}
			if (m_startNextTweenPhase)
			{
				UpdateTweenPhase();
			}
		}
	}
}
