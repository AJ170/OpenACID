namespace Assets.Scripts.Audio
{
	public interface IConstFuture
	{
		int Index { get; }

		SRawSoundLoadDef SoundLoadDef { get; }
	}
}
