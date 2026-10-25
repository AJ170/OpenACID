using UnityEngine;

namespace Assets.Scripts.Animation
{
	public interface IParkourAnimation : IAnimation
	{
		Vector3 Destination { get; }

		float Height { get; }
	}
}
