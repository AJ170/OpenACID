namespace Assets.Scripts.Audio
{
	public class Future : IConstFuture
	{
		public int Index { get; set; }

		public SRawSoundLoadDef SoundLoadDef { get; set; }

		public Future()
		{
			Index = 0;
			SoundLoadDef = SRawSoundLoadDef.Invalid;
		}
	}
}
