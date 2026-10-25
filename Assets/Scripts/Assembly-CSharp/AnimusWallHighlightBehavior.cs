using UnityEngine;

public class AnimusWallHighlightBehavior : HighlightBehaviour
{
	private BarrierManager m_barrierManager;

	public BoxCollider BarrierCollider;

	public Barrier Barrier;

	private BarrierManager BarrierManager
	{
		get
		{
			if (m_barrierManager != null)
			{
				return m_barrierManager;
			}
			m_barrierManager = Object.FindObjectOfType<BarrierManager>();
			if (m_barrierManager != null)
			{
				return m_barrierManager;
			}
			GameObject gameObject = new GameObject("BarrierManager");
			m_barrierManager = gameObject.AddComponent<BarrierManager>();
			return m_barrierManager;
		}
	}

	public void Start()
	{
		Barrier.m_size = new Vector2(base.transform.lossyScale.x, base.transform.lossyScale.z * 4f);
		BarrierManager.Init();
	}

	public override void Activate()
	{
	}

	public override void Deactivate()
	{
	}
}
