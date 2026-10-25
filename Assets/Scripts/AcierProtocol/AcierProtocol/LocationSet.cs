using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LocationSet")]
	public class LocationSet : IExtensible
	{
		private EEra _era;

		private EMood _mood;

		private ERegion _region;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "era", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EEra.EEra_None)]
		public EEra era
		{
			get
			{
				return _era;
			}
			set
			{
				_era = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "mood", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMood.Daytime)]
		public EMood mood
		{
			get
			{
				return _mood;
			}
			set
			{
				_mood = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "region", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ERegion.Animus)]
		public ERegion region
		{
			get
			{
				return _region;
			}
			set
			{
				_region = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
