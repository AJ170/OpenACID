using System.Collections;
using SharedBaseLib.Core;
using UbiCore;
using UnityEngine;

public class AnimationPlotStateWaitForMovementStopped : AnimationPlotStateBase
{
	private BBList<AnimationController> m_animationControllers;

	private float m_maxWaitTimeSec = 2f;

	private float m_checkFrequencySec = 0.2f;

	public AnimationPlotStateWaitForMovementStopped(BBList<AnimationController> _animationControllers)
	{
		m_animationControllers = _animationControllers;
	}

	public override void OnStart()
	{
		base.OnStart();
		Main.Instance.StartCoroutine(WaitForStop(this, m_checkFrequencySec));
	}

	public bool CheckIfFinished()
	{
		bool flag = true;
		if (m_maxWaitTimeSec > 0f)
		{
			for (int i = 0; i < m_animationControllers.Count; i++)
			{
				AnimationController animationController = m_animationControllers[i];
				if (!MathHelper.Approximately(0f, animationController.Animator.deltaPosition.magnitude))
				{
					flag = false;
					break;
				}
			}
			m_maxWaitTimeSec -= m_checkFrequencySec;
		}
		if (flag)
		{
			base.Finished = true;
			base.Owner.OnStateFinished(this);
		}
		return flag;
	}

	private IEnumerator WaitForStop(AnimationPlotStateWaitForMovementStopped _state, float _waitSec)
	{
		bool done = false;
		while (!done)
		{
			done = _state.CheckIfFinished();
			if (!done)
			{
				yield return new WaitForSeconds(_waitSec);
			}
		}
	}
}
