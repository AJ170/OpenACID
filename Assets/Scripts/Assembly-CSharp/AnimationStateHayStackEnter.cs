using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationStateHayStackEnter : AnimationStateHideoutEnterBase
{
	public static float MaxSlideDistance = 1f;

	public AnimationStateHayStackEnter(AnimationController _animationController, HideoutObject _hideoutObject, InteractionLocators.Locator _selectedLocator)
		: base(AnimationStateType.HayStackEnter, _animationController, _hideoutObject, _selectedLocator)
	{
		Vector3 position = m_hideoutObject.InteractionLocators.Center.transform.position;
		Vector3 position2 = _selectedLocator.Step.transform.position;
		Vector3 nextPosition = AnimationController.NextPosition;
		m_CurrentMatchTarget = Slide(position2, position, nextPosition, MaxSlideDistance);
		m_faceDirection = position - nextPosition;
		m_faceDirection.y = 0f;
		m_faceDirection.Normalize();
	}

	protected override Vector3 GetHideoutCenter()
	{
		return m_hideoutObject.InteractionLocators.Center.transform.position;
	}

	protected override void OnStartJump()
	{
		Haystack haystack = (Haystack)m_hideoutObject;
		haystack.TriggerJumpInAnimation();
	}
}
