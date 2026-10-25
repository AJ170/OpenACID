using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public class SimplePositionProvider : IPositionProvider
	{
		public Vector3 Position { get; set; }

		public Vector3 Scale
		{
			get
			{
				return Vector3.one;
			}
			set
			{
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
			set
			{
			}
		}

		public SimplePositionProvider(Vector3 _position)
		{
			Position = _position;
		}
	}
}
