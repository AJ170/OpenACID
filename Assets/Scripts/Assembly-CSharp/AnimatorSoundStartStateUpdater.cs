using UnityEngine;

[RequireComponent(typeof(AnimatorDataController), typeof(AnimatorSoundStart))]
public class AnimatorSoundStartStateUpdater : MonoBehaviour
{
	private AnimatorDataController m_animatorDataController;

	private AnimatorSoundStart m_animatorSoundStart;

	private void Awake()
	{
		m_animatorDataController = GetComponent<AnimatorDataController>();
		m_animatorSoundStart = GetComponent<AnimatorSoundStart>();
	}

	private void OnEnable()
	{
		m_animatorSoundStart.Attach();
	}

	private void OnDisable()
	{
		m_animatorSoundStart.Detach();
	}

	public void OnAnimatorMove()
	{
		if (base.enabled)
		{
			m_animatorDataController.UpdateStates();
		}
	}
}
