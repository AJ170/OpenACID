using System.Collections;
using UnityEngine;

public class bloodDecals : MonoBehaviour
{
	public float displayTime;

	public float fadeOutSpeed;

	private void OnEnable()
	{
		StartCoroutine("delay");
	}

	private IEnumerator delay()
	{
		yield return new WaitForSeconds(displayTime);
		StartCoroutine("fadeOut");
	}

	private IEnumerator fadeOut()
	{
		for (float f = 1f; f >= 0f; f -= fadeOutSpeed * Time.deltaTime)
		{
			Color c = GetComponent<Renderer>().material.color;
			c.a = f;
			GetComponent<Renderer>().material.color = c;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		base.gameObject.SetActive(false);
	}
}
