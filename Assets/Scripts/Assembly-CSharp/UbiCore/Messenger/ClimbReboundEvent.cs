using System;
using UnityEngine;

namespace UbiCore.Messenger
{
	[Serializable]
	public class ClimbReboundEvent : ParcourEvent
	{
		public Vector3 WallPoint { get; private set; }

		public Vector3 WallNormal { get; private set; }

		public ClimbReboundEvent(Vector3 _wallPoint, Vector3 _wallNormal)
		{
			base.Type = EParcourType.ClimbRebound;
			WallPoint = _wallPoint;
			WallNormal = _wallNormal;
		}
	}
}
