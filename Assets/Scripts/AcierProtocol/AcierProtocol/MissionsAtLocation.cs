using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionsAtLocation")]
	public class MissionsAtLocation : IExtensible
	{
		private LocationSet _location;

		private MissionInfo[] _missions = new MissionInfo[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "location", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LocationSet location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}

		[ProtoMember(2, Name = "missions", DataFormat = DataFormat.Default)]
		public MissionInfo[] missions
		{
			get
			{
				return _missions;
			}
			set
			{
				Array.Resize(ref _missions, value.Length);
				value.CopyTo(_missions, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
