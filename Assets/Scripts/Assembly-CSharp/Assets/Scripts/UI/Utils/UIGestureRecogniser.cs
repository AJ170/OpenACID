using System;
using System.Collections.Generic;
using UbiCore;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class UIGestureRecogniser : MonoBehaviour
	{
		public delegate void OnDeltaDelegate(Vector2 _swipeDelta);

		public delegate void OnDeltaBoolDelegate(Vector2 _swipeDelta, bool _wasDragging);

		public delegate void VoidDelegate();

		private const int GatherInfoCount = 0;

		private const float TrackTime = 0.17f;

		[SerializeField]
		private bool m_allowSwipe;

		public List<EventDelegate> OnSwipeNext = new List<EventDelegate>();

		public List<EventDelegate> OnSwipePrevious = new List<EventDelegate>();

		public float SwipeThresholdTime = 0.52f;

		public float SwipeThreshold = 15.4f;

		public float AcknowledgeThreshold;

		public float DistanceRecogniseSwipeThreshold = 45f;

		private float m_totalDistance;

		private float m_smoothDistance;

		private bool m_isSwipe;

		private bool m_isPressed;

		private bool m_shouldGatherInfo = true;

		private int m_gatherInfoTick;

		private Vector2 m_trackedDelta;

		private float m_trackTimeTick;

		private float m_swipeThresholdTimeTick;

		private float m_dpi;

		public Vector2 SmoothDelta { get; private set; }

		public Vector2 SmoothDeltaDpiAdjusted
		{
			get
			{
				return SmoothDelta / m_dpi;
			}
		}

		public bool IsDragging { get; private set; }

		public event OnDeltaBoolDelegate OnSwipeCallback;

		public event OnDeltaDelegate OnDragCallback;

		public event VoidDelegate OnDragStarted;

		public event VoidDelegate OnDragEnded;

		private void Awake()
		{
			m_dpi = ((!MathHelper.Approximately(Screen.dpi, 0f)) ? Screen.dpi : 264f);
		}

		private void Update()
		{
			if (m_isPressed)
			{
				m_swipeThresholdTimeTick += Time.deltaTime;
				if (m_swipeThresholdTimeTick > SwipeThresholdTime)
				{
					m_isSwipe = false;
				}
				if (m_swipeThresholdTimeTick > SwipeThresholdTime * 1.6f)
				{
					IsDragging = true;
				}
			}
			else
			{
				m_swipeThresholdTimeTick = 0f;
			}
			if (!m_isPressed || (!IsDragging && !m_isSwipe))
			{
				return;
			}
			if (SmoothDelta == m_trackedDelta)
			{
				m_trackTimeTick += Time.deltaTime;
				if (m_trackTimeTick >= 0.17f)
				{
					bool isDragging = IsDragging;
					Reset();
					IsDragging = isDragging;
				}
			}
			else
			{
				m_trackTimeTick = 0f;
			}
			m_trackedDelta = SmoothDelta;
		}

		private void Reset()
		{
			m_swipeThresholdTimeTick = 0f;
			m_isSwipe = false;
			IsDragging = false;
			m_shouldGatherInfo = true;
			m_gatherInfoTick = 0;
			m_trackTimeTick = 0f;
			SmoothDelta = Vector2.zero;
			m_smoothDistance = 0f;
			m_totalDistance = 0f;
		}

		private void OnDragStart()
		{
			if (this.OnDragStarted != null)
			{
				this.OnDragStarted();
			}
		}

		private void OnDragEnd()
		{
			if (this.OnDragEnded != null)
			{
				this.OnDragEnded();
			}
		}

		private void OnPress(bool _isPressed)
		{
			m_isPressed = _isPressed;
			bool flag = m_swipeThresholdTimeTick < SwipeThresholdTime;
			if (!m_isPressed && (m_isSwipe || (IsDragging && m_smoothDistance > SwipeThreshold + SwipeThreshold * 0.45f) || (flag && m_totalDistance > DistanceRecogniseSwipeThreshold)))
			{
				if (this.OnSwipeCallback != null)
				{
					this.OnSwipeCallback(SmoothDelta, IsDragging);
				}
				if (SmoothDelta.x < 0f)
				{
					if (OnSwipeNext.Count > 0)
					{
						EventDelegate.Execute(OnSwipeNext);
					}
				}
				else if (OnSwipePrevious.Count > 0)
				{
					EventDelegate.Execute(OnSwipePrevious);
				}
			}
			if (!m_isPressed)
			{
				Reset();
			}
		}

		private void OnDrag(Vector2 _delta)
		{
			if (!m_isPressed)
			{
				return;
			}
			SmoothDelta = SmoothDelta * 0.6f + _delta * 0.4f;
			if (m_shouldGatherInfo)
			{
				if (m_gatherInfoTick < 0)
				{
					m_gatherInfoTick++;
					return;
				}
				m_shouldGatherInfo = false;
				m_gatherInfoTick = 0;
			}
			m_smoothDistance = m_smoothDistance * 0.6f + Math.Abs(Vector2.Distance(Vector2.zero, SmoothDelta) * Time.deltaTime * 100f) * 0.4f;
			m_totalDistance += m_smoothDistance;
			if (m_smoothDistance < AcknowledgeThreshold || (m_isSwipe && (double)m_smoothDistance < (double)SwipeThreshold * 0.22))
			{
				m_isSwipe = false;
				IsDragging = false;
			}
			else
			{
				if (m_isSwipe)
				{
					return;
				}
				if (!IsDragging)
				{
					if (m_smoothDistance > SwipeThreshold && m_allowSwipe)
					{
						m_isSwipe = true;
					}
					else
					{
						IsDragging = true;
					}
				}
				if (IsDragging && this.OnDragCallback != null)
				{
					this.OnDragCallback(SmoothDeltaDpiAdjusted);
				}
			}
		}
	}
}
