using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.UI;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class ApproachEntityIcon : EntityIcon
{
	public const float Tolerance = 0.05f;

	public override bool IsHiddenOnBorders
	{
		get
		{
			if (EntityId.Equals(UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.SelectedEntityId))
			{
				return false;
			}
			return base.IsHiddenOnBorders;
		}
		set
		{
			base.IsHiddenOnBorders = value;
		}
	}

	public bool IsIndistance
	{
		get
		{
			if (EntityId.Equals(UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.SelectedEntityId))
			{
				return true;
			}
			float num = ((!base.Visible) ? 0f : 0.05f);
			Vector3 position = LogicMain.PlayerEntity.Position;
			Vector3 position2 = base.HeadIcon.GameEntity.Position;
			float num2 = (position - position2).LengthXZ();
			if (num2 > 10f + num || position.y - position2.y > ParcourHelper.Acrobatics.JumpMaxFallHeightNoDamage + num)
			{
				return IsDistanceCheckSkipped(num2);
			}
			return true;
		}
	}

	public ApproachEntityIcon()
	{
		base.VisibilityPredicate = () => IsIndistance && AcierInputHandler.Instance.MoveInterpreter.InputEnabled;
		base.SparseUpdateIntervalSeconds = 0.2f;
	}

	public virtual bool IsDistanceCheckSkipped(float _planeDistance)
	{
		return false;
	}
}
