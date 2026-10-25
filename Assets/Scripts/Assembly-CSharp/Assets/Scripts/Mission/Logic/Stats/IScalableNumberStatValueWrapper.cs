using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IScalableNumberStatValueWrapper : IDataSerializable, IStatValueWrapper, INumberStatValueWrapper
	{
		float LevelIncrement { get; set; }
	}
}
