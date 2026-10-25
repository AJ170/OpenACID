using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using UbiCore.Messenger;

namespace Assets.Scripts.Mission.UI
{
	public class BuffUiEvent : LogicUiEvent
	{
		public enum EState
		{
			Active = 0,
			Inactive = 1
		}

		public EState State { get; private set; }

		public EBuffType Type { get; private set; }

		public EntityId Target { get; private set; }

		public int Id { get; private set; }

		public int StartTime { get; private set; }

		public int EndTime { get; private set; }

		public int Duration { get; private set; }

		public BuffUiEvent(BuffEvent _source)
		{
			IBuff buff = _source.Buff;
			State = ((!buff.IsRunning) ? EState.Inactive : EState.Active);
			Type = buff.Type;
			Target = _source.Entity;
			Id = buff.GetHashCode();
			StartTime = buff.StartTurn;
			if (buff.DurationType == EBuffDurationType.Finite)
			{
				EndTime = buff.StartTurn + buff.TurnDuration;
			}
			else
			{
				EndTime = int.MaxValue;
			}
			Duration = buff.TurnDuration;
		}
	}
}
