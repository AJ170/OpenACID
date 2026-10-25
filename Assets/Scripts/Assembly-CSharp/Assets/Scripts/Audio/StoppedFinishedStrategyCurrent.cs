namespace Assets.Scripts.Audio
{
	public class StoppedFinishedStrategyCurrent : StoppedFinishedStrategyBase
	{
		private readonly ILoadStrategy m_loadStrategy;

		public override EStoppedFinishedStrategy EStoppedFinishedStrategy
		{
			get
			{
				return EStoppedFinishedStrategy.UnloadCurrent;
			}
		}

		public StoppedFinishedStrategyCurrent(ILoadStrategy _loadStrategy)
		{
			m_loadStrategy = _loadStrategy;
		}

		public override void OnStoppedFinished(SRawSoundLoadDef _soundLoadDef)
		{
			m_loadStrategy.Unload();
		}
	}
}
