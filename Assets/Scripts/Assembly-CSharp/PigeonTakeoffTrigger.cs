using System.Collections;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PigeonTakeoffTrigger : MonoBehaviour
{
	private Animator m_Animator;

	private SkinnedMeshRenderer m_Renderer;

	private AnimatorSoundStartStateUpdater m_soundStartStateUpdater;

	private static int s_TakeOffIDHash = Animator.StringToHash("TakeOffID");

	private static int s_PeckingHash = Animator.StringToHash("StartPecking");

	private static int s_NumberOfTakeOffs = 3;

	private static float s_ResetDelay = 20f;

	private static float s_HideDelay = 3f;

	private int m_triggerRefCount;

	public void Start()
	{
		m_Animator = base.gameObject.GetComponent<Animator>();
		m_Renderer = base.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		m_soundStartStateUpdater = base.gameObject.GetComponent<AnimatorSoundStartStateUpdater>();
		m_Animator.Play("PigeonIdle", 0, Random.Range(0f, 1f));
		StartCoroutine(PeckingCycle());
	}

	public void OnTriggerEnter(Collider other)
	{
		if (m_triggerRefCount == 0)
		{
			if (m_soundStartStateUpdater != null)
			{
				m_soundStartStateUpdater.enabled = true;
			}
			m_Animator.SetInteger(s_TakeOffIDHash, Random.Range(1, s_NumberOfTakeOffs));
			Invoke("Hide", s_HideDelay);
			if (other != null)
			{
				GameObject gameObject = other.gameObject;
				if (gameObject != null)
				{
					GameObjectEntity gameObjectEntity = gameObject.TryGetEntity();
					if (gameObjectEntity == LogicMain.PlayerEntity)
					{
						LogicMain.MissionResult.IncreaseFlushedPigeons();
					}
				}
			}
		}
		m_triggerRefCount++;
	}

	public void OnTriggerExit(Collider other)
	{
		m_triggerRefCount--;
		if (m_triggerRefCount == 0)
		{
			if (m_soundStartStateUpdater != null)
			{
				m_soundStartStateUpdater.enabled = false;
			}
			CancelInvoke("Reset");
			Invoke("Reset", s_ResetDelay);
		}
	}

	public void Reset()
	{
		if (Camera.main != null)
		{
			Vector3 point = Camera.main.WorldToViewportPoint(base.transform.position);
			if (point.z > 0f && new Rect(0f, 0f, 1f, 1f).Contains(point))
			{
				CancelInvoke("Reset");
				Invoke("Reset", s_ResetDelay);
				return;
			}
		}
		m_Animator.SetInteger(s_TakeOffIDHash, 0);
		m_Renderer.enabled = true;
	}

	public void Hide()
	{
		m_Renderer.enabled = false;
	}

	private IEnumerator PeckingCycle()
	{
		while (true)
		{
			if (Random.Range(0, 25) == 0)
			{
				m_Animator.SetTrigger(s_PeckingHash);
			}
			yield return new WaitForSeconds(0.75f);
		}
	}

	public void OnDestroy()
	{
		StopAllCoroutines();
	}
}
