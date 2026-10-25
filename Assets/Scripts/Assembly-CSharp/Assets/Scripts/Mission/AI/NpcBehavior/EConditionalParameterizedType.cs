namespace Assets.Scripts.Mission.AI.NpcBehavior
{
	public enum EConditionalParameterizedType
	{
		Common_IsAffectedBySkill = 0,
		Common_IsInInteractionCharacterMainState = 1,
		Common_IsVictimType = 2,
		Common_IsApproachedByAssassin = 3,
		Target_PlayingTask = 4,
		Target_MaxAngleVertical = 5,
		Target_InParameterRange = 6,
		Target_InParameterDistance = 7,
		Target_InParameterMainState = 8
	}
}
