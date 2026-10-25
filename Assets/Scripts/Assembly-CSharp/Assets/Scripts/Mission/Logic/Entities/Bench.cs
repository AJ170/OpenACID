using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class Bench : HideoutObject
	{
		public enum ESeats
		{
			Player = 0,
			LeftNPC = 1,
			RightNPC = 2,
			Count = 3
		}

		public class Seat
		{
			public Bench Bench { get; set; }

			public Vector3 Position { get; set; }

			public Quaternion Orientation { get; set; }

			public Vector3 RandomOffset { get; set; }

			public EntityId EntityId { get; set; }

			public Seat(Bench _bench, Vector3 _position, Quaternion _orientation, Vector3 _randomOffset)
			{
				Bench = _bench;
				Position = _position;
				Orientation = _orientation;
				RandomOffset = _randomOffset;
				EntityId = EntityId.InvalidId;
			}
		}

		private Seat[] m_seats = new Seat[3];

		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.Bench;
			}
		}

		public int AssassinationStartTurn { get; set; }

		public Bench()
		{
			AssassinationStartTurn = 0;
		}

		public Seat GetSeat(ESeats _seat)
		{
			return m_seats[(int)_seat];
		}

		public void SetSeat(ESeats _seat, Vector3 _position, Quaternion _orientation)
		{
			m_seats[(int)_seat] = new Seat(this, _position, _orientation, GetRandomOffset(_orientation));
		}

		public override bool CanInteract()
		{
			bool flag = base.CanInteract();
			if (flag)
			{
				flag = AssassinationStartTurn == 0 || (float)AssassinationStartTurn + 100f < (float)LogicMain.CurrentTurn;
			}
			return flag;
		}

		private static Vector3 GetRandomOffset(Quaternion _orientation)
		{
			Vector3 right = Vector3.right;
			right.x *= LogicMain.Random.Rand(-0.15f, 0.15f);
			return _orientation * right;
		}
	}
}
