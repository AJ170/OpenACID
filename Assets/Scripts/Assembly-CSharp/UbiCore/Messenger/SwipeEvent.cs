using System;
using UnityEngine;

namespace UbiCore.Messenger
{
	public class SwipeEvent : InputEvent
	{
		[Flags]
		public enum eDirection
		{
			Up = 1,
			Down = 2,
			Left = 4,
			Right = 8,
			UpLeftDiagonal = 0x10,
			UpRightDiagonal = 0x20,
			DownLeftDiagonal = 0x40,
			DownRightDiagonal = 0x80,
			None = 0,
			Vertical = 3,
			Horizontal = 0xC,
			Cross = 0xF,
			UpDiagonals = 0x30,
			DownDiagonals = 0xC0,
			Diagonals = 0xF0,
			All = 0xFF
		}

		public eDirection Direction { get; private set; }

		public float Speed { get; private set; }

		public Vector2 Distance { get; private set; }

		public SwipeEvent(eDirection _direction, float _speed, Vector2 _distance, int _fingerCount)
			: base(_fingerCount)
		{
			Direction = _direction;
			Speed = _speed;
			Distance = _distance;
		}
	}
}
