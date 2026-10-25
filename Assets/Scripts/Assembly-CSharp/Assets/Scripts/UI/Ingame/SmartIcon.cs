using System;
using UbiCore;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame
{
	public abstract class SmartIcon : IconAttachment
	{
		public const int DefaultPriority = 0;

		public const int MaxIconPriority = 9;

		private Func<bool> m_VisibilityPredicate;

		private bool m_VisibleUser = true;

		protected bool m_VisibleCached;

		private int[] m_Icons = new int[10];

		private int m_TopPriority;

		public float m_Alpha = 1f;

		public virtual bool ShouldBeUpdated { get; protected set; }

		public float SparseUpdateIntervalSeconds { get; protected set; }

		public int TopPriority
		{
			get
			{
				return m_TopPriority;
			}
		}

		public IconMaterial TopIcon
		{
			get
			{
				return (IconMaterial)m_Icons[m_TopPriority];
			}
		}

		public virtual bool IsHiddenOnBorders { get; set; }

		public float FadingTime { get; set; }

		public bool Visible
		{
			get
			{
				return m_VisibleCached;
			}
			set
			{
				m_VisibleUser = value;
			}
		}

		public Func<bool> VisibilityPredicate
		{
			get
			{
				return m_VisibilityPredicate;
			}
			set
			{
				m_VisibilityPredicate = value;
			}
		}

		protected SmartIcon()
		{
			FadingTime = 0.3f;
			IsHiddenOnBorders = true;
			ShouldBeUpdated = true;
			SparseUpdateIntervalSeconds = float.MaxValue;
		}

		public void SetIcon(IconMaterial _icon, int _priority = 0)
		{
			m_Icons[_priority] = (int)_icon;
			ShouldBeUpdated = true;
		}

		public IconMaterial GetIcon(int _priority)
		{
			return (IconMaterial)m_Icons[_priority];
		}

		public override void Release()
		{
			base.Release();
			m_Alpha = 0f;
		}

		public override bool OnTap(NavMeshAgent _agent)
		{
			if (!Visible)
			{
				return false;
			}
			return base.OnTap(_agent);
		}

		public void Update()
		{
			UpdateVisibility();
			bool flag = m_display != null;
			if (Visible)
			{
				base.SetIcon(TopIcon);
			}
			float num = (float)((Visible && (!IsHiddenOnBorders || !base.IsOnBorder)) ? 1 : (-1)) / FadingTime * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			m_Alpha += num;
			m_Alpha = m_Alpha.Clamp(0f, 1f);
			if (m_Alpha <= 0f && !Visible)
			{
				base.SetIcon(IconMaterial.Invalid);
				ShouldBeUpdated = false;
			}
			else
			{
				SetProperties(m_Alpha);
			}
			if (!flag && m_display != null)
			{
				m_display.LateUpdate();
				m_display.Alpha = 0f;
			}
		}

		public void UpdateVisibility()
		{
			bool visibleCached = m_VisibleCached;
			UpdateVisibilityInternal();
			if (!visibleCached && m_VisibleCached)
			{
				ShouldBeUpdated = true;
			}
		}

		protected virtual void UpdateVisibilityInternal()
		{
			m_TopPriority = 0;
			for (int num = m_Icons.Length - 1; num >= 0; num--)
			{
				if (m_Icons[num] != 0)
				{
					m_TopPriority = num;
					break;
				}
			}
			m_VisibleCached = m_VisibleUser && TopIcon != 0 && (m_VisibilityPredicate == null || m_VisibilityPredicate());
		}
	}
}
