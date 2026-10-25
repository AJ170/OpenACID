using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.UI.LogicInterface
{
	public delegate void SkillAvailabilityHandler(Enum<SkillTypes> _skillType, bool _available, ESkillAvailabilityFlags _availabilityFlags);
}
