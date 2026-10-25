using AcierProtocol;
using Database;
using UbiCore.Utils;
using UnityEngine;

public class ChestLight : MonoBehaviour
{
	public GameObject[] EffectLayers;

	public Material Materials;

	public float Speeds;

	private float Progress;

	public ParticleSystem BurstEffect;

	public AnimationCurve Fade;

	public Color EffectColor;

	private float StartTime;

	private bool Animate;

	public void SetEffectColor(Quality.EQuality _Quality)
	{
		switch (_Quality)
		{
		case Quality.EQuality.Perfect:
			EffectColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_perfectColor).Color0;
			EffectLayers[4].SetActive(true);
			EffectLayers[3].SetActive(true);
			break;
		case Quality.EQuality.Rare:
			EffectColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_rareColor).Color0;
			EffectLayers[4].SetActive(false);
			EffectLayers[3].SetActive(true);
			break;
		case Quality.EQuality.Uncommon:
			EffectColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_uncommonColor).Color0;
			EffectLayers[4].SetActive(false);
			EffectLayers[3].SetActive(false);
			break;
		case Quality.EQuality.Common:
			EffectColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_commonColor).Color0;
			EffectLayers[4].SetActive(false);
			EffectLayers[3].SetActive(false);
			break;
		case Quality.EQuality.Mundane:
			EffectColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_mundaneColor).Color0;
			EffectLayers[4].SetActive(false);
			EffectLayers[3].SetActive(false);
			break;
		}
	}

	public void PlayCoins()
	{
		Animate = true;
		StartTime = Time.time;
		BurstEffect.Play();
		EffectColor = new Color(0.964f, 0.901f, 0.432f, 0.584f);
	}

	public void Play()
	{
		Animate = true;
		StartTime = Time.time;
		BurstEffect.Play();
		BurstEffect.gameObject.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.SetColor("_TintColor", EffectColor);
	}

	private void Update()
	{
		if (Animate)
		{
			if (Time.time - StartTime > (float)Fade.length)
			{
				Animate = false;
			}
			float num = Fade.Evaluate(Time.time - StartTime);
			Materials.SetColor("_TintColor", EffectColor * num);
			Progress += Speeds * Time.deltaTime;
			Materials.SetTextureOffset("_MainTex", new Vector2(Progress, 0f));
		}
	}
}
