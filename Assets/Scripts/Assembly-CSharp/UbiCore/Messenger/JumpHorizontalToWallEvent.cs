using System;
using UnityEngine;

namespace UbiCore.Messenger
{
	[Serializable]
	public class JumpHorizontalToWallEvent : ParcourEvent
	{
		public ClimbRoute Route;

		public Vector3 TargetPosition;

		public Vector3 StartPosition;

		public JumpHorizontalToWallEvent(ClimbRoute _route, Vector3 _startPosition, Vector3 _targetPosition)
		{
			Route = _route;
			StartPosition = _startPosition;
			TargetPosition = _targetPosition;
			base.Type = EParcourType.JumpHorizontalToWall;
		}
	}
}
