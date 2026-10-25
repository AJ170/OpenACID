using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class AnimationMain : UbiCore.Utils.Singleton<AnimationMain>
{
	private BBDictionary<int, AnimatorStateMachineStatic> m_AnimatorStateMachineStatic;

	public AnimationMain()
	{
		m_AnimatorStateMachineStatic = new BBDictionary<int, AnimatorStateMachineStatic>();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		m_AnimatorStateMachineStatic = null;
	}

	public AnimatorStateMachineStatic GetAnimatorStateMachineStatic(Animator _animator)
	{
		RuntimeAnimatorController runtimeAnimatorController = _animator.runtimeAnimatorController;
		if (runtimeAnimatorController is AnimatorOverrideController)
		{
			runtimeAnimatorController = ((AnimatorOverrideController)runtimeAnimatorController).runtimeAnimatorController;
		}
		string name = runtimeAnimatorController.name;
		int key = Animator.StringToHash(name);
		AnimatorStateMachineStatic value = null;
		if (!m_AnimatorStateMachineStatic.TryGetValue(key, out value))
		{
			value = new AnimatorStateMachineStatic(name);
			m_AnimatorStateMachineStatic.Add(key, value);
		}
		return value;
	}
}
