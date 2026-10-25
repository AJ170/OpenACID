namespace Assets.Scripts.Audio
{
	public interface IStoppedFinishedStrategy
	{
		EStoppedFinishedStrategy EStoppedFinishedStrategy { get; }

		void OnStoppedFinished(SRawSoundLoadDef _soundLoadDef);
	}
}
