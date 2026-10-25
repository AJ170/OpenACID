using System;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Ingame;
using UnityEngine;
using UnityEngine.AI;

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

	void IEntityIcon.SetIcon(IconMaterial _icon)
	{
		((IconAttachment)this).SetIcon(_icon);
	}

}
