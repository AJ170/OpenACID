using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IInstanceStatValueCollection : IInstanceCarrier, IReadonlyStatValueCollection, IStatValueCollection
	{
		float IncrementFactor { get; }
	}
}
