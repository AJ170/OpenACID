using AcierProtocol;
using Assets.Scripts.Audio;
using Database;
using UbiCore.Utils;
using UnityEngine;

public class MysteryBoxEffects : MonoBehaviour
{
	public GameObject MundaneEffect;

	public GameObject CommonEffect;

	public GameObject UncommonEffect;

	public GameObject Rare;

	public GameObject Perfect;

	private bool WasTriggered;

	private bool ParticlesWereEmitted;

	public void PlayEffect(Quality.EQuality _Quality)
	{
		WasTriggered = true;
		MundaneEffect.SetActive(false);
		CommonEffect.SetActive(false);
		UncommonEffect.SetActive(false);
		Rare.SetActive(false);
		Perfect.SetActive(false);
		Color white = Color.white;
		switch (_Quality)
		{
		case Quality.EQuality.Mundane:
			MundaneEffect.SetActive(true);
			MundaneEffect.GetComponent<Renderer>().material.SetColor("_TintColor", white);
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_mysterybox_reward_blue);
			break;
		case Quality.EQuality.Common:
			white = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_commonColor).Color0;
			MundaneEffect.SetActive(true);
			CommonEffect.SetActive(true);
			MundaneEffect.GetComponent<Renderer>().material.SetColor("_TintColor", white);
			CommonEffect.GetComponent<ParticleSystem>().startColor = white;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_mysterybox_reward_grey);
			break;
		case Quality.EQuality.Uncommon:
			white = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_uncommonColor).Color0;
			MundaneEffect.GetComponent<Renderer>().material.SetColor("_TintColor", white);
			MundaneEffect.SetActive(true);
			CommonEffect.SetActive(true);
			UncommonEffect.SetActive(true);
			MundaneEffect.GetComponent<Renderer>().material.SetColor("_TintColor", white);
			CommonEffect.GetComponent<ParticleSystem>().startColor = white;
			UncommonEffect.GetComponent<ParticleSystem>().startColor = white;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_mysterybox_reward_green);
			break;
		case Quality.EQuality.Rare:
			white = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_rareColor).Color0;
			MundaneEffect.GetComponent<Renderer>().material.SetColor("_TintColor", white);
			MundaneEffect.SetActive(true);
			CommonEffect.SetActive(true);
			UncommonEffect.SetActive(true);
			Perfect.SetActive(true);
			MundaneEffect.GetComponent<Renderer>().material.SetColor("_TintColor", white);
			CommonEffect.GetComponent<ParticleSystem>().startColor = white;
			UncommonEffect.GetComponent<ParticleSystem>().startColor = white;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_mysterybox_reward_purple);
			break;
		case Quality.EQuality.Perfect:
			white = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_perfectColor).Color0;
			MundaneEffect.GetComponent<Renderer>().material.SetColor("_TintColor", white);
			MundaneEffect.SetActive(true);
			CommonEffect.SetActive(true);
			UncommonEffect.SetActive(true);
			Rare.SetActive(true);
			Perfect.SetActive(true);
			MundaneEffect.GetComponent<Renderer>().material.SetColor("_TintColor", white);
			CommonEffect.GetComponent<ParticleSystem>().startColor = white;
			UncommonEffect.GetComponent<ParticleSystem>().startColor = white;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_mysterybox_reward_orange);
			break;
		}
	}

	private void Update()
	{
		if (WasTriggered && !ParticlesWereEmitted && (!CommonEffect.GetComponent<ParticleSystem>().IsAlive() || !UncommonEffect.GetComponent<ParticleSystem>().IsAlive() || !Rare.GetComponent<ParticleSystem>().IsAlive() || !Perfect.GetComponent<ParticleSystem>().IsAlive()))
		{
			ParticlesWereEmitted = true;
		}
		if (WasTriggered && ParticlesWereEmitted && !CommonEffect.GetComponent<ParticleSystem>().IsAlive() && !UncommonEffect.GetComponent<ParticleSystem>().IsAlive() && !Rare.GetComponent<ParticleSystem>().IsAlive() && !Perfect.GetComponent<ParticleSystem>().IsAlive())
		{
			MundaneEffect.SetActive(false);
			CommonEffect.SetActive(false);
			UncommonEffect.SetActive(false);
			Rare.SetActive(false);
			Perfect.SetActive(false);
		}
	}
}
