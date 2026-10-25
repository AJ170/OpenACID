namespace Assets.Scripts.Audio
{
	public abstract class ILoadStrategy : ILoadStrategyEvent
	{
		public abstract int LoadedSoundsCount { get; }

		public abstract ELoadStrategy ELoadStrategy { get; }

		public abstract void Load();

		public abstract void Unload();
	}
}
