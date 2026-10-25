using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	[AddComponentMenu("Focus Child In ScrollView")]
	public class UIFocusChildInScrollView : MonoBehaviour
	{
		[SerializeField]
		private float m_springStrength = 8f;

		[SerializeField]
		private UIScrollView m_scrollView;

		[HideInInspector]
		public List<EventDelegate> OnFinishedEvent = new List<EventDelegate>();

		private bool m_springPanelOnFinishedEventAdded;

		private Bounds m_focusBounds;

		private bool m_focusChildNow;

		private bool m_init;

		private bool m_moveInstant;

		public SpringPanel.OnProgress onProgress;

		public bool FocusIsCentered { get; set; }

		public float SpringStrength
		{
			get
			{
				return m_springStrength;
			}
			set
			{
				m_springStrength = value;
			}
		}

		public bool StartFocusOffscreenAutoReset { get; set; }

		public bool MoveInstant
		{
			get
			{
				return m_moveInstant;
			}
			set
			{
				m_moveInstant = value;
			}
		}

		public Transform FocusedObject
		{
			set
			{
				if (value != null)
				{
					FocusOn(value);
				}
			}
		}

		public Bounds FocusOnBounds
		{
			get
			{
				return m_focusBounds;
			}
			set
			{
				m_focusBounds = value;
				m_focusChildNow = true;
				if (MoveInstant)
				{
					Update();
				}
			}
		}

		public void Start()
		{
			m_init = true;
		}

		public void OnDestroy()
		{
			OnFinishedEvent.Clear();
		}

		public void FocusOn(Vector3 _position, Vector3 _dimension)
		{
			if (!(m_scrollView == null) && !(m_scrollView.panel == null))
			{
				Bounds focusOnBounds = new Bounds(_position, _dimension);
				FocusOnBounds = focusOnBounds;
			}
		}

		public void FocusOn(Transform _target)
		{
			if (!(m_scrollView == null) && !(m_scrollView.panel == null))
			{
				Bounds focusOnBounds = NGUIMath.CalculateRelativeWidgetBounds(m_scrollView.panel.cachedTransform, _target);
				FocusOnBounds = focusOnBounds;
			}
		}

		private void Update()
		{
			if (m_focusChildNow && (MoveInstant || m_init))
			{
				m_focusChildNow = false;
				FocusOn(FocusOnBounds, FocusIsCentered);
			}
			else
			{
				m_scrollView.MoveRelative(Vector3.zero);
			}
		}

		private void FocusOn(Bounds _bounds, bool _center = false)
		{
			Vector3 vector = (_center ? (-GetMovementToCenterBoundsInPanel(_bounds, m_scrollView.panel)) : (-m_scrollView.panel.CalculateConstrainOffset(_bounds.min, _bounds.max)));
			if (!m_scrollView.canMoveHorizontally)
			{
				vector.x = 0f;
			}
			if (!m_scrollView.canMoveVertically)
			{
				vector.y = 0f;
			}
			vector.z = 0f;
			if (m_moveInstant)
			{
				m_scrollView.MoveRelative(-vector);
				m_scrollView.RestrictWithinBounds(true, m_scrollView.canMoveHorizontally, m_scrollView.canMoveVertically);
				EventDelegate.Execute(OnFinishedEvent);
				return;
			}
			Vector3 vector2 = ConstrainTargetBeforeMoveToPannelBounds(vector);
			if (!m_scrollView.canMoveHorizontally)
			{
				vector2.x = 0f;
			}
			if (!m_scrollView.canMoveVertically)
			{
				vector2.y = 0f;
			}
			Vector3 pos = m_scrollView.panel.cachedTransform.localPosition - vector2;
			if (StartFocusOffscreenAutoReset)
			{
				Vector3 zero = Vector3.zero;
				zero.y = (zero.z = 0f);
				zero.x = m_scrollView.panel.width;
				m_scrollView.MoveRelative(zero);
				StartFocusOffscreenAutoReset = false;
			}
			SpringPanel springPanel = SpringPanel.Begin(m_scrollView.panel.cachedGameObject, pos, m_springStrength);
			if (!m_springPanelOnFinishedEventAdded)
			{
				springPanel.onFinished = (SpringPanel.OnFinished)Delegate.Combine(springPanel.onFinished, new SpringPanel.OnFinished(OnFinishedSpringPanel));
				springPanel.onProgress = (SpringPanel.OnProgress)Delegate.Combine(springPanel.onProgress, new SpringPanel.OnProgress(OnProgressSpringPanel));
			}
		}

		private Vector3 GetMovementToCenterBoundsInPanel(Bounds _bounds, UIPanel _panel)
		{
			return PanelBounds(_panel).center - _bounds.center;
		}

		private Bounds PanelBounds(UIPanel _panel)
		{
			Vector4 finalClipRegion = _panel.finalClipRegion;
			float num = finalClipRegion.z * 0.5f;
			float num2 = finalClipRegion.w * 0.5f;
			Vector2 vector = new Vector2(finalClipRegion.x - num, finalClipRegion.y - num2);
			Vector2 vector2 = new Vector2(finalClipRegion.x + num, finalClipRegion.y + num2);
			if (_panel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				vector.x += _panel.clipSoftness.x;
				vector.y += _panel.clipSoftness.y;
				vector2.x -= _panel.clipSoftness.x;
				vector2.y -= _panel.clipSoftness.y;
			}
			float num3 = vector2.x - vector.x;
			float num4 = vector2.y - vector.y;
			return new Bounds(new Vector3(vector.x + num3 * 0.5f, vector.y + num4 * 0.5f), new Vector3(num3, num4));
		}

		private Vector3 ConstrainTargetBeforeMoveToPannelBounds(Vector3 _moveOffset)
		{
			if ((m_scrollView.canMoveVertically && !m_scrollView.shouldMoveVertically) || (m_scrollView.canMoveHorizontally && !m_scrollView.shouldMoveHorizontally))
			{
				return Vector3.zero;
			}
			UIPanel panel = m_scrollView.panel;
			Bounds bounds = m_scrollView.bounds;
			Vector4 finalClipRegion = panel.finalClipRegion;
			finalClipRegion.x += _moveOffset.x;
			finalClipRegion.y += _moveOffset.y;
			float num = finalClipRegion.z * 0.5f;
			float num2 = finalClipRegion.w * 0.5f;
			Vector2 minRect = new Vector2(bounds.min.x, bounds.min.y);
			Vector2 maxRect = new Vector2(bounds.max.x, bounds.max.y);
			Vector2 minArea = new Vector2(finalClipRegion.x - num, finalClipRegion.y - num2);
			Vector2 maxArea = new Vector2(finalClipRegion.x + num, finalClipRegion.y + num2);
			if (panel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				Vector2 clipSoftness = panel.clipSoftness;
				minArea.x += clipSoftness.x;
				minArea.y += clipSoftness.y;
				maxArea.x -= clipSoftness.x;
				maxArea.y -= clipSoftness.y;
			}
			Vector3 vector = NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
			return _moveOffset - vector;
		}

		private void OnFinishedSpringPanel()
		{
			EventDelegate.Execute(OnFinishedEvent);
			SpringPanel current = SpringPanel.current;
			current.onFinished = (SpringPanel.OnFinished)Delegate.Remove(current.onFinished, new SpringPanel.OnFinished(OnFinishedSpringPanel));
			SpringPanel current2 = SpringPanel.current;
			current2.onProgress = (SpringPanel.OnProgress)Delegate.Remove(current2.onProgress, new SpringPanel.OnProgress(OnProgressSpringPanel));
			m_springPanelOnFinishedEventAdded = false;
		}

		private void OnProgressSpringPanel(float _progress)
		{
			if (onProgress != null)
			{
				onProgress(_progress);
			}
		}
	}
}
