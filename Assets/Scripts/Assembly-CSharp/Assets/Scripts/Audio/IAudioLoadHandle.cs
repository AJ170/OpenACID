using Database;

namespace Assets.Scripts.Audio
{
	public interface IAudioLoadHandle
	{
		GameID AudioControllerId { get; }

		void Load();

		void Unload();
	}
}
