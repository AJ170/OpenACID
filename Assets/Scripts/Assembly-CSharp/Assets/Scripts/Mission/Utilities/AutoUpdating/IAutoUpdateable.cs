namespace Assets.Scripts.Mission.Utilities.AutoUpdating
{
	public interface IAutoUpdateable
	{
		bool IsRunning { get; }

		void Start();

		void Stop();

		bool UpdateFrame(float _gameTimeDelta);
	}
}
