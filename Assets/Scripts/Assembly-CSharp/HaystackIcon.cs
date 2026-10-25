using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion;
using UnityEngine;

public class HaystackIcon : ApproachEntityIcon
{
	public override bool IsDistanceCheckSkipped(float _planeDistance)
	{
		IconMaterial topIcon = base.TopIcon;
		if (topIcon == IconMaterial.InteractLeapOfFaith)
		{
			return true;
		}
		float num = ((!base.Visible) ? 0f : 0.05f);
		if (_planeDistance > 15f + num)
		{
			return false;
		}
		Vector3 _edgePosition;
		if (topIcon == IconMaterial.InteractHaystack)
		{
			return ParcourHelper.CanLeapOfFaith(LogicMain.PlayerEntity, (Haystack)EntityId.Entity, out _edgePosition);
		}
		return false;
	}

	protected override void UpdateVisibilityInternal()
	{
		base.UpdateVisibilityInternal();
		if (base.Visible)
		{
			Vector3 _edgePosition;
			if (ParcourHelper.CanLeapOfFaith(LogicMain.PlayerEntity, (Haystack)EntityId.Entity, out _edgePosition))
			{
				SetIcon(IconMaterial.InteractLeapOfFaith, 5);
			}
			else
			{
				SetIcon(IconMaterial.InteractHaystack, 5);
			}
		}
	}
}
