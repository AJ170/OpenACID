using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public interface ITransitionable
	{
		Vector3 Position { get; }

		bool IsDone { get; }

		void UpdateTransition(SmoothPositionTransition _transition);
	}
}
