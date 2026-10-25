namespace Assets.Scripts.Audio
{
	public class StoppedFinishedStrategyNone : StoppedFinishedStrategyBase
	{
		public override EStoppedFinishedStrategy EStoppedFinishedStrategy
		{
			get
			{
				return EStoppedFinishedStrategy.UnloadNone;
			}
		}

		public override void OnStoppedFinished(SRawSoundLoadDef _soundLoadDef)
		{
		}
	}
}
