public class GlobalTimeScale : ITimeScaleProvider
{
	public float TimeScale { get; set; }

	public GlobalTimeScale(float _timeScale = 1f)
	{
		TimeScale = _timeScale;
	}

	public float Evaluate()
	{
		return TimeScale;
	}
}
