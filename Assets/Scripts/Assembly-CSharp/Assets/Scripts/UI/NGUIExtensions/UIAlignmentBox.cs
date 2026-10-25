using System.Collections.Generic;
using UbiCore;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions
{
	[AddComponentMenu("NGUI/Interaction/Alignment Box")]
	public class UIAlignmentBox : UIWidgetContainer
	{
		public enum EAlignment
		{
			Horizontal = 0,
			Vertical = 1
		}

		public EAlignment Alignment;

		public UIWidget.Pivot PivotAlignment = UIWidget.Pivot.Center;

		public Vector2 MinPadding;

		public bool StrechWidget = true;

		public bool StartAndEndWithPadding = true;

		public bool AlignToPivot;

		public bool Sorted;

		public bool HideInactive = true;

		public bool IgnoreChildren;

		private bool m_reposition;

		private bool m_initDone;

		public List<EventDelegate> OnReposition = new List<EventDelegate>();

		private readonly List<Transform> m_children = new List<Transform>();

		public Vector2 Size { get; private set; }

		public bool RepositionNow
		{
			get
			{
				return m_reposition;
			}
			set
			{
				if (value)
				{
					m_reposition = true;
					base.enabled = true;
				}
			}
		}

		public List<Transform> Children
		{
			get
			{
				if (m_children.Count == 0)
				{
					Transform transform = base.transform;
					m_children.Clear();
					for (int i = 0; i < transform.childCount; i++)
					{
						Transform child = transform.GetChild(i);
						if ((bool)child && (bool)child.gameObject && (!HideInactive || child.gameObject.activeSelf))
						{
							m_children.Add(child);
						}
					}
					if (Sorted)
					{
						m_children.Sort(SortByName);
					}
				}
				return m_children;
			}
		}

		protected static int SortByName(Transform a, Transform b)
		{
			return string.Compare(a.name, b.name);
		}

		[ContextMenu("Execute")]
		public void Reposition()
		{
			if (Application.isPlaying && !m_initDone && NGUITools.GetActive(this))
			{
				m_reposition = true;
				return;
			}
			if (!m_initDone)
			{
				Init();
			}
			m_reposition = false;
			m_children.Clear();
			RepositionChildren(Children);
			EventDelegate.Execute(OnReposition);
		}

		private void Init()
		{
			m_initDone = true;
		}

		private void Start()
		{
			Init();
			m_reposition = true;
		}

		private void OnDestroy()
		{
			m_children.Clear();
		}

		private void Update()
		{
			if (m_reposition)
			{
				Reposition();
			}
			base.enabled = false;
		}

		private void RepositionChildren(List<Transform> _children)
		{
			float num = 0f;
			float num2 = 0f;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			UIWidget uIWidget = base.gameObject.AddComponentIfNotExist<UIWidget>();
			uIWidget.ForceUpdate(true);
			float num3 = uIWidget.width;
			float num4 = uIWidget.height;
			bool flag = Alignment == EAlignment.Horizontal;
			int num5 = 0;
			Bounds[] array = new Bounds[_children.Count];
			for (int i = 0; i < _children.Count; i++)
			{
				Transform transform = _children[i];
				Bounds bounds = ((!IgnoreChildren) ? NGUIMath.CalculateRelativeWidgetBounds(transform, !HideInactive) : transform.GetComponent<UIWidget>().CalculateBounds());
				Vector3 localScale = transform.localScale;
				bounds.min = Vector3.Scale(bounds.min, localScale);
				bounds.max = Vector3.Scale(bounds.max, localScale);
				array[i] = bounds;
				zero.x += bounds.size.x;
				zero2.y = Mathf.Max(zero2.y, bounds.size.y);
				zero.y += bounds.size.y;
				zero2.x = Mathf.Max(zero2.x, bounds.size.x);
				num5++;
			}
			int num6 = num5 - 1;
			if (StartAndEndWithPadding)
			{
				num6 += 2;
			}
			float num7 = 0f;
			float num8 = 0f;
			Vector2 minPadding = MinPadding;
			if (StrechWidget)
			{
				num4 = (int)((!flag) ? zero.y : zero2.y);
				num3 = (int)((!flag) ? zero2.x : zero.x);
				if (flag)
				{
					num4 += (float)(int)(2f * MinPadding.y);
					num3 += (float)(int)((float)num6 * MinPadding.x);
				}
				else
				{
					num3 += (float)(int)(2f * MinPadding.x);
					num4 += (float)(int)((float)num6 * MinPadding.y);
				}
				num7 = (float)uIWidget.minWidth - num3;
				num8 = (float)uIWidget.minHeight - num4;
				if (uIWidget.leftAnchor.target == null || uIWidget.rightAnchor.target == null)
				{
					uIWidget.width = (int)num3;
				}
				if (uIWidget.bottomAnchor.target == null || uIWidget.topAnchor.target == null)
				{
					uIWidget.height = (int)num4;
				}
			}
			Vector2 pivotOffset = uIWidget.pivotOffset;
			pivotOffset.y = Mathf.Abs(pivotOffset.y - 1f);
			if (flag)
			{
				num = (float)(-uIWidget.width) * pivotOffset.x;
			}
			else
			{
				num2 = (float)uIWidget.height * pivotOffset.y;
			}
			Vector2 pivotOffset2 = NGUIMath.GetPivotOffset(PivotAlignment);
			if (StrechWidget)
			{
				if (flag)
				{
					if (AlignToPivot && num7 > 0f)
					{
						num += num7 * pivotOffset2.x;
					}
					num += (num3 - (zero.x + (float)num6 * MinPadding.x)) / 2f;
					num2 += MinPadding.y * pivotOffset.y * 2f;
				}
				else
				{
					if (AlignToPivot && num8 > 0f)
					{
						num2 -= num8 * pivotOffset2.y;
					}
					num2 -= (num4 - (zero.y + (float)num6 * MinPadding.y)) / 2f;
					num -= MinPadding.x * pivotOffset.x * 2f;
				}
			}
			else if (flag)
			{
				float num9 = num3 - zero.x;
				if (AlignToPivot)
				{
					num9 -= (float)num6 * minPadding.x;
					if (num9 > 0f)
					{
						num += num9 * pivotOffset2.x;
					}
				}
				else
				{
					float b = ((num6 != 0) ? (num9 / (float)num6) : 0f);
					minPadding.x = Mathf.Max(MinPadding.x, b);
				}
				if (MathHelper.Approximately(pivotOffset.y, 0.5f))
				{
					minPadding.y = Mathf.Max(MinPadding.y, (num4 - zero2.y) / 2f);
				}
				num2 += minPadding.y * pivotOffset.y * 2f;
			}
			else
			{
				float num10 = num4 - zero.y;
				if (AlignToPivot)
				{
					num10 -= (float)num6 * minPadding.y;
					if (num10 > 0f)
					{
						num2 -= num10 * pivotOffset2.y;
					}
				}
				else
				{
					float b2 = ((num6 != 0) ? (num10 / (float)num6) : 0f);
					minPadding.y = Mathf.Max(MinPadding.y, b2);
				}
				if (MathHelper.Approximately(pivotOffset.x, 0.5f))
				{
					minPadding.x = Mathf.Max(MinPadding.x, (num3 - zero2.x) / 2f);
				}
				num -= minPadding.x * pivotOffset.x * 2f;
			}
			int num11 = 0;
			for (int j = 0; j < _children.Count; j++)
			{
				Transform transform2 = _children[j];
				if (!transform2.gameObject.activeSelf)
				{
					continue;
				}
				Bounds bounds2 = array[j];
				float num12 = 0f;
				float num13 = 0f;
				if (flag)
				{
					num13 = minPadding.y;
					if (StartAndEndWithPadding || num11 != 0)
					{
						num12 = minPadding.x;
					}
				}
				else
				{
					num12 = minPadding.x;
					if (StartAndEndWithPadding || num11 != 0)
					{
						num13 = minPadding.y;
					}
				}
				UIWidget component = transform2.gameObject.GetComponent<UIWidget>();
				float num14 = 0f;
				float num15 = 0f;
				if (component != null)
				{
					if (flag)
					{
						num14 = bounds2.size.x * component.pivotOffset.x;
						float childPivot = Mathf.Abs(component.pivotOffset.y - 1f);
						float childOffsetFactorToAlignWithParrentPivot = GetChildOffsetFactorToAlignWithParrentPivot(childPivot, pivotOffset.y);
						num15 = bounds2.size.y * childOffsetFactorToAlignWithParrentPivot;
					}
					else
					{
						float childOffsetFactorToAlignWithParrentPivot2 = GetChildOffsetFactorToAlignWithParrentPivot(component.pivotOffset.x, pivotOffset.x);
						num14 = bounds2.size.x * childOffsetFactorToAlignWithParrentPivot2;
						num15 = bounds2.max.y;
					}
				}
				Vector3 localPosition = transform2.localPosition;
				localPosition.x = Mathf.Round(num + num12 + num14);
				localPosition.y = Mathf.Round(num2 - num13 - num15);
				if (flag)
				{
					num += bounds2.size.x + num12;
				}
				else
				{
					num2 -= bounds2.size.y + num13;
				}
				transform2.localPosition = localPosition;
				num11++;
			}
			Size = new Vector2(num3, num4);
		}

		private float GetChildOffsetFactorToAlignWithParrentPivot(float _childPivot, float _parentOffset)
		{
			_childPivot = (MathHelper.Approximately(_parentOffset, 0.5f) ? (MathHelper.Approximately(_childPivot, 0.5f) ? 0f : ((!(_childPivot <= 0f)) ? 0.5f : (-0.5f))) : ((_parentOffset <= 0f) ? (MathHelper.Approximately(_childPivot, 0.5f) ? 0.5f : ((!(_childPivot <= 0f)) ? 1f : 0f)) : (MathHelper.Approximately(_childPivot, 0.5f) ? (-0.5f) : ((!(_childPivot <= 0f)) ? 0f : (-1f)))));
			return _childPivot;
		}

		private void LateUpdate()
		{
			if (m_reposition)
			{
				Reposition();
			}
			base.enabled = false;
		}
	}
}
