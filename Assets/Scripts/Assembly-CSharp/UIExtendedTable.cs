using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/BB/Interaction/Extended Table")]
public class UIExtendedTable : UITable
{
	public delegate void OnSort(List<Transform> _list);

	[SerializeField]
	private int m_rows;

	public event OnSort OnSortTable;

	protected override void Sort(List<Transform> list)
	{
		if (this.OnSortTable != null)
		{
			this.OnSortTable(list);
		}
		else
		{
			list.Sort(UIGrid.SortByName);
		}
	}

	protected void RepositionByRowVariableSize(List<Transform> _children)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = ((m_rows <= 0) ? 1 : m_rows);
		int num4 = ((m_rows <= 0) ? base.Children.Count : (base.Children.Count / num3 + 1));
		Bounds[,] array = new Bounds[num4, num3];
		Bounds[] array2 = new Bounds[num3];
		Bounds[] array3 = new Bounds[num4];
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		for (int count = base.Children.Count; i < count; i++)
		{
			Transform transform = base.Children[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform, !hideInactive);
			Vector3 localScale = transform.localScale;
			bounds.min = Vector3.Scale(bounds.min, localScale);
			bounds.max = Vector3.Scale(bounds.max, localScale);
			array[num5, num6] = bounds;
			array2[num6].Encapsulate(bounds);
			array3[num5].Encapsulate(bounds);
			num6++;
			if (num6 >= num3 && num3 > 0)
			{
				num6 = 0;
				num5++;
			}
		}
		num5 = 0;
		num6 = 0;
		int j = 0;
		for (int count2 = base.Children.Count; j < count2; j++)
		{
			Transform transform2 = base.Children[j];
			Bounds bounds2 = array[num5, num6];
			Bounds bounds3 = array2[num6];
			Bounds bounds4 = array3[num5];
			Vector3 localPosition = transform2.localPosition;
			localPosition.x = num + bounds2.extents.x - bounds2.center.x;
			localPosition.x += bounds2.min.x - bounds3.min.x + padding.x;
			if (direction == Direction.Down)
			{
				localPosition.y = 0f - num2 - bounds2.extents.y - bounds2.center.y;
				localPosition.y += (bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y) * 0.5f - padding.y;
			}
			else
			{
				localPosition.y = num2 + (bounds2.extents.y - bounds2.center.y);
				localPosition.y -= (bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y) * 0.5f - padding.y;
			}
			num2 += bounds4.size.y + padding.y * 2f;
			transform2.localPosition = localPosition;
			num6++;
			if (num6 >= num3 && num3 > 0)
			{
				num6 = 0;
				num5++;
				num2 = 0f;
				num += bounds4.size.x + padding.x * 2f;
			}
		}
	}

	[ContextMenu("Execute")]
	public override void Reposition()
	{
		if (Application.isPlaying && !mInitDone && NGUITools.GetActive(this))
		{
			mReposition = true;
			return;
		}
		if (!mInitDone)
		{
			Init();
		}
		mReposition = false;
		Transform target = base.transform;
		m_children.Clear();
		List<Transform> children = base.Children;
		if (children.Count > 0)
		{
			RepositionByRowVariableSize(children);
		}
		if (keepWithinPanel && mPanel != null)
		{
			mPanel.ConstrainTargetToBounds(target, true);
			UIScrollView component = mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		if (onReposition != null)
		{
			onReposition();
		}
	}
}
