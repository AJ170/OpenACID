using System;
using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class HeadIcon : TurnAndFrameUpdateableComponent
	{
		private EntityIcon m_iconAttachment;

		private readonly BBList<EntityIcon> m_customAttachments = new BBList<EntityIcon>();

		private readonly ReadOnlyCollection<EntityIcon> m_customAttachmentsAccess;

		private Func<HeadIcon, bool> m_callback;

		private float m_FrameUpdateIntervalSeconds;

		public bool IsVisible
		{
			get
			{
				return IconAttachment != null && IconAttachment.Visible;
			}
			set
			{
				if (IconAttachment != null)
				{
					IconAttachment.Visible = value;
				}
			}
		}

		public float SparseUpdateIntervalSeconds
		{
			get
			{
				float num = float.MaxValue;
				if (IconAttachment != null)
				{
					num = Math.Min(num, IconAttachment.SparseUpdateIntervalSeconds);
				}
				for (int i = 0; i < m_customAttachments.Count; i++)
				{
					num = Math.Min(num, m_customAttachments[i].SparseUpdateIntervalSeconds);
				}
				return num;
			}
		}

		public sealed override int Priority
		{
			get
			{
				return 12;
			}
		}

		public override float FrameUpdateIntervalSeconds
		{
			get
			{
				return m_FrameUpdateIntervalSeconds;
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return 4;
			}
		}

		public EntityIcon IconAttachment
		{
			get
			{
				return m_iconAttachment;
			}
			set
			{
				if (m_iconAttachment != value)
				{
					if (m_iconAttachment != null)
					{
						m_iconAttachment.HeadIcon = null;
					}
					m_iconAttachment = value;
					if (m_iconAttachment != null)
					{
						m_iconAttachment.HeadIcon = this;
					}
				}
			}
		}

		public ReadOnlyCollection<EntityIcon> CustomIcons
		{
			get
			{
				return m_customAttachmentsAccess;
			}
		}

		public HeadIcon(IEntity _entity)
			: base(_entity)
		{
			m_customAttachmentsAccess = new ReadOnlyCollection<EntityIcon>(m_customAttachments);
			m_FrameUpdateIntervalSeconds = SparseUpdateIntervalSeconds;
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			if (IconAttachment != null)
			{
				IconAttachment.Attach();
			}
			for (int i = 0; i < m_customAttachments.Count; i++)
			{
				m_customAttachments[i].Attach();
			}
			if (IconAttachment != null)
			{
				IconAttachment.SetInteractable(m_callback != null, OnInteractableAction);
			}
		}

		public void SetInteractable(Func<HeadIcon, bool> _callback)
		{
			m_callback = _callback;
			if (IconAttachment != null)
			{
				IconAttachment.SetInteractable(m_callback != null, OnInteractableAction);
			}
		}

		public override void Detach()
		{
			base.Detach();
			if (IconAttachment != null)
			{
				IconAttachment.Detach();
			}
			for (int i = 0; i < m_customAttachments.Count; i++)
			{
				m_customAttachments[i].Detach();
			}
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
			IsVisible = true;
		}

		public override void OnUnregistered()
		{
			if (IconAttachment != null)
			{
				IconAttachment.Release();
			}
			for (int i = 0; i < m_customAttachments.Count; i++)
			{
				m_customAttachments[i].Release();
			}
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			if (m_iconAttachment != null)
			{
				m_iconAttachment.UpdateVisibility();
			}
			for (int i = 0; i < m_customAttachments.Count; i++)
			{
				m_customAttachments[i].UpdateVisibility();
			}
			base.DoTurnUpdate(_deltaTurns);
		}

		public override void DoFrameUpdate(float _deltaTime)
		{
			bool flag = false;
			if (m_iconAttachment != null)
			{
				m_iconAttachment.Update();
				flag = m_iconAttachment.ShouldBeUpdated;
			}
			for (int i = 0; i < m_customAttachments.Count; i++)
			{
				m_customAttachments[i].Update();
				flag |= m_customAttachments[i].ShouldBeUpdated;
			}
			m_FrameUpdateIntervalSeconds = ((!flag) ? SparseUpdateIntervalSeconds : 0f);
			base.DoFrameUpdate(_deltaTime);
		}

		public void AddCustomIconAttachment(EntityIcon _icon)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(!m_customAttachments.Contains(_icon)))
			{
				_icon.HeadIcon = this;
				m_customAttachments.Add(_icon);
			}
		}

		public void RemoveCustomIconAttachment(EntityIcon _icon)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(m_customAttachments.Contains(_icon)))
			{
				_icon.HeadIcon = null;
				m_customAttachments.Remove(_icon);
			}
		}

		private bool OnInteractableAction(NavMeshAgent agent, IconAttachment icon)
		{
			return m_callback(this);
		}
	}
}
