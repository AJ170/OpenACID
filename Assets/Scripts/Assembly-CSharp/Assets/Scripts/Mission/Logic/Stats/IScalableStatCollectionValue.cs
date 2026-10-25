namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IScalableStatCollectionValue : IReadonlyStatCollectionValue, IStatCollectionValue, IReadonlyStatValueBase, IStatValueBase
	{
		void SetLevelIncrement(int _idx, float _levelIncrement);

		float GetLevelIncrement(int _idx);
	}
}
