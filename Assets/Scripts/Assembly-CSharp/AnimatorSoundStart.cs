using Assets.Scripts.Audio;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Database;
using UbiCore.Utils;
using UnityEngine;

[RequireComponent(typeof(AnimatorDataController))]
public class AnimatorSoundStart : MonoBehaviour, IAnimatorEventListener
{
	private AnimatorDataController m_animatorDataController;

	private AnimatorDataController AnimatorDataController
	{
		get
		{
			if (m_animatorDataController == null)
			{
				m_animatorDataController = GetComponent<AnimatorDataController>();
			}
			return m_animatorDataController;
		}
	}

	public void Attach()
	{
		AnimatorDataController.RegisterEventListener(this);
	}

	public void Detach()
	{
		AnimatorDataController.UnregisterEventListener(this);
	}

	private void OnDestroy()
	{
		Detach();
	}

	public bool OnAnimatorEvent(AnimatorEvent _e)
	{
		switch (_e.Type)
		{
		case AnimatorEventType.SoundStart:
			if (_e.Payload != null)
			{
				AudioControllerAsset audioControllerAsset = (AudioControllerAsset)_e.Payload;
				if (GameID.IsValid(audioControllerAsset.GameId))
				{
					IAudioHandle audioHandle2 = AudioManager.Instance.CreateHandle(audioControllerAsset.GameId, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
					audioHandle2.Load();
					audioHandle2.Play(false, GetComponent<AudioSource>());
				}
			}
			break;
		case AnimatorEventType.SoundVoiceAttack:
		{
			EntityWrapperComponent component = GetComponent<EntityWrapperComponent>();
			if (component != null)
			{
				bool flag = component.Entity.Type == EntityTypes.EType.PlayerCharacter;
				IAudioHandle audioHandle = AudioManager.Instance.CreateHandle((!flag) ? UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_enemy_attack : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_player_attack, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				audioHandle.Load();
				audioHandle.Play(false, GetComponent<AudioSource>());
			}
			break;
		}
		}
		return false;
	}
}
