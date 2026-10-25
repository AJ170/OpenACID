using Assets.Scripts.Mission.Logic.Entities;

namespace UbiCore.Messenger
{
	public sealed class NPCKillEvent : Message
	{
		public NpcCharacter LogicEntity { get; private set; }

		public NPCKillEvent(NpcCharacter _entity)
		{
			LogicEntity = _entity;
		}
	}
}
