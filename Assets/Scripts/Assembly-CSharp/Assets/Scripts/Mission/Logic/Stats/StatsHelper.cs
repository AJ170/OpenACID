namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class StatsHelper<T> : StatsHelperBase where T : StatsHelper<T>, new()
	{
		private static readonly T m_instance = new T();

		public static T Instance
		{
			get
			{
				return m_instance;
			}
		}

		protected StatsHelper(StatTypes.StatsRange _immutableRange, StatTypes.StatsRange _mutableRange, StatTypes.StatsRange _computedRange)
			: base(_immutableRange, _mutableRange, _computedRange)
		{
		}
	}
}
