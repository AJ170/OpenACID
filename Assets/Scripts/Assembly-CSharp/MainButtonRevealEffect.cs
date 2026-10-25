using UnityEngine;

public class MainButtonRevealEffect : MonoBehaviour
{
	[SerializeField]
	private UIWidget m_uiWidget;

	[SerializeField]
	private DynamicSwipe m_dynamicSwipe;

	[SerializeField]
	private ParticleSystem m_revealEffect;

	private void Awake()
	{
	}

	public void StartRevealEffect()
	{
		m_revealEffect.transform.localPosition = new Vector3((float)m_uiWidget.width / 2f, (float)(-m_uiWidget.height) / 2f, 0f);
		m_dynamicSwipe.InitializeDoubleSkew(true, m_uiWidget.width, m_uiWidget.height);
		m_dynamicSwipe.Animate();
		m_revealEffect.Play(true);
	}
}
