using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetTwitterTimelineResponse")]
	public class GetTwitterTimelineResponse : IExtensible
	{
		private TwitterFeed _twitterFeed;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "twitterFeed", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TwitterFeed twitterFeed
		{
			get
			{
				return _twitterFeed;
			}
			set
			{
				_twitterFeed = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
