namespace Assets.Scripts.Mission.AI.Combat
{
	public enum EActionType
	{
		Flee = 0,
		QueueAction = 1,
		CancelAction = 2,
		EnableDodge = 3,
		ConsumeAllyWasAttacked = 4,
		ConsumeAllyWasComboKilled = 5,
		ConsumeQueuedAllyDied = 6,
		ConsumeQueuedAllyFled = 7
	}
}
