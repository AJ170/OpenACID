using Assets.Scripts.Audio;
using UbiCore.Utils;

public class BonFireInstantiateManager : FindAndInstantiateManager<BonfireFindClass, PlayAudioTrigger>
{
	protected override void SetupInstance(PlayAudioTrigger _instance)
	{
		_instance.PlayAudio.Loop = true;
		_instance.PlayAudio.ReloadAudioHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_env_fire_01_lp);
	}
}
