using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Events")]
	public class Events : IExtensible
	{
		private Event[] _events = new Event[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "events", DataFormat = DataFormat.Default)]
		public Event[] events
		{
			get
			{
				return _events;
			}
			set
			{
				Array.Resize(ref _events, value.Length);
				value.CopyTo(_events, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
