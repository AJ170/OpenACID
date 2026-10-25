using System.Collections;
using UnityEngine;

public class NotificationPopUpCompleteEffect : MonoBehaviour
{
	public GameObject Effects;

	public GameObject Swipe;

	public GameObject Swipe2;

	public void StartEffects()
	{
		Debug.Log("Effect Start!");
		StartCoroutine(PlayEffects());
	}

	private IEnumerator PlayEffects()
	{
		Swipe.SetActive(true);
		Swipe2.SetActive(true);
		Effects.SetActive(true);
		yield return new WaitForSeconds(2f);
		Effects.SetActive(false);
	}
}
