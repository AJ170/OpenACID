using System.Collections;
using UnityEngine;

public class AnimateAchievementHighlight : MonoBehaviour
{
	public float AnimationSpeed;

	public Vector2 StartOffset;

	public Vector2 TargetOffset;

	public float startDelay;

	public bool startOnEnable = true;

	private float lastTime;

	private ParticleSystem particle;

	private void OnEnable()
	{
		if (startOnEnable)
		{
			StartCoroutine(AnimateHighlight(new Color(1f, 1f, 1f, 1f)));
		}
	}

	private void Start()
	{
		lastTime = Time.realtimeSinceStartup;
	}

	public IEnumerator AnimateHighlight(Color _colorTint)
	{
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.SetColor("_TintColor", _colorTint);
		renderer.material.SetTextureOffset("_MainTex", StartOffset);
		lastTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < lastTime + startDelay)
		{
			yield return null;
		}
		while (true)
		{
			float deltaTime = Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			if (deltaTime == 0f)
			{
				yield return 0;
			}
			if ((bool)renderer)
			{
				Vector2 newOffset = Vector2.Lerp(renderer.material.GetTextureOffset("_MainTex"), TargetOffset, AnimationSpeed * deltaTime);
				renderer.material.SetTextureOffset("_MainTex", newOffset);
				if (Vector2.Distance(newOffset, TargetOffset) <= 0.05f)
				{
					renderer.material.SetTextureOffset("_MainTex", TargetOffset);
					base.gameObject.SetActive(false);
					break;
				}
				yield return 0;
				continue;
			}
			break;
		}
	}
}
