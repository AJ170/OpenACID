using SharedBaseLib.Core;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
	private BBList<Barrier> m_barriers;

	[SerializeField]
	private float m_cullingWidth = 40f;

	[SerializeField]
	private float m_cullingDepth = 40f;

	private bool m_visible = true;

	public bool IsVisible
	{
		get
		{
			return m_visible;
		}
	}

	public void Start()
	{
		Init();
	}

	public void Init()
	{
		m_barriers = new BBList<Barrier>();
		Barrier[] array = Object.FindObjectsOfType<Barrier>();
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			Barrier barrier = array[i];
			barrier.Build();
			m_barriers.Add(barrier);
		}
		if (Application.platform != RuntimePlatform.Android)
		{
			StaticBatchingUtility.Combine(base.gameObject);
		}
	}

	public void SetVisible(bool _visible)
	{
		m_visible = _visible;
	}

	private void Update()
	{
		Camera main = Camera.main;
		if (main != null)
		{
			Cull(main);
		}
	}

	private void Cull(Camera c)
	{
		Vector3 position = c.transform.position;
		for (int i = 0; i < m_barriers.Count; i++)
		{
			Barrier barrier = m_barriers[i];
			Vector3 vector = barrier.CachedTransform.InverseTransformPoint(position);
			Vector2 maxSize = barrier.MaxSize;
			bool flag = vector.x < 0f - maxSize.x - m_cullingWidth || vector.x > maxSize.x + m_cullingWidth || vector.z < 0f - m_cullingDepth || vector.z > m_cullingDepth;
			flag |= !m_visible;
			if (barrier.CachedRenderer.enabled != !flag)
			{
				barrier.CachedRenderer.enabled = !flag;
			}
		}
	}
}
