using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationStateRooftopGardenEnter : AnimationStateHideoutEnterBase
{
	public static float MaxSlideDistance = 0.5f;

	private RooftopGarden m_rooftopGarden;

	private InteractionLocators.Locator m_locator;

	public AnimationStateRooftopGardenEnter(AnimationController _animationController, RooftopGarden _rooftopGarden, InteractionLocators.Locator _selectedLocator)
		: base(AnimationStateType.RooftopGardenEnter, _animationController, _rooftopGarden, _selectedLocator)
	{
		m_rooftopGarden = _rooftopGarden;
		m_locator = _selectedLocator;
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
		return m_hideoutObject.InteractionLocators.Rest.transform.position;
	}

	public override bool OnAnimatorEvent(AnimatorEvent _event)
	{
		base.OnAnimatorEvent(_event);
		if (_event.Type == AnimatorEventType.RooftopClothesEnter)
		{
			m_rooftopGarden.RooftopGardenClothes.JumpIn(m_locator);
		}
		return false;
	}
}
