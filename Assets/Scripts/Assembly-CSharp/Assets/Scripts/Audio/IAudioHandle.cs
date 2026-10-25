namespace Assets.Scripts.Audio
{
	public interface IAudioHandle : IAudioLoadHandle, IAudioPlayHandle
	{
		PlayResult LastPlayResult { get; }

		void ChangeStoppedFinishedStrategy(EStoppedFinishedStrategy _eStrategy);
	}
}
