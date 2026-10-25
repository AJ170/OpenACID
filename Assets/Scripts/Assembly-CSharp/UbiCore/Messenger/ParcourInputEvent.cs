using System;
using UnityEngine;

namespace UbiCore.Messenger
{
	[Serializable]
	public class ParcourInputEvent : ParcourUpdateEvent
	{
		public Vector3 Direction { get; private set; }

		public ParcourInputEvent(Vector3 _direction)
		{
			base.UpdateType = EUpdateType.Input;
			Direction = _direction;
		}
	}
}
