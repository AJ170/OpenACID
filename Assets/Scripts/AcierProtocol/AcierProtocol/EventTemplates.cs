using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EventTemplates")]
	public class EventTemplates : IExtensible
	{
		private EventTemplate[] _templates = new EventTemplate[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "templates", DataFormat = DataFormat.Default)]
		public EventTemplate[] templates
		{
			get
			{
				return _templates;
			}
			set
			{
				Array.Resize(ref _templates, value.Length);
				value.CopyTo(_templates, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
