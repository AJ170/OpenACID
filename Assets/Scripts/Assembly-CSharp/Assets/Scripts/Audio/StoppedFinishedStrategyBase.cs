namespace Assets.Scripts.Audio
{
	public abstract class StoppedFinishedStrategyBase : IStoppedFinishedStrategy
	{
		public abstract EStoppedFinishedStrategy EStoppedFinishedStrategy { get; }

		public abstract void OnStoppedFinished(SRawSoundLoadDef _soundLoadDef);
	}
}
