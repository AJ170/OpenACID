using UbiCore.Messenger;

namespace Assets.Scripts.Mission.UI
{
	public class CombatKillTokenUiEvent : ILogicUiEvent, IMessage
	{
		public enum EType
		{
			Gain = 0,
			Lost = 1,
			Spend = 2,
			EnterKillTokenMode = 3,
			ExitKillTokenMode = 4
		}

		public EType Type { get; private set; }

		public int Value { get; private set; }

		public CombatKillTokenUiEvent(EType _type, int _valueNew)
		{
			Type = _type;
			Value = _valueNew;
		}
	}
}
