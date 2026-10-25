using System;
using UnityEngine;
using UnityEngine.AI;

namespace UbiCore.Messenger
{
	[Serializable]
	public class MoveToEvent : Message
	{
		public enum EInputType
		{
			Hotspot = 0,
			GroundTap = 1,
			GroundDrag = 2
		}

		public NavMeshPath Path { get; private set; }

		public EInputType Type { get; private set; }

		public float SpeedFactor { get; private set; }

		public float DeltaPercentage { get; private set; }

		public OffMeshLink Link { get; private set; }

		public EDispatcherType Dispatcher { get; private set; }

		public bool EnableFacing { get; private set; }

		public Vector3 FacingDirection { get; private set; }

		public bool HideDecals { get; private set; }

		public MoveToEvent(NavMeshPath _path, EInputType _type, float _speedFactor = -1f, OffMeshLink _link = null, EDispatcherType _dispatcher = EDispatcherType.Unknown, bool _hideDecals = false)
		{
			Path = _path;
			Type = _type;
			SpeedFactor = _speedFactor;
			Link = _link;
			Dispatcher = _dispatcher;
			EnableFacing = false;
			FacingDirection = Vector3.zero;
			HideDecals = _hideDecals;
		}

		public MoveToEvent(NavMeshPath _path, Vector3 _facingDirection, EInputType _type, float _speedFactor = -1f, OffMeshLink _link = null, EDispatcherType _dispatcher = EDispatcherType.Unknown, bool _hideDecals = false)
		{
			Path = _path;
			Type = _type;
			SpeedFactor = _speedFactor;
			Link = _link;
			Dispatcher = _dispatcher;
			EnableFacing = true;
			FacingDirection = _facingDirection;
			HideDecals = _hideDecals;
		}
	}
}
