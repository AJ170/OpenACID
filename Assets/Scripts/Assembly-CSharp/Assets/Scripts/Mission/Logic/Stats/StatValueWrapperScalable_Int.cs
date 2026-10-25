using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWrapperScalable_Int : StatValueWrapper_Int, IDataSerializable, IStatValueWrapper, INumberStatValueWrapper, IScalableNumberStatValueWrapper
	{
		public float LevelIncrement { get; set; }

		public override IStatValueWrapper Clone()
		{
			StatValueWrapperScalable_Int statValueWrapperScalable_Int = new StatValueWrapperScalable_Int();
			statValueWrapperScalable_Int.Int = Int;
			statValueWrapperScalable_Int.LevelIncrement = LevelIncrement;
			return statValueWrapperScalable_Int;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj) && LevelIncrement == ((IScalableNumberStatValueWrapper)obj).LevelIncrement;
		}

		public override void CopyFrom(IStatValueWrapper _wrapper)
		{
			base.CopyFrom(_wrapper);
			IScalableNumberStatValueWrapper scalableNumberStatValueWrapper = _wrapper as IScalableNumberStatValueWrapper;
			if (scalableNumberStatValueWrapper != null)
			{
				LevelIncrement = scalableNumberStatValueWrapper.LevelIncrement;
			}
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			base.Serialize(_name, _s, _customData);
			LevelIncrement = XmlHelpers.Serialize("levelIncrement", _s, LevelIncrement);
		}
	}
}
