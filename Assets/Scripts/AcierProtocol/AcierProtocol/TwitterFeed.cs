using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TwitterFeed")]
	public class TwitterFeed : IExtensible
	{
		private TwitterStatus[] _statusMessages = new TwitterStatus[0];

		private string _screenName = "";

		private IExtension extensionObject;

		[ProtoMember(1, Name = "statusMessages", DataFormat = DataFormat.Default)]
		public TwitterStatus[] statusMessages
		{
			get
			{
				return _statusMessages;
			}
			set
			{
				Array.Resize(ref _statusMessages, value.Length);
				value.CopyTo(_statusMessages, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "screenName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string screenName
		{
			get
			{
				return _screenName;
			}
			set
			{
				_screenName = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
