namespace Assets.Scripts.Audio
{
	public interface IAudioPlayBehaviour : IAudioPlayBehaviourFuture
	{
		SRawSoundLoadDef Current { get; }

		EPlayBehaviour EPlayBehaviour { get; }

		IConstFuture Take { get; }

		IConstFuture GiveBack { set; }
	}
}
