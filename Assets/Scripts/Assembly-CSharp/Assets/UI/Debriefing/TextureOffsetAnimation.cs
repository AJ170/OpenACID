using UnityEngine;

namespace Assets.UI.Debriefing
{
	public class TextureOffsetAnimation : MonoBehaviour
	{
		public string PropertyName = string.Empty;

		public float Delay;

		public float VerticalStart;

		public float VerticalEnd;

		public float HorizontalStart;

		public float HorizontalEnd;

		public float AnimationLength;

		public bool PlayOnStart = true;

		private float m_startTime;

		public void ActivateEffect()
		{
			m_startTime = Time.realtimeSinceStartup;
			base.transform.GetComponent<Renderer>().material.SetTextureOffset(PropertyName, new Vector2(HorizontalStart, VerticalStart));
		}

		private void Awake()
		{
			base.transform.GetComponent<Renderer>().sortingOrder = 1;
			if (PropertyName == string.Empty)
			{
				base.gameObject.SetActive(false);
			}
		}

		private void OnEnable()
		{
			m_startTime = Time.realtimeSinceStartup + Delay;
			base.transform.GetComponent<Renderer>().material.SetTextureOffset(PropertyName, new Vector2(HorizontalStart, VerticalStart));
		}

		private void Update()
		{
			float num = (Time.realtimeSinceStartup - m_startTime) / AnimationLength;
			Vector2 offset = new Vector2(Mathf.Lerp(VerticalStart, VerticalEnd, num), Mathf.Lerp(HorizontalStart, HorizontalEnd, num));
			base.transform.GetComponent<Renderer>().material.SetTextureOffset(PropertyName, offset);
			if (num > 1f)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
