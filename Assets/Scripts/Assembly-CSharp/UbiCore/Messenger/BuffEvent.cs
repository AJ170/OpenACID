using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Stats;

namespace UbiCore.Messenger
{
	public class BuffEvent : Message
	{
		public IBuff Buff { get; private set; }

		public bool Removed { get; private set; }

		public EntityId Entity { get; private set; }

		public BuffEvent(IBuff _buff, EntityId _entity, bool _gotRemoved)
		{
			Buff = _buff;
			Removed = _gotRemoved;
			Entity = _entity;
		}
	}
}
