using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace UbiCore.Messenger
{
	public class SkillEvent : Message
	{
		public EntityId EntityId { get; private set; }

		public ESkillEventType SubType { get; private set; }

		public Enum<SkillTypes> SkillType { get; private set; }

		public int TurnDuration { get; private set; }

		public NpcData.NpcType TargetNpcType { get; private set; }

		public ESkillAvailabilityFlags AvailabilityFlags { get; set; }

		private SkillEvent(EntityId _entityId, ESkillEventType _subType, Enum<SkillTypes> _skillType, int _turnDuration, NpcData.NpcType _targetNpcType, ESkillAvailabilityFlags _availabilityFlags)
		{
			EntityId = _entityId;
			SubType = _subType;
			SkillType = _skillType;
			TurnDuration = _turnDuration;
			TargetNpcType = _targetNpcType;
		}

		public SkillEvent(EntityId _entityId, ESkillEventType _subType, Enum<SkillTypes> _skillType, NpcData.NpcType _targetNpcType)
			: this(_entityId, _subType, _skillType, 0, _targetNpcType, ESkillAvailabilityFlags.Unknown)
		{
		}

		public SkillEvent(EntityId _entityId, ESkillEventType _subType, Enum<SkillTypes> _skillType, int _turnDuration)
			: this(_entityId, _subType, _skillType, _turnDuration, null, ESkillAvailabilityFlags.Unknown)
		{
		}

		public SkillEvent(EntityId _entityId, ESkillEventType _subType, Enum<SkillTypes> _skillType, int _turnDuration, ESkillAvailabilityFlags _availabilityFlags)
			: this(_entityId, _subType, _skillType, _turnDuration, null, _availabilityFlags)
		{
		}

		public SkillEvent(EntityId _entityId, ESkillEventType _subType, Enum<SkillTypes> _skillType)
			: this(_entityId, _subType, _skillType, 0, null, ESkillAvailabilityFlags.Unknown)
		{
		}
	}
}
