using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWrapperScalable_Float : StatValueWrapper_Float, IDataSerializable, IStatValueWrapper, INumberStatValueWrapper, IScalableNumberStatValueWrapper
	{
		public float LevelIncrement { get; set; }

		public override IStatValueWrapper Clone()
		{
			StatValueWrapperScalable_Float statValueWrapperScalable_Float = new StatValueWrapperScalable_Float();
			statValueWrapperScalable_Float.Float = Float;
			statValueWrapperScalable_Float.LevelIncrement = LevelIncrement;
			return statValueWrapperScalable_Float;
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

		public override bool Equals(object obj)
		{
			return base.Equals(obj) && LevelIncrement == ((IScalableNumberStatValueWrapper)obj).LevelIncrement;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			base.Serialize(_name, _s, _customData);
			LevelIncrement = XmlHelpers.Serialize("levelIncrement", _s, LevelIncrement);
		}
	}
}
