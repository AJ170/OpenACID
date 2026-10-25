using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Acier
{
	public class MoveInterpreterDecalAnimation : MonoBehaviour
	{
		private const float FadeDuration = 0.4f;

		private float m_time;

		private Renderer m_renderer;

		public void StartAnimation()
		{
			m_time = 0.4f;
		}

		private void Awake()
		{
			m_renderer = GetComponent<Renderer>();
		}

		private void Update()
		{
			int nameID = Shader.PropertyToID("_TintColor");
			Material material = m_renderer.material;
			Color color = material.GetColor(nameID);
			float num = Mathf.Max(0.4f - m_time, 0f) / 0.4f;
			float num2 = num * num;
			float num3 = num2 * num;
			float num4 = 1f - num;
			float a = num4 * num4;
			float num5 = num3 * 4f;
			base.transform.localScale = new Vector3(1f + num5, 1f + num5, 1f + num5);
			color.a = a;
			material.SetColor(nameID, color);
			m_time -= UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			if (m_time < 0f)
			{
				base.gameObject.SetActive(false);
			}
			else
			{
				base.transform.localRotation = Quaternion.LookRotation(base.transform.position - Camera.main.transform.position, Vector3.up);
			}
		}
	}
}
