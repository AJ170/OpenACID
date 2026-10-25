using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ITransformProxy
	{
		Vector3 Position { get; set; }

		Quaternion Rotation { get; set; }
	}
}
