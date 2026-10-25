using System;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Ingame;
using UnityEngine;

public class EntityIcon : SmartIcon, IEntityIcon
{
	private HeadIcon m_headIcon;

	private bool m_shouldBeUpdated = true;

	private bool m_attached;

	public HeadIcon HeadIcon
	{
		get
		{
			return m_headIcon;
		}
		set
		{
			if (m_headIcon != value)
			{
				Release();
				m_headIcon = value;
				if (m_headIcon != null)
				{
					Attach();
				}
				ShouldBeUpdated = true;
			}
		}
	}

	public override Vector3 Position
	{
		get
		{
			return (!(m_animator == null) || m_IconCarrier != null) ? base.Position : m_headIcon.GameEntity.Position;
		}
	}

	public override bool ShouldBeUpdated
	{
		get
		{
			return m_shouldBeUpdated;
		}
		protected set
		{
			m_shouldBeUpdated = value;
			if (m_shouldBeUpdated && m_headIcon != null)
			{
				m_headIcon.ScheduleFrameUpdate(false);
			}
		}
	}

	public override GameObject GameObject
	{
		get
		{
			return (m_headIcon != null) ? m_headIcon.GameEntity.GameObject : null;
		}
	}

	public EntityId EntityId
	{
		get
		{
			return (m_headIcon != null) ? m_headIcon.GameEntity.Id : EntityId.InvalidId;
		}
	}

	public void Attach()
	{
		m_IconCarrier = m_headIcon.GameEntity.FindComponent<IIconPositionCarrier>();
		if (m_IconCarrier == null && GameObject != null)
		{
			m_animator = GameObject.GetComponent<Animator>();
		}
		m_attached = true;
		ShouldBeUpdated = true;
	}

	public override void Release()
	{
		base.Release();
		m_IconCarrier = null;
		Detach();
	}

	public void Detach()
	{
		m_animator = null;
		m_attached = false;
		ShouldBeUpdated = true;
	}

	public void SetInteractable(bool _interactable, Func<IEntityIcon, bool> _action)
	{
		SetInteractable(_interactable, (_action != null) ? ((Func<NavMeshAgent, IconAttachment, bool>)((NavMeshAgent agent, IconAttachment attachment) => _action((IEntityIcon)attachment))) : null);
	}

	protected override void UpdateVisibilityInternal()
	{
		base.UpdateVisibilityInternal();
		m_VisibleCached &= m_attached;
	}

	virtual void IEntityIcon.SetIcon(IconMaterial _icon)
	{
		((IconAttachment)this).SetIcon(_icon);
	}

	virtual void IEntityIcon.SetIcon(IconMaterial _icon, int _priority)
	{
		SetIcon(_icon, _priority);
	}

	virtual IconMaterial IEntityIcon.get_TopIcon()
	{
		return base.TopIcon;
	}

	virtual float IEntityIcon.get_FadingTime()
	{
		return base.FadingTime;
	}

	virtual void IEntityIcon.set_FadingTime(float value)
	{
		base.FadingTime = value;
	}

	virtual bool IEntityIcon.get_Visible()
	{
		return base.Visible;
	}

	virtual void IEntityIcon.set_Visible(bool value)
	{
		base.Visible = value;
	}

	virtual Func<bool> IEntityIcon.get_VisibilityPredicate()
	{
		return base.VisibilityPredicate;
	}

	virtual void IEntityIcon.set_VisibilityPredicate(Func<bool> value)
	{
		base.VisibilityPredicate = value;
	}

	virtual Vector3 IEntityIcon.get_Offset2D()
	{
		return base.Offset2D;
	}

	virtual void IEntityIcon.set_Offset2D(Vector3 value)
	{
		base.Offset2D = value;
	}

	virtual Vector3 IEntityIcon.get_Offset3D()
	{
		return base.Offset3D;
	}

	virtual void IEntityIcon.set_Offset3D(Vector3 value)
	{
		base.Offset3D = value;
	}

	virtual bool IEntityIcon.get_IsInteractable()
	{
		return base.IsInteractable;
	}

	virtual bool IEntityIcon.get_IsOnBorder()
	{
		return base.IsOnBorder;
	}

	virtual float IEntityIcon.get_IconSize()
	{
		return base.IconSize;
	}
}
