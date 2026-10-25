using Assets.Scripts.Audio;
using Database;
using UnityEngine;

public class EffectAudioMixState : EffectBase
{
	[SerializeField]
	private string m_mixStateGameId;

	private AudioManager.MixGroupCarrier m_mixState;

	private void Awake()
	{
		if (!string.IsNullOrEmpty(m_mixStateGameId))
		{
			GameID gameID = AudioManager.GameDb.FindGameIdFromGuid(m_mixStateGameId);
			if (GameID.IsValid(gameID))
			{
				m_mixState = new AudioManager.MixGroupCarrier(gameID);
			}
		}
	}

	public override void OnDestroy()
	{
		if (m_mixState != null)
		{
			AudioManager.Instance.MixStateManager.Stop(m_mixState);
		}
		base.OnDestroy();
	}

	public override void On()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		AudioManager.Instance.MixStateManager.Start(m_mixState);
	}

	public override void Off()
	{
		AudioManager.Instance.MixStateManager.Stop(m_mixState);
		DeregisterAndDestroy();
	}

	public override void Trigger()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
	}
}
