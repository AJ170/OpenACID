namespace Assets.Scripts.Audio
{
	public interface IAudioPlayBehaviourFuture
	{
		IConstFuture Future { get; }

		void Unload(SRawSoundLoadDef _def);
	}
}
