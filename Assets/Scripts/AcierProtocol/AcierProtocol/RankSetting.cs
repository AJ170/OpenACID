using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "RankSetting")]
	public class RankSetting : IExtensible
	{
		private int _rank;

		private float _probability;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int rank
		{
			get
			{
				return _rank;
			}
			set
			{
				_rank = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "probability", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float probability
		{
			get
			{
				return _probability;
			}
			set
			{
				_probability = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
