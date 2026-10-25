using System;
using UnityEngine;

namespace UbiCore.Messenger
{
	[Serializable]
	public class JumpAnywhereEvent : ParcourEvent
	{
		public Vector3 LandPosition { get; private set; }

		public JumpAnywhereEvent(Vector3 _landPosition)
		{
			LandPosition = _landPosition;
			base.Type = EParcourType.JumpAnywhere;
		}
	}
}
