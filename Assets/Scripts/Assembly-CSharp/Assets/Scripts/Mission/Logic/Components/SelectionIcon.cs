using Assets.Scripts.Engine;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class SelectionIcon : Component
	{
		private IconMaterial m_iconMaterial;

		private bool m_selected;

		public sealed override int Priority
		{
			get
			{
				return 13;
			}
		}

		public IconMaterial SelectionIconMaterial
		{
			get
			{
				return m_iconMaterial;
			}
			set
			{
				m_iconMaterial = value;
				SetIcon();
			}
		}

		public bool Selected
		{
			get
			{
				return m_selected;
			}
			set
			{
				m_selected = value;
				SetIcon();
			}
		}

		public GameObject EntityRoot
		{
			get
			{
				return GameObject;
			}
		}

		public IHeadIconCarrier LogicEntity
		{
			get
			{
				return GameEntity.Cast<IHeadIconCarrier>();
			}
		}

		private EntityIcon Icon
		{
			get
			{
				return GameEntity.Cast<IHeadIconCarrier>().HeadIcon.IconAttachment;
			}
		}

		public SelectionIcon(IEntity _entity)
			: base(_entity)
		{
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			GameEntity.Cast<IHeadIconCarrier>().HeadIcon.IconAttachment = new EntityIcon();
			GameEntity.Cast<IHeadIconCarrier>().HeadIcon.IconAttachment.IsHiddenOnBorders = false;
			Selected = false;
		}

		private void SetIcon()
		{
			if (Icon != null)
			{
				Icon.SetIcon(m_selected ? SelectionIconMaterial : IconMaterial.Invalid, 6);
			}
		}
	}
}
