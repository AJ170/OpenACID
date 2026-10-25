using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IEntityStats : IInstanceCarrier, IInstanceStatValueCollection, IReadonlyStatValueCollection, IStatValueCollection
	{
	}
}
