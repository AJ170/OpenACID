namespace Assets.Scripts.Audio
{
	public interface IVolumeFader
	{
		bool IsRunning { get; }

		void Start(float _volumeTo, float _time);

		void Stop();
	}
}
