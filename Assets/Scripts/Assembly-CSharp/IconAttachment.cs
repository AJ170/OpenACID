using System;
using UnityEngine;

public abstract class IconAttachment : IDisposable
{
	private Func<NavMeshAgent, IconAttachment, bool> m_interactAction;

	protected IconDisplay m_display;

	protected Animator m_animator;

	protected IIconPositionCarrier m_IconCarrier;

	public abstract GameObject GameObject { get; }

	public Vector3 Offset2D { get; set; }

	public Vector3 Offset3D { get; set; }

	public float InteractionFeedbackScale { get; set; }

	public bool IsInteractable { get; private set; }

	public bool HasInteractAction
	{
		get
		{
			return m_interactAction != null;
		}
	}

	public bool IsOnBorder
	{
		get
		{
			return m_display != null && m_display.IsOnBorder;
		}
	}

	public float IconSize
	{
		get
		{
			return (!(m_display == null)) ? m_display.IconSize : 0f;
		}
	}

	public virtual Vector3 Position
	{
		get
		{
			Vector3 vector = Vector3.zero;
			if (m_IconCarrier == null)
			{
				if (m_animator != null)
				{
					Transform boneTransform = m_animator.GetBoneTransform(HumanBodyBones.Head);
					vector = boneTransform.position;
				}
			}
			else
			{
				vector = m_IconCarrier.IconPosition;
			}
			return vector + Offset3D;
		}
	}

	protected IconAttachment()
	{
		Offset2D = new Vector3(0f, 0f, 0f);
		Offset3D = new Vector3(0f, 0.3f, 0f);
		InteractionFeedbackScale = 0.7f;
	}

	public void Dispose()
	{
		Release();
		SetInteractable(false, null);
	}

	public virtual void Release()
	{
		if (m_display != null)
		{
			m_display.Release();
		}
		m_display = null;
	}

	public void SetIcon(IconMaterial _icon)
	{
		if (_icon == IconMaterial.Invalid)
		{
			if (m_display != null)
			{
				m_display.Release();
			}
			m_display = null;
		}
		else
		{
			if (m_display == null)
			{
				m_display = IconDisplay.Get(this);
			}
			m_display.SetIcon(_icon);
		}
	}

	public void SetInteractableKeepAction(bool _interactable)
	{
		if (m_interactAction == null)
		{
			_interactable = false;
		}
		IsInteractable = _interactable;
		if (m_display != null)
		{
			m_display.UpdateInteractableLayer();
		}
	}

	public void SetInteractable(bool _interactable, Func<NavMeshAgent, IconAttachment, bool> _action)
	{
		IsInteractable = _interactable;
		if (m_display != null)
		{
			m_display.UpdateInteractableLayer();
		}
		if (IsInteractable)
		{
			m_interactAction = _action;
		}
		else
		{
			m_interactAction = null;
		}
	}

	public IconMaterial GetIcon()
	{
		if (m_display != null)
		{
			return m_display.GetIcon();
		}
		return IconMaterial.Invalid;
	}

	public virtual bool OnTap(NavMeshAgent _agent)
	{
		if (IsInteractable && m_interactAction != null)
		{
			if (m_display != null)
			{
				m_display.AdditionalScale = InteractionFeedbackScale;
			}
			return m_interactAction(_agent, this);
		}
		return false;
	}

	public void SetProperties(float _alpha)
	{
		if (m_display != null)
		{
			m_display.Alpha = _alpha;
		}
	}
}
