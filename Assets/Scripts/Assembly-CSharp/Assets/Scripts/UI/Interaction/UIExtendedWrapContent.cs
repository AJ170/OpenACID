using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Interaction
{
	[AddComponentMenu("NGUI/BB/Interaction/Extended Wrap Content")]
	public class UIExtendedWrapContent : MonoBehaviour, IExtendedWrapContent
	{
		[SerializeField]
		private Vector2 m_padding = Vector2.zero;

		[SerializeField]
		private int m_groupSize = 1;

		[SerializeField]
		private bool m_autoAlign;

		[SerializeField]
		private int m_itemCount;

		[SerializeField]
		private GameObject m_childPrefab;

		[SerializeField]
		private bool m_dontWrap;

		[SerializeField]
		private UIRect m_containerRect;

		[SerializeField]
		private bool m_useBounds;

		[SerializeField]
		private Vector2 m_offsetTop = Vector2.zero;

		private Transform m_cachedTransform;

		private UIPanel m_panel;

		private UIWrapScrollView m_wrapScrollView;

		private bool m_isHorizontal;

		private List<Transform> m_children = new List<Transform>();

		private UIFocusChildInScrollView m_focusChildInScrollView;

		public BBList<EventDelegate> OnFocusFinishedEvent = new BBList<EventDelegate>();

		private Bounds m_containerBounds = new Bounds(Vector3.zero, Vector3.zero);

		private Vector2 m_widgetSize = Vector2.zero;

		private int m_constrainedItemCount = -1;

		private int m_freeItemCount = -1;

		private int id;

		public UIPanel GetParentPanel
		{
			get
			{
				return m_panel;
			}
		}

		public UIWrapScrollView WrapScrollView
		{
			get
			{
				return CacheScrollView() ? m_wrapScrollView : null;
			}
		}

		public List<Transform> Children
		{
			get
			{
				return m_children;
			}
		}

		private Bounds ContainerBounds
		{
			get
			{
				if (m_containerBounds.size == Vector3.zero)
				{
					if (m_containerRect != null)
					{
						m_containerRect.Update();
					}
					else
					{
						m_panel.Update();
					}
					Vector3[] array = ((!(m_containerRect != null)) ? m_panel.localCorners : m_containerRect.localCorners);
					for (int i = 0; i < array.Length; i++)
					{
						m_containerBounds.Encapsulate(array[i]);
					}
				}
				return m_containerBounds;
			}
		}

		private Vector2 WidgetSize
		{
			get
			{
				if (m_widgetSize == Vector2.zero)
				{
					if (m_cachedTransform.childCount == 0)
					{
						CreateItem();
					}
					UIWidget component = m_cachedTransform.GetChild(0).GetComponent<UIWidget>();
					if (m_useBounds)
					{
						m_widgetSize = NGUIMath.CalculateRelativeWidgetBounds(component.transform, true).size;
					}
					else
					{
						m_widgetSize = component.localSize;
					}
				}
				return m_widgetSize;
			}
		}

		private UIRect Container
		{
			get
			{
				return m_containerRect ?? m_panel;
			}
		}

		public Vector2 AdjustedPadding
		{
			get
			{
				if (!m_autoAlign)
				{
					return m_padding;
				}
				float num = ((!m_isHorizontal) ? m_padding.x : m_padding.y);
				float num2 = ((!m_isHorizontal) ? ContainerBounds.size.x : ContainerBounds.size.y);
				float num3 = ((!m_isHorizontal) ? WidgetSize.x : WidgetSize.y);
				int constrainedItemCount = ConstrainedItemCount;
				float num4 = num2 - ((float)constrainedItemCount * num3 + (float)(constrainedItemCount - 1) * num);
				float num5 = num4 / (float)(constrainedItemCount - 1);
				return (!m_isHorizontal) ? new Vector2(m_padding.x + num5, m_padding.y) : new Vector2(m_padding.x, m_padding.y + num5);
			}
		}

		public int ConstrainedItemCount
		{
			get
			{
				if (!m_autoAlign)
				{
					return m_groupSize;
				}
				if (m_constrainedItemCount == -1)
				{
					float num = ((!m_isHorizontal) ? m_padding.x : m_padding.y);
					float num2 = ((!m_isHorizontal) ? ContainerBounds.size.x : ContainerBounds.size.y);
					float num3 = ((!m_isHorizontal) ? WidgetSize.x : WidgetSize.y);
					m_constrainedItemCount = (int)((num2 + num) / (num3 + num));
				}
				return m_constrainedItemCount;
			}
		}

		public int FreeItemCount
		{
			get
			{
				if (m_freeItemCount == -1 && !m_dontWrap)
				{
					float num = ((!m_isHorizontal) ? m_padding.y : m_padding.x);
					float num2 = ((!m_isHorizontal) ? ContainerBounds.size.y : ContainerBounds.size.x);
					float num3 = ((!m_isHorizontal) ? WidgetSize.y : WidgetSize.x);
					int num4 = Mathf.CeilToInt((num2 + num) / (num3 + num)) + 1;
					int num5 = (int)((num2 + num) % (num3 + num));
					if ((float)num5 <= num3 / 2f)
					{
						num4++;
					}
					m_freeItemCount = num4;
				}
				else if (m_dontWrap)
				{
					if (ItemCount > 0)
					{
						m_freeItemCount = Mathf.CeilToInt((float)ItemCount / (float)ConstrainedItemCount);
					}
					else
					{
						m_freeItemCount = 0;
					}
				}
				return m_freeItemCount;
			}
		}

		public bool MoveInstant
		{
			get
			{
				return FocusChildInScrollView != null && FocusChildInScrollView.MoveInstant;
			}
			set
			{
				if (FocusChildInScrollView != null)
				{
					FocusChildInScrollView.MoveInstant = value;
				}
			}
		}

		public int ItemCount
		{
			get
			{
				return m_itemCount;
			}
			set
			{
				bool flag = m_dontWrap && m_itemCount != value;
				m_itemCount = value;
				if (flag)
				{
					m_children.Clear();
					ValidateChildren();
				}
				SortBasedOnScrollMovement();
			}
		}

		public int GroupCount
		{
			get
			{
				return (ItemCount <= 0) ? 1 : Mathf.CeilToInt((float)ItemCount / (float)ConstrainedItemCount);
			}
		}

		public Vector2 TotalDimension
		{
			get
			{
				return GetDimensionByCount(GroupCount - 1);
			}
		}

		public List<Transform> SortedChildrenByPositionList
		{
			get
			{
				List<Transform> list = new List<Transform>(Children.Count);
				for (int i = 0; i < Children.Count; i++)
				{
					Transform transform = Children[i];
					if (transform.gameObject.activeSelf)
					{
						list.Add(transform);
					}
				}
				if (m_isHorizontal)
				{
					list.Sort(SortHorizontal);
				}
				else
				{
					list.Sort(SortVertical);
				}
				return list;
			}
		}

		public int Colums
		{
			get
			{
				return (!m_isHorizontal) ? ConstrainedItemCount : ((ItemCount > 0) ? Mathf.CeilToInt((float)ItemCount / (float)ConstrainedItemCount) : 0);
			}
		}

		public int Rows
		{
			get
			{
				return m_isHorizontal ? ConstrainedItemCount : ((ItemCount > 0) ? Mathf.CeilToInt((float)ItemCount / (float)ConstrainedItemCount) : 0);
			}
		}

		public Bounds Bounds
		{
			get
			{
				Bounds result = default(Bounds);
				if (m_children.Count > 0 && ItemCount > 0)
				{
					float x = (float)Colums * WidgetSize.x + (float)(Colums - 1) * AdjustedPadding.x + Mathf.Abs(m_offsetTop.x * 2f);
					float y = (float)Rows * WidgetSize.y + (float)(Rows - 1) * AdjustedPadding.y + Mathf.Abs(m_offsetTop.y * 2f);
					result.Encapsulate(new Vector3(x, y));
					return result;
				}
				return ContainerBounds;
			}
		}

		private Vector2 BlockSize
		{
			get
			{
				int num = ((!m_isHorizontal) ? ConstrainedItemCount : FreeItemCount);
				int num2 = ((!m_isHorizontal) ? FreeItemCount : ConstrainedItemCount);
				float x = (float)num * WidgetSize.x + (float)(num - 1) * AdjustedPadding.x;
				float y = (float)num2 * WidgetSize.y + (float)(num2 - 1) * AdjustedPadding.y;
				return new Vector2(x, y);
			}
		}

		public UIFocusChildInScrollView FocusChildInScrollView
		{
			get
			{
				if (m_focusChildInScrollView == null)
				{
					m_focusChildInScrollView = GetComponent<UIFocusChildInScrollView>();
					EventDelegate.Add(m_focusChildInScrollView.OnFinishedEvent, OnFocusFinished);
				}
				return m_focusChildInScrollView;
			}
		}

		public event Action<Transform, int> OnUpdateItem;

		public event Action<Transform> OnItemCreated;

		public Vector2 GetDimensionByCount(int _groupIndex)
		{
			int num = ((!m_isHorizontal) ? ConstrainedItemCount : (_groupIndex + 1));
			int num2 = ((!m_isHorizontal) ? (_groupIndex + 1) : ConstrainedItemCount);
			Vector2 result = default(Vector2);
			result.x = (float)num * WidgetSize.x + (float)Math.Max(num - 1, 0) * AdjustedPadding.x;
			result.y = (float)num2 * WidgetSize.y + (float)Math.Max(num2 - 1, 0) * AdjustedPadding.y;
			return result;
		}

		[ContextMenu("Init")]
		public void Init()
		{
			InitBounds();
			ValidateChildren();
			SortBasedOnScrollMovement();
			if (m_wrapScrollView != null)
			{
				m_wrapScrollView.GetComponent<UIPanel>().onClipMove = OnMove;
				m_wrapScrollView.restrictWithinPanel = true;
				m_wrapScrollView.disableDragIfFits = true;
				m_wrapScrollView.dragEffect = UIScrollView.DragEffect.MomentumAndSpring;
			}
		}

		private void InitBounds()
		{
			if (!(m_containerRect == null))
			{
				if (m_panel == null)
				{
					CacheScrollView();
				}
				if (m_isHorizontal)
				{
					m_containerRect.leftAnchor.target = m_panel.transform;
					m_containerRect.rightAnchor.target = m_panel.transform;
					m_containerRect.leftAnchor.absolute = (int)m_panel.clipSoftness.x;
					m_containerRect.rightAnchor.absolute = (int)(0f - m_panel.clipSoftness.x);
				}
				else
				{
					m_containerRect.bottomAnchor.target = m_panel.transform;
					m_containerRect.topAnchor.target = m_panel.transform;
					m_containerRect.bottomAnchor.absolute = (int)m_panel.clipSoftness.y;
					m_containerRect.topAnchor.absolute = (int)(0f - m_panel.clipSoftness.y);
				}
			}
		}

		public void SetFocusOnChildByIndex(int _index)
		{
			if (_index < ItemCount && FocusChildInScrollView != null)
			{
				int groupIndex = Mathf.FloorToInt((float)_index / (float)m_groupSize);
				Vector2 dimensionByCount = GetDimensionByCount(groupIndex);
				if (!m_isHorizontal)
				{
					dimensionByCount.y = 0f - dimensionByCount.y;
				}
				Vector3 position = m_panel.transform.InverseTransformPoint(m_cachedTransform.TransformPoint(dimensionByCount));
				Vector2 dimensionByCount2 = GetDimensionByCount(0);
				FocusChildInScrollView.FocusOn(position, dimensionByCount2);
			}
		}

		[ContextMenu("Sort Based on Scroll Movement")]
		public void SortBasedOnScrollMovement()
		{
			if (CacheScrollView())
			{
				m_constrainedItemCount = -1;
				m_freeItemCount = -1;
				m_children.Clear();
				for (int i = 0; i < m_cachedTransform.childCount; i++)
				{
					m_children.Add(m_cachedTransform.GetChild(i));
				}
				ResetChildPositions();
			}
		}

		public static int SortHorizontal(Transform _a, Transform _b)
		{
			if (Math.Abs(_a.localPosition.x - _b.localPosition.x) < 0.0001f)
			{
				return -_a.localPosition.y.CompareTo(_b.localPosition.y);
			}
			return _a.localPosition.x.CompareTo(_b.localPosition.x);
		}

		public static int SortVertical(Transform _a, Transform _b)
		{
			if (Math.Abs(_a.localPosition.y - _b.localPosition.y) < 0.0001f)
			{
				return _a.localPosition.x.CompareTo(_b.localPosition.x);
			}
			return _b.localPosition.y.CompareTo(_a.localPosition.y);
		}

		protected virtual void OnMove(UIPanel _panel)
		{
			WrapContent();
		}

		protected virtual void UpdateItem(Transform _item, int _index)
		{
			bool flag = _index < m_itemCount;
			if (flag && this.OnUpdateItem != null)
			{
				this.OnUpdateItem(_item, _index);
			}
			_item.gameObject.SetActive(flag);
		}

		private void OnDestroy()
		{
			if (m_focusChildInScrollView != null)
			{
				EventDelegate.Remove(m_focusChildInScrollView.OnFinishedEvent, OnFocusFinished);
			}
			m_children.Clear();
		}

		private void OnFocusFinished()
		{
			EventDelegate.Execute(OnFocusFinishedEvent);
		}

		private bool CacheScrollView()
		{
			if (m_panel != null)
			{
				return true;
			}
			m_cachedTransform = base.transform;
			m_panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			m_wrapScrollView = m_panel.GetComponent<UIWrapScrollView>();
			if (m_wrapScrollView == null)
			{
				return false;
			}
			if (m_wrapScrollView.movement == UIScrollView.Movement.Horizontal)
			{
				m_isHorizontal = true;
			}
			else
			{
				if (m_wrapScrollView.movement != UIScrollView.Movement.Vertical)
				{
					return false;
				}
				m_isHorizontal = false;
			}
			return true;
		}

		private Vector3 GetOriginPoint()
		{
			return Container.worldCorners[1];
		}

		private Vector3 GetOffset()
		{
			return (!(m_containerRect == null)) ? Vector3.zero : new Vector3(m_panel.clipSoftness.x, m_panel.clipSoftness.y, 0f);
		}

		private void WrapContent()
		{
			Vector3[] worldCorners = Container.worldCorners;
			for (int i = 0; i < worldCorners.Length; i++)
			{
				Vector3 position = worldCorners[i];
				position = m_cachedTransform.InverseTransformPoint(position);
				worldCorners[i] = position;
			}
			Vector3 vector = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
			Vector2 vector2 = (BlockSize + AdjustedPadding) / 2f;
			for (int j = 0; j < m_children.Count; j += ConstrainedItemCount)
			{
				Transform transform = m_children[j];
				Vector3 vector3 = transform.localPosition - vector;
				if (m_isHorizontal)
				{
					float num = vector3.x % (vector2.x * 2f);
					float x = vector3.x - num;
					Vector3 newPosition = transform.localPosition - new Vector3(x, 0f, 0f);
					if (vector3.x < 0f - vector2.x)
					{
						if (num < 0f - vector2.x)
						{
							newPosition.x += vector2.x * 2f;
						}
						if (newPosition.x < TotalDimension.x)
						{
							UpdateChildPosition(newPosition, j);
						}
					}
					else if (vector3.x > vector2.x)
					{
						if (num > vector2.x)
						{
							newPosition.x -= vector2.x * 2f;
						}
						if (newPosition.x >= 0f)
						{
							UpdateChildPosition(newPosition, j);
						}
					}
					continue;
				}
				float num2 = vector3.y % (vector2.y * 2f);
				float y = vector3.y - num2;
				Vector3 newPosition2 = transform.localPosition - new Vector3(0f, y, 0f);
				if (vector3.y < 0f - vector2.y)
				{
					if (num2 < 0f - vector2.y)
					{
						newPosition2.y += vector2.y * 2f;
					}
					if (newPosition2.y <= 0f)
					{
						UpdateChildPosition(newPosition2, j);
					}
				}
				else if (vector3.y > vector2.y)
				{
					if (num2 > vector2.y)
					{
						newPosition2.y -= vector2.y * 2f;
					}
					if (newPosition2.y > 0f - TotalDimension.y)
					{
						UpdateChildPosition(newPosition2, j);
					}
				}
			}
		}

		private void UpdateChildPosition(Vector3 _newPosition, int _childIndex)
		{
			int num = ((!m_isHorizontal) ? (Math.Abs((int)(_newPosition.y / (WidgetSize.y + AdjustedPadding.y))) * ConstrainedItemCount) : (Math.Abs((int)(_newPosition.x / (WidgetSize.x + AdjustedPadding.x))) * ConstrainedItemCount));
			for (int i = 0; i < ConstrainedItemCount; i++)
			{
				int num2 = num + i;
				Transform transform = m_children[_childIndex + i];
				bool flag = num2 < ItemCount;
				transform.gameObject.SetActive(flag);
				if (flag)
				{
					if (m_isHorizontal)
					{
						_newPosition.y = transform.localPosition.y;
					}
					else
					{
						_newPosition.x = transform.localPosition.x;
					}
					transform.localPosition = _newPosition;
					UpdateItem(transform, num2);
				}
			}
		}

		private void ResetChildPositions()
		{
			if (m_cachedTransform.childCount == 0)
			{
				CreateItem();
			}
			if (m_wrapScrollView != null)
			{
				m_wrapScrollView.ResetPosition();
			}
			UIWidget component = m_cachedTransform.GetChild(0).GetComponent<UIWidget>();
			m_cachedTransform.localPosition = m_cachedTransform.parent.InverseTransformPoint(GetOriginPoint()) + new Vector3(component.pivotOffset.x * WidgetSize.x + m_offsetTop.x, 0f - ((1f - component.pivotOffset.y) * WidgetSize.y + m_offsetTop.y), 0f) - GetOffset();
			int num = 0;
			int num2 = 0;
			while (num2 < m_children.Count)
			{
				for (int i = 0; i < ConstrainedItemCount; i++)
				{
					Transform transform = m_children[num2 + i];
					transform.gameObject.SetActive(true);
					Vector3 vector = default(Vector3);
					vector.x = ((!m_isHorizontal) ? ((float)i * WidgetSize.x + (float)i * AdjustedPadding.x) : ((float)num * WidgetSize.x + (float)num * AdjustedPadding.x));
					vector.y = ((!m_isHorizontal) ? (0f - (float)num * WidgetSize.y - (float)num * AdjustedPadding.y) : (0f - (float)i * WidgetSize.y - (float)i * AdjustedPadding.y));
					vector.z = 0f;
					Vector3 localPosition = vector;
					if (component.isAnchoredHorizontally)
					{
						localPosition.x = transform.localPosition.x;
					}
					else if (component.isAnchoredVertically)
					{
						localPosition.y = transform.localPosition.y;
					}
					transform.localPosition = localPosition;
				}
				num2 += ConstrainedItemCount;
				num++;
			}
			if (m_isHorizontal)
			{
				m_children.Sort(SortHorizontal);
			}
			else
			{
				m_children.Sort(SortVertical);
			}
			for (int j = 0; j < m_children.Count; j++)
			{
				UpdateItem(m_children[j], j);
			}
			if (m_wrapScrollView != null)
			{
				m_wrapScrollView.ExtendedWraperContent = this;
				m_wrapScrollView.UpdateScrollbars(true);
			}
		}

		private void ValidateChildren()
		{
			if (!(m_childPrefab == null) && CacheScrollView())
			{
				int num = (m_dontWrap ? m_itemCount : 0);
				if (num == 0)
				{
					num = ConstrainedItemCount * FreeItemCount;
				}
				int childCount = m_cachedTransform.childCount;
				for (int i = childCount; i < num; i++)
				{
					CreateItem();
				}
				SortBasedOnScrollMovement();
				WrapContent();
			}
		}

		private void CreateItem()
		{
			GameObject gameObject = NGUITools.AddChild(m_cachedTransform.gameObject, m_childPrefab);
			id++;
			if (this.OnItemCreated != null)
			{
				this.OnItemCreated(gameObject.transform);
			}
		}
	}
}
