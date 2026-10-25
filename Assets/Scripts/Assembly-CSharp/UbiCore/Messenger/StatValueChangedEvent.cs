using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Stats;

namespace UbiCore.Messenger
{
	public class StatValueChangedEvent : Message
	{
		public IEntity Owner { get; private set; }

		public IReadonlyStatValue StatValue { get; private set; }

		public IStatValueWrapper PreviousValueWrapper { get; private set; }

		public StatValueChangedEvent(IEntity _owner, IReadonlyStatValue _statValue, IStatValueWrapper _prevValueWrapper)
		{
			Owner = _owner;
			StatValue = _statValue;
			PreviousValueWrapper = _prevValueWrapper;
		}
	}
}
