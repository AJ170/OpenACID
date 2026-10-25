using System.Collections;
using UbiCore.Utils;
using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class FPS : MonoBehaviour
{
	[SerializeField]
	private float m_frequency = 0.5f;

	private string m_text;

	private GUIStyle m_style;

	private void Start()
	{
		StartCoroutine(UpdateCoroutine());
	}

	private IEnumerator UpdateCoroutine()
	{
		while (true)
		{
			int lastFrameCount = Time.frameCount;
			float lastTime = Time.realtimeSinceStartup;
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(m_frequency));
			float num = Time.realtimeSinceStartup - lastTime;
			int num2 = Mathf.RoundToInt((float)(Time.frameCount - lastFrameCount) / num);
			m_text = num2 + " FPS";
		}
	}

	public void OnGUI()
	{
		if (m_style == null)
		{
			m_style = new GUIStyle(GUI.skin.button)
			{
				fontSize = 30
			};
		}
		GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, 0f, 200f, 50f));
		GUILayout.Label(m_text, m_style);
		GUILayout.EndArea();
	}
}
