using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public interface IPositionProvider
	{
		Vector3 Position { get; set; }

		Vector3 Scale { get; set; }

		Quaternion Rotation { get; set; }
	}
}
