namespace Assets.Scripts.Audio
{
	public class StoppedFinishedStrategyAll : StoppedFinishedStrategyBase
	{
		private readonly ILoadStrategyUnloadAll m_loadStrategy;

		public override EStoppedFinishedStrategy EStoppedFinishedStrategy
		{
			get
			{
				return EStoppedFinishedStrategy.UnloadAll;
			}
		}

		public StoppedFinishedStrategyAll(ILoadStrategyUnloadAll _loadStrategy)
		{
			m_loadStrategy = _loadStrategy;
		}

		public override void OnStoppedFinished(SRawSoundLoadDef _soundLoadDef)
		{
			m_loadStrategy.UnloadAll();
		}
	}
}
