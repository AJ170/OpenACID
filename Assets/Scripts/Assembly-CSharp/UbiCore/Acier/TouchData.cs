using UnityEngine;

namespace UbiCore.Acier
{
	public class TouchData
	{
		public enum ETouchState
		{
			None = 0,
			Touch = 1,
			Drag = 2,
			Select = 3
		}

		private TouchPhase m_phase;

		private ETouchState m_state;

		public int fingerId;

		public bool isOnUi;

		public Vector2 startPosition;

		public Vector2 stickBasePosition;

		public Vector2 position;

		public Vector2 delta;

		public Vector2 stickInput;

		public float startTime;

		public TouchPhase phase
		{
			get
			{
				return m_phase;
			}
			set
			{
				m_phase = value;
			}
		}

		public ETouchState state
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
			}
		}

		public Vector2 distance
		{
			get
			{
				return position - startPosition;
			}
		}

		public TouchData()
		{
			state = ETouchState.None;
		}
	}
}
