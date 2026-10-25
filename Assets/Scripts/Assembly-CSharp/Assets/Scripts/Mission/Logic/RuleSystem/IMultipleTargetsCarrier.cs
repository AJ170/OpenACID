using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface IMultipleTargetsCarrier
	{
		BBList<EntityId> TargetEntityIds { get; }
	}
}
