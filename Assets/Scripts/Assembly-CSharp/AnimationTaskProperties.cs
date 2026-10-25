public class AnimationTaskProperties
{
	public AnimationTaskManager.ELayer Layer { get; set; }

	public AnimationTaskType TaskType
	{
		get
		{
			return ParametersTemplate.AnimationTaskType;
		}
	}

	public AnimationTaskParameters ParametersTemplate { get; set; }
}
