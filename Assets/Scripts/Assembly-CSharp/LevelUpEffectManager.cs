using UnityEngine;

public class LevelUpEffectManager : MonoBehaviour
{
	public ParticleSystem turbulence;

	public ParticleSystem glow;

	public AnimateAchievementHighlight wave1;

	public AnimateAchievementHighlight wave2;

	public AnimateAchievementHighlight wave3;

	public AnimateAchievementHighlight wave4;

	public AnimateAchievementHighlight wave5;

	public void PlayEffects()
	{
		wave1.gameObject.SetActive(true);
		wave2.gameObject.SetActive(true);
		wave3.gameObject.SetActive(true);
		wave4.gameObject.SetActive(true);
		wave5.gameObject.SetActive(true);
		StartCoroutine(wave1.AnimateHighlight(new Color(1f, 1f, 1f, 1f)));
		StartCoroutine(wave2.AnimateHighlight(new Color(1f, 1f, 1f, 1f)));
		StartCoroutine(wave3.AnimateHighlight(new Color(1f, 1f, 1f, 1f)));
		StartCoroutine(wave4.AnimateHighlight(new Color(1f, 1f, 1f, 1f)));
		StartCoroutine(wave5.AnimateHighlight(new Color(1f, 1f, 1f, 1f)));
	}

	public bool IsPlayingEfect()
	{
		return wave1.gameObject.activeSelf || wave2.gameObject.activeSelf || wave3.gameObject.activeSelf || wave4.gameObject.activeSelf || wave5.gameObject.activeSelf;
	}

	public void StopEffects()
	{
		base.gameObject.SetActive(false);
	}
}
