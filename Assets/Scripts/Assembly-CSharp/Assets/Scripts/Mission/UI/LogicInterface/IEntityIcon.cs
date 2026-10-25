using System;
using Assets.Scripts.Mission.Logic;
using UnityEngine;

namespace Assets.Scripts.Mission.UI.LogicInterface
{
	public interface IEntityIcon
	{
		EntityId EntityId { get; }

		IconMaterial TopIcon { get; }

		bool IsHiddenOnBorders { get; set; }

		float FadingTime { get; set; }

		bool Visible { get; set; }

		Func<bool> VisibilityPredicate { get; set; }

		Vector3 Offset2D { get; set; }

		Vector3 Offset3D { get; set; }

		bool IsInteractable { get; }

		bool IsOnBorder { get; }

		float IconSize { get; }

		void SetIcon(IconMaterial _icon);

		void SetIcon(IconMaterial _icon, int _priority);

		void SetInteractable(bool _interactable, Func<IEntityIcon, bool> _action);
	}
}
