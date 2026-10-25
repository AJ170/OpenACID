using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LocationMap")]
	public class LocationMap : IExtensible
	{
		private Location[] _mapping = new Location[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "mapping", DataFormat = DataFormat.Default)]
		public Location[] mapping
		{
			get
			{
				return _mapping;
			}
			set
			{
				Array.Resize(ref _mapping, value.Length);
				value.CopyTo(_mapping, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
