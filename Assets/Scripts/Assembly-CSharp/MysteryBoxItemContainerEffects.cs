using AcierProtocol;
using Database;
using UbiCore.Utils;
using UnityEngine;

public class MysteryBoxItemContainerEffects : MonoBehaviour
{
	public ParticleSystem ExtraItem;

	public ParticleSystem ImpactEffect;

	private void PlayExtraItemEffect()
	{
		ExtraItem.Play();
	}

	private void PlayImpactEffect(Quality.EQuality _Quality)
	{
		switch (_Quality)
		{
		case Quality.EQuality.Mundane:
			ImpactEffect.startColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_mundaneColor).Color0;
			break;
		case Quality.EQuality.Common:
			ImpactEffect.startColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_commonColor).Color0;
			break;
		case Quality.EQuality.Uncommon:
			ImpactEffect.startColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_uncommonColor).Color0;
			break;
		case Quality.EQuality.Rare:
			ImpactEffect.startColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_rareColor).Color0;
			break;
		case Quality.EQuality.Perfect:
			ImpactEffect.startColor = Main.SafeGameDb.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_perfectColor).Color0;
			break;
		}
		ImpactEffect.Play();
	}
}
